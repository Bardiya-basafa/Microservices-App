# EShop Microservices

A **microservices-based e-commerce back end** built with **.NET 9**. It shows a range of modern back-end architecture styles and patterns side by side, one per service:

| Service | Architecture style | Data store | Protocol |
|---|---|---|---|
| **Catalog** | Vertical Slice + CQRS | PostgreSQL as a **document DB** (Marten) | REST (Minimal APIs / Carter) |
| **Basket** | Vertical Slice + CQRS + Decorator cache | PostgreSQL (Marten) + **Redis** | REST + **gRPC client** + **RabbitMQ publisher** |
| **Discount** | Simple gRPC service | **SQLite** (EF Core) | **gRPC** |
| **Ordering** | **Clean Architecture + Domain-Driven Design** | **SQL Server** (EF Core) | REST + **RabbitMQ consumer** |

Shared cross-cutting code (CQRS abstractions, MediatR pipeline behaviors, pagination, exceptions and messaging) lives in **Building Blocks** libraries.

> **Status:** Catalog, Basket and Discount are implemented end to end. Ordering has a complete domain model, persistence layer and API surface, while DTO mapping and the checkout-event consumer are still in progress (see [Roadmap](#roadmap)).

---

## Table of Contents

- [Architecture Overview](#architecture-overview)
- [Tech Stack](#tech-stack)
- [Solution Structure](#solution-structure)
- [Building Blocks](#building-blocks)
- [Catalog Service](#catalog-service)
- [Basket Service](#basket-service)
- [Discount Service (gRPC)](#discount-service-grpc)
- [Ordering Service (Clean Architecture + DDD)](#ordering-service-clean-architecture--ddd)
- [End-to-End Flows](#end-to-end-flows)
- [API Reference](#api-reference)
- [Running Locally](#running-locally)
- [Roadmap](#roadmap)

---

## Architecture Overview

```
                    ┌──────────────────────────────────────────────────────────┐
                    │                       Clients                            │
                    └───────┬─────────────────────┬──────────────────┬─────────┘
                            │ REST                │ REST             │ REST
                 ┌──────────▼─────────┐ ┌─────────▼─────────┐ ┌──────▼──────────────┐
                 │   Catalog.API      │ │   Basket.API      │ │   Ordering.API      │
                 │ Vertical slices    │ │ Vertical slices   │ │ Clean Arch + DDD    │
                 │ Carter + MediatR   │ │ Carter + MediatR  │ │ Carter + MediatR    │
                 └──────────┬─────────┘ └──┬──────┬──────┬─┘ └──────▲───────┬──────┘
                            │              │      │      │          │       │
                     ┌──────▼──────┐ ┌─────▼──┐ ┌─▼────┐ │ gRPC     │       │
                     │ PostgreSQL  │ │Postgres│ │Redis │ │          │  ┌────▼──────┐
                     │ (Marten)    │ │(Marten)│ │cache │ │          │  │SQL Server │
                     │ CatalogDb   │ │BasketDb│ │      │ │          │  │ OrderDb   │
                     └─────────────┘ └────────┘ └──────┘ │          │  └───────────┘
                                                ┌────────▼───────┐  │
                                                │ Discount.Grpc  │  │
                                                │ SQLite         │  │
                                                └────────────────┘  │
                                                                    │
                    Basket ── BasketCheckoutEvent ──► RabbitMQ ─────┘ (MassTransit)
```

**Communication styles**
- **Synchronous REST** between clients and services (Minimal APIs through Carter).
- **Synchronous gRPC** between services: Basket calls Discount for low-latency, strongly typed requests.
- **Asynchronous messaging**: Basket publishes `BasketCheckoutEvent` to RabbitMQ and Ordering consumes it (MassTransit).

---

## Tech Stack

| Concern | Technology |
|---|---|
| Runtime | .NET 9, ASP.NET Core Minimal APIs |
| Endpoint routing | **Carter** (`ICarterModule`) |
| CQRS / mediator | **MediatR** (commands, queries, notifications, pipeline behaviors) |
| Validation | **FluentValidation** |
| Mapping | **Mapster** |
| Document database | **Marten** on PostgreSQL |
| Relational ORM | **EF Core 9** (SQL Server, SQLite) |
| Distributed cache | **Redis** (`IDistributedCache`) |
| Decorator DI | **Scrutor** |
| Inter-service RPC | **gRPC** (`Grpc.AspNetCore`, `Grpc.Net.ClientFactory`) + Protocol Buffers |
| Messaging | **RabbitMQ** + **MassTransit** |
| Health checks | `AspNetCore.HealthChecks.*` (Npgsql, Redis, SQL Server) + HealthChecks UI JSON writer |
| Containers | Docker, Docker Compose |
| Code quality | Qodana configuration |

---

## Solution Structure

```
src/
├── EShopMicroservices.sln
├── compose.yaml
├── global.json
├── BuildingBlocks/                         # Shared kernel
│   ├── CQRS/                               # ICommand, IQuery, handlers
│   ├── Behaviors/                          # ValidationBehavior, LoggingBehavior
│   ├── Exceptions/                         # NotFoundException
│   └── Pagination/                         # PaginationRequest, PaginatedResult<T>
├── BuildinBlocks/
│   └── BuildingBlocks.Messaging/           # IntegrationEvent, BasketCheckoutEvent, MassTransit setup
└── Services/
    ├── Catalog/Catalog.API/                # Products feature slices
    ├── Basket/Basket.API/                  # Basket feature slices, repository + cache
    ├── Discount/Discount.Grpc/             # discount.proto, DiscountService, SQLite
    └── Ordering/
        ├── Ordering.API/                   # Carter endpoints, health checks
        ├── Ordering.Application/           # Commands, queries, validators, event handlers
        ├── Ordering.Domain/                # Aggregates, entities, value objects, domain events
        └── Ordering.Infrastructure/        # EF Core DbContext, configurations, interceptors, seed
```

---

## Building Blocks

### CQRS abstractions

```csharp
public interface ICommand : ICommand<Unit> { }
public interface ICommand<out TResult> : IRequest<TResult> { }
public interface IQuery<out TResult> : IRequest<TResult> where TResult : notnull { }

public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult> where TResult : notnull { }
public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult> where TResult : notnull { }
```

Separate interfaces for commands and queries make intent explicit and let pipeline behaviors target **only commands**.

### MediatR pipeline behaviors

| Behavior | What it does |
|---|---|
| `ValidationBehavior<TRequest, TResponse>` | Constrained to `ICommand<TResponse>`. Runs **every registered FluentValidation validator in parallel** (`Task.WhenAll`), collects the failures and throws `ValidationException` before the handler runs |
| `LoggingBehavior<TRequest, TResponse>` | Logs `[START]` and `[END]` with request and response types and request data, times the handler with a `Stopwatch`, and logs a **`[PERFORMANCE]` warning when a request takes more than 3 seconds** |

### Pagination

```csharp
public record PaginationRequest { int PageIndex = 0; int PageSize = 10; }
public record PaginatedResult<T> { int PageIndex; int PageSize; long Count; List<T> Data; }
```

### Messaging (`BuildingBlocks.Messaging`)

- `IntegrationEvent`: base record with `Id`, `OccurredOn` and `EventType`.
- `BasketCheckoutEvent`: user name, customer ID, total price, billing address and payment details.
- `AddMessagingBroker(configuration, assembly?)`: a one-line MassTransit setup with RabbitMQ, a **kebab-case endpoint formatter**, automatic consumer registration from an assembly, and host and credentials from `MessagingBroker:*` settings.

---

## Catalog Service

The product catalog, built with **Vertical Slice Architecture**: each feature is a folder holding its **endpoint**, **request/response records**, **command/query**, **validator** and **handler**, so a change to one feature touches one place.

```
Products/
├── CreateProduct/        CreateProductEndpoint.cs + CreateProductHandler.cs
├── GetProducts/
├── GetProductById/
├── GetProductByCategory/
├── UpdateProduct/
└── DeleteProduct/
```

### Persistence: Marten (PostgreSQL as a document database)

```csharp
builder.Services.AddMarten(o => o.Connection(connectionString)).UseLightweightSessions();
```

`Product { Guid Id; string Name; string Description; decimal Price; string ImageUrl; List<string> Categories; }` is stored as **JSONB**. Marten generates the IDs and supports LINQ queries over document fields, for example a case-insensitive category match:

```csharp
session.Query<Product>()
       .Where(p => p.Categories.Contains(category, StringComparer.OrdinalIgnoreCase))
```

### Validation rules (`CreateProductValidator`)

| Field | Rule |
|---|---|
| Name | required, ≤ 50 characters |
| Price | required, between 0 and 3000 |
| Description | required, ≤ 255 characters |
| ImageUrl | required, ≤ 255 characters |

### Endpoint example

```csharp
app.MapPost("/product", async (CreateProductRequest request, ISender sender) => {
        var command = request.Adapt<CreateProductCommand>();
        var result  = await sender.Send(command);
        return Results.Created($"/product/{result.Id}", result.Adapt<CreateProductResponse>());
    })
   .WithName("CreateProduct")
   .Produces<CreateProductResponse>(StatusCodes.Status201Created)
   .ProducesProblem(StatusCodes.Status400BadRequest);
```

---

## Basket Service

The shopping basket. Like Catalog, it uses Vertical Slice + CQRS, plus a **Redis cache decorator**, a **gRPC client** to the Discount service and an **event publisher**.

### Model

```csharp
public class Basket {
    [Identity] public string UserName { get; set; }      // the document ID is the user name
    public List<BasketItem> Items { get; set; }
    public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);
}
public class BasketItem { int Quantity; string Color; decimal Price; Guid ProductId; string ProductName; }
```

### Repository + cache decorator (Scrutor)

```csharp
builder.Services.AddScoped<IBasketRepository, BasketRepository>();          // Marten
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();     // Redis
```

| Operation | `CachedBasketRepository` behaviour |
|---|---|
| `GetBasket` | Return from Redis on a hit. On a miss, load from Marten and write to Redis |
| `StoreBasket` | Write through: Marten, then Redis |
| `DeleteBasket` | Delete from Marten, then evict from Redis |

The handlers only know `IBasketRepository`. Caching is added **transparently** without changing them (Open/Closed principle).

### Discount applied through gRPC

When a basket is stored, `StoreBasketHandler` calls the Discount service **for each item** and subtracts the coupon amount:

```csharp
foreach (var item in request.Cart.Items) {
    var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName });
    item.Price -= decimal.Parse(coupon.Amount);
}
```

The gRPC client is registered with `AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>()` and the address `GrpcClient:DiscountAddress`.

### Checkout: publishing an integration event

`CheckoutBasketHandler`:
1. Loads the basket (returns `IsSuccess = false` if it does not exist).
2. Maps the checkout DTO to a `BasketCheckoutEvent` and sets `TotalPrice` from the basket.
3. **Publishes** the event to RabbitMQ through MassTransit `IPublishEndpoint`.
4. Deletes the basket.

Validation: `CheckoutBasket` must not be null and `UserName` is required.

### Health checks

`GET /health` checks **PostgreSQL** and **Redis** and returns the HealthChecks UI JSON format.

---

## Discount Service (gRPC)

A small **gRPC** service that manages product coupons.

### Contract (`Protos/discount.proto`)

```protobuf
service DiscountProtoService {
  rpc GetDiscount    (GetDiscountRequest)    returns (CouponModel);
  rpc CreateDiscount (CreateDiscountRequest) returns (CouponModel);
  rpc UpdateDiscount (UpdateDiscountRequest) returns (CouponModel);
  rpc DeleteDiscount (DeleteDiscountRequest) returns (DeleteDiscountResponse);
}
message CouponModel { int32 id = 1; string productName = 2; string description = 3; string amount = 4; }
```

### Implementation
- **EF Core + SQLite** (`Data Source=DiscountDb`), a file-based database that needs no extra container.
- **Seeded coupons:** `S23` → 10 off, `16ProMax` → 20 off.
- Migrations run automatically on startup (`app.UseMigration()`).
- `GetDiscount` returns a **zero-amount "no discount" coupon** instead of an error when a product has no coupon, so Basket never fails because of a missing discount.
- Mapster maps between `Coupon` entities and the generated `CouponModel` messages.

---

## Ordering Service (Clean Architecture + DDD)

The most complex service. It follows **Clean Architecture** with a rich **Domain-Driven Design** model.

```
Ordering.API            → Carter endpoints, health checks (composition root)
Ordering.Application    → CQRS commands/queries, validators, domain & integration event handlers,
                          IAppDbContext abstraction, DTOs
Ordering.Domain         → Aggregates, entities, value objects, domain events (no dependencies)
Ordering.Infrastructure → EF Core AppDbContext, entity configurations, interceptors, seeding
```

Dependencies point **inward**: Infrastructure and API depend on Application, which depends on Domain.

### Domain model

```
            ┌───────────────────────────────────────────────┐
            │  Order : Aggregate<OrderId>   (aggregate root)│
            │  CustomerId, OrderName, Status, TotalPrice    │
            │  ShippingAddress, BillingAddress, Payment     │  ← value objects
            │  ─ Create(...)  → raises OrderCreatedEvent    │
            │  ─ Update(...)  → raises OrderUpdatedEvent    │
            │  ─ AddItem(productId, qty, price)             │
            └──────────────┬────────────────────────────────┘
                           │ 1..*  (only through the aggregate)
                 ┌─────────▼─────────┐        ┌────────────────────┐
                 │ OrderItem         │───────►│ Product            │
                 │ Entity<OrderItemId>│       │ Entity<ProductId>  │
                 └───────────────────┘        └────────────────────┘
   Customer : Entity<CustomerId>  (Name, Email unique)
```

**Building blocks of the domain**

| Concept | Implementation |
|---|---|
| `Entity<TId>` | ID plus audit fields `CreatedAt`, `CreatedBy`, `LastModified`, `LastModifiedBy` |
| `Aggregate<TId>` | Entity plus a list of **domain events** (`AddDomainEvent`, `ClearDomainEvents`) |
| `IDomainEvent` | Extends MediatR `INotification` with `EventId`, `OccurredOn` and `EventType` |
| **Strongly typed IDs** | `OrderId`, `CustomerId`, `OrderItemId`, `ProductId`: records that wrap a `Guid`, with an `Of()` factory that **rejects `Guid.Empty`**. The compiler stops you from passing a `CustomerId` where an `OrderId` is expected |
| **Value objects** | `OrderName`, `Address` (requires e-mail and address line), `Payment` (requires card name, number and CVV, with **CVV ≤ 3 characters**) |
| **Invariants** | `Order.AddItem` rejects quantity or price ≤ 0. `Product.Create` rejects an empty name or a price ≤ 0. `Customer.Create` requires e-mail and name |
| **Encapsulation** | `OrderItems` is exposed as `IReadOnlyList` backed by a private list, setters are private, and `OrderItem` has an `internal` constructor, so items can only be added through the aggregate root |
| `OrderStatus` | `Draft`, `Pending`, `Shipped`, `Delivered`, `Canceled` |

### Persistence (EF Core + SQL Server)

- `IAppDbContext` (Application layer) hides EF Core behind an interface. `AppDbContext` implements it in Infrastructure.
- **Fluent configurations** per entity (`IEntityTypeConfiguration<T>`), loaded with `ApplyConfigurationsFromAssembly`.
- **Value converters** for every strongly typed ID (`OrderId` ↔ `Guid`).
- **EF Core 8+ complex properties** map the `OrderName`, `ShippingAddress`, `BillingAddress` and `Payment` value objects into columns of the `Orders` table, with length limits and required flags.
- `Status` is stored as a **string** (enum ↔ string conversion) with a default of `Draft`.
- Relationships: Order → Customer (required), Order → OrderItems (`NoAction`), OrderItem → Product (`Restrict`). Customer e-mail has a **unique index**.

### EF Core interceptors

| Interceptor | Responsibility |
|---|---|
| `AuditableEntityInterceptor` | Before `SaveChanges`, fills `CreatedAt`/`CreatedBy` for added entities and `LastModified`/`LastModifiedBy` for modified ones, **including changes to owned entities** (`HasChangedOwnedEntities`) |
| `DispatchDomainEventsInterceptor` | Collects domain events from tracked aggregates, clears them, and publishes each one through **MediatR** (`INotificationHandler<OrderCreatedEvent>`, …) |

### Application layer

| Type | Name | Notes |
|---|---|---|
| Command | `CreateOrderCommand` | Builds the `Order` aggregate (addresses, payment, items) from `OrderDto` |
| Command | `UpdateOrderCommand` | Loads the order, calls `order.Update(...)` (raises `OrderUpdatedEvent`) |
| Command | `DeleteOrderCommand` | Throws `OrderNotFoundException` (a `NotFoundException`) when the order is missing |
| Query | `GetOrdersQuery` | **Paginated** (`Skip`/`Take`) with a total count, `AsNoTracking` |
| Query | `GetOrderByCustomerQuery` | Orders for a customer, oldest first |
| Query | `GetOrderByNameQuery` | Orders whose name contains a search term |
| Domain event handler | `OrderCreatedEventHandler`, `OrderUpdatedEventHandler` | Log the event. The update handler also publishes an integration event through MassTransit |
| Integration consumer | `BasketCheckoutEventHandler` | MassTransit `IConsumer<BasketCheckoutEvent>`: turns a basket checkout into a `CreateOrderCommand` |

MediatR is registered with both open behaviors:
```csharp
cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
```

### Seeding

In Development, `app.InitializeDatabase()` applies migrations and seeds customers and products when the tables are empty.

---

## End-to-End Flows

### Add to basket (with discount)

```
POST /basket { cart }
  → Carter endpoint → MediatR → StoreBasketHandler
      → for each item: gRPC Discount.GetDiscount(productName) → price -= amount
      → CachedBasketRepository.StoreBasket
            → Marten (PostgreSQL JSONB)
            → Redis SET userName
  ← 201 Created /basket/{userName}
```

### Checkout → order (event-driven)

```
POST /basket/checkout { checkoutDto }
  → CheckoutBasketHandler
      → load basket → map to BasketCheckoutEvent (TotalPrice from basket)
      → MassTransit Publish → RabbitMQ (exchange: BasketCheckoutEvent)
      → delete basket (Marten + Redis)
  ← 200 { isSuccess: true }

RabbitMQ → Ordering: BasketCheckoutEventHandler (IConsumer)
      → CreateOrderCommand → ValidationBehavior → LoggingBehavior → CreateOrderHandler
      → Order.Create(...) raises OrderCreatedEvent
      → SaveChanges → AuditableEntityInterceptor + DispatchDomainEventsInterceptor
      → OrderCreatedEventHandler
```

---

## API Reference

### Catalog.API (`http://localhost:6000`)

| Method | Route | Description |
|---|---|---|
| GET | `/products` | List products |
| GET | `/products/{id}` | Get a product (404-style `ProductNotFoundException`) |
| GET | `/product/category/{category}` | Products in a category |
| POST | `/product` | Create a product → `201 Created` |
| PUT | `/products` | Update a product |
| DELETE | `/products/{id}` | Delete a product |

### Basket.API (`http://localhost:6001`)

| Method | Route | Description |
|---|---|---|
| GET | `/basket/{userName}` | Get a basket |
| POST | `/basket` | Create or replace a basket (discounts applied) → `201 Created` |
| DELETE | `/basket/{userName}` | Delete a basket |
| POST | `/basket/checkout` | Check out → publishes `BasketCheckoutEvent` |
| GET | `/health` | PostgreSQL + Redis health |

### Discount.Grpc (`http://localhost:6002`)

gRPC service `discount.DiscountProtoService`: `GetDiscount`, `CreateDiscount`, `UpdateDiscount`, `DeleteDiscount`. Try it with **grpcurl** or **Postman gRPC**:

```bash
grpcurl -plaintext -d '{"productName":"S23"}' localhost:6002 discount.DiscountProtoService/GetDiscount
```

### Ordering.API

| Method | Route | Description |
|---|---|---|
| GET | `/orders?PageIndex=0&PageSize=10` | Paginated orders |
| GET | `/orders/{orderName}` | Orders whose name matches |
| GET | `/orders/customer/{customerId}` | Orders for a customer |
| POST | `/orders` | Create an order |
| PUT | `/orders` | Update an order |
| DELETE | `/orders/{orderId}` | Delete an order |
| GET | `/health` | SQL Server health |

---

## Running Locally

### Prerequisites
- .NET 9 SDK (pinned in `global.json`)
- Docker and Docker Compose

### Infrastructure and services with Docker Compose

```bash
git clone https://github.com/Bardiya-basafa/Microservices-App.git
cd Microservices-App/src
docker compose up --build
```

| Container | Port | Purpose |
|---|---|---|
| `catalogdb` | 5432 | PostgreSQL for Catalog (Marten) |
| `basketdb` | 5433 | PostgreSQL for Basket (Marten) |
| `orderdb` | 1433 | SQL Server for Ordering |
| `distributedcache` | 6379 | Redis |
| `catalog.api` | 6000 | Catalog REST API |
| `basket.api` | 6001 | Basket REST API |
| `discount.grpc` | 6002 | Discount gRPC service |
| `ordering.api` | – | Ordering REST API |

RabbitMQ is not in the Compose file yet. Start it separately for the checkout flow:

```bash
docker run -d --name ecommerce-mq -p 5672:5672 -p 15672:15672 rabbitmq:management
```

### Running a service from the IDE / CLI

Each service reads its connection strings from `appsettings.json` (pointing at `localhost`), so you can start the infrastructure containers and run a service directly:

```bash
dotnet run --project Services/Catalog/Catalog.API
dotnet run --project Services/Basket/Basket.API
dotnet run --project Services/Discount/Discount.Grpc
dotnet run --project Services/Ordering/Ordering.API
```

---

## Roadmap

- [ ] **Ordering**: implement `OrderExtensions.ConvertToOrderDto(s)`, finish the `AddressDto`, `PaymentDto` and `OrderItemDto` records, complete `CreateOrderCommandValidator`, and map `BasketCheckoutEvent` → `CreateOrderCommand` in `BasketCheckoutEventHandler`
- [ ] **Ordering**: call `DispatchDomainEvents` from the interceptor overrides, and register both interceptors on the `DbContext`
- [ ] Add **RabbitMQ** to `compose.yaml` and align configuration keys (`MessagingBroker:*`) and container host names (Redis, Postgres ports) across environments
- [ ] Align the Catalog connection-string name (`Database` vs `CatalogDb`)
- [ ] Global exception handler (`IExceptionHandler`) that maps `ValidationException`, `NotFoundException` and so on to RFC 7807 ProblemDetails
- [ ] **API gateway** (YARP) with rate limiting
- [ ] Web front end (Razor/Blazor or SPA)
- [ ] Unit and integration tests (Testcontainers), and OpenTelemetry tracing
