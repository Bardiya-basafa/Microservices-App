// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: discount.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Discount.Grpc {
  /// <summary>
  /// discount service definition
  /// </summary>
  public static partial class DiscountProtoService
  {
    static readonly string __ServiceName = "discount.DiscountProtoService";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Discount.Grpc.GetDiscountRequest> __Marshaller_discount_GetDiscountRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Discount.Grpc.GetDiscountRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Discount.Grpc.CouponModel> __Marshaller_discount_CouponModel = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Discount.Grpc.CouponModel.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Discount.Grpc.CreateDiscountRequest> __Marshaller_discount_CreateDiscountRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Discount.Grpc.CreateDiscountRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Discount.Grpc.UpdateDiscountRequest> __Marshaller_discount_UpdateDiscountRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Discount.Grpc.UpdateDiscountRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Discount.Grpc.DeleteDiscountRequest> __Marshaller_discount_DeleteDiscountRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Discount.Grpc.DeleteDiscountRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Discount.Grpc.DeleteDiscountResponse> __Marshaller_discount_DeleteDiscountResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Discount.Grpc.DeleteDiscountResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Discount.Grpc.GetDiscountRequest, global::Discount.Grpc.CouponModel> __Method_GetDiscount = new grpc::Method<global::Discount.Grpc.GetDiscountRequest, global::Discount.Grpc.CouponModel>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetDiscount",
        __Marshaller_discount_GetDiscountRequest,
        __Marshaller_discount_CouponModel);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Discount.Grpc.CreateDiscountRequest, global::Discount.Grpc.CouponModel> __Method_CreateDiscount = new grpc::Method<global::Discount.Grpc.CreateDiscountRequest, global::Discount.Grpc.CouponModel>(
        grpc::MethodType.Unary,
        __ServiceName,
        "CreateDiscount",
        __Marshaller_discount_CreateDiscountRequest,
        __Marshaller_discount_CouponModel);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Discount.Grpc.UpdateDiscountRequest, global::Discount.Grpc.CouponModel> __Method_UpdateDiscount = new grpc::Method<global::Discount.Grpc.UpdateDiscountRequest, global::Discount.Grpc.CouponModel>(
        grpc::MethodType.Unary,
        __ServiceName,
        "UpdateDiscount",
        __Marshaller_discount_UpdateDiscountRequest,
        __Marshaller_discount_CouponModel);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Discount.Grpc.DeleteDiscountRequest, global::Discount.Grpc.DeleteDiscountResponse> __Method_DeleteDiscount = new grpc::Method<global::Discount.Grpc.DeleteDiscountRequest, global::Discount.Grpc.DeleteDiscountResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "DeleteDiscount",
        __Marshaller_discount_DeleteDiscountRequest,
        __Marshaller_discount_DeleteDiscountResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Discount.Grpc.DiscountReflection.Descriptor.Services[0]; }
    }

    /// <summary>Client for DiscountProtoService</summary>
    public partial class DiscountProtoServiceClient : grpc::ClientBase<DiscountProtoServiceClient>
    {
      /// <summary>Creates a new client for DiscountProtoService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public DiscountProtoServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for DiscountProtoService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public DiscountProtoServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected DiscountProtoServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected DiscountProtoServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Discount.Grpc.CouponModel GetDiscount(global::Discount.Grpc.GetDiscountRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetDiscount(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Discount.Grpc.CouponModel GetDiscount(global::Discount.Grpc.GetDiscountRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetDiscount, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Discount.Grpc.CouponModel> GetDiscountAsync(global::Discount.Grpc.GetDiscountRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetDiscountAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Discount.Grpc.CouponModel> GetDiscountAsync(global::Discount.Grpc.GetDiscountRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetDiscount, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Discount.Grpc.CouponModel CreateDiscount(global::Discount.Grpc.CreateDiscountRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CreateDiscount(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Discount.Grpc.CouponModel CreateDiscount(global::Discount.Grpc.CreateDiscountRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_CreateDiscount, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Discount.Grpc.CouponModel> CreateDiscountAsync(global::Discount.Grpc.CreateDiscountRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CreateDiscountAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Discount.Grpc.CouponModel> CreateDiscountAsync(global::Discount.Grpc.CreateDiscountRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_CreateDiscount, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Discount.Grpc.CouponModel UpdateDiscount(global::Discount.Grpc.UpdateDiscountRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return UpdateDiscount(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Discount.Grpc.CouponModel UpdateDiscount(global::Discount.Grpc.UpdateDiscountRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_UpdateDiscount, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Discount.Grpc.CouponModel> UpdateDiscountAsync(global::Discount.Grpc.UpdateDiscountRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return UpdateDiscountAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Discount.Grpc.CouponModel> UpdateDiscountAsync(global::Discount.Grpc.UpdateDiscountRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_UpdateDiscount, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Discount.Grpc.DeleteDiscountResponse DeleteDiscount(global::Discount.Grpc.DeleteDiscountRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return DeleteDiscount(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Discount.Grpc.DeleteDiscountResponse DeleteDiscount(global::Discount.Grpc.DeleteDiscountRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_DeleteDiscount, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Discount.Grpc.DeleteDiscountResponse> DeleteDiscountAsync(global::Discount.Grpc.DeleteDiscountRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return DeleteDiscountAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Discount.Grpc.DeleteDiscountResponse> DeleteDiscountAsync(global::Discount.Grpc.DeleteDiscountRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_DeleteDiscount, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override DiscountProtoServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new DiscountProtoServiceClient(configuration);
      }
    }

  }
}
#endregion
