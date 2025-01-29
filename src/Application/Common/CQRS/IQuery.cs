namespace Application.Common.CQRS;

using MediatR;

public interface IQuery : IRequest;

public interface IQuery<out T> : IRequest<T>;
