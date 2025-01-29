namespace Application.Common.CQRS;

using MediatR;

public interface ICommandHandler<in TRequest> : IRequestHandler<TRequest>
    where TRequest : IRequest;

public interface ICommandHandler<in TRequest, TResult> : IRequestHandler<TRequest, TResult>
    where TRequest : IRequest<TResult>;
