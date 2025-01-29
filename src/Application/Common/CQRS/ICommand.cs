namespace Application.Common.CQRS;

using MediatR;

public interface ICommand : IRequest;

public interface ICommand<out T> : IRequest<T>;
