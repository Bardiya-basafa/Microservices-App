﻿namespace BuildingBlocks.CQRS;



public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
    where TResult : notnull {

}
