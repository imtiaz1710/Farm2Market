using MediatR;

namespace BuildingBlocks.CQRS;

public interface ICommand: ICommand<Unit> //unit is the void type of mediatr
{

}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
