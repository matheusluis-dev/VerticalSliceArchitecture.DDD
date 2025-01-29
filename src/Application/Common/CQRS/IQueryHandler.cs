namespace Application.Common.CQRS;

using MediatR;

public interface IQueryHandler<in TRequest> : IRequestHandler<TRequest>
    where TRequest : IRequest;

public interface IQueryHandler<in TRequest, TResult> : IRequestHandler<TRequest, TResult>
    where TRequest : IRequest<TResult>;
