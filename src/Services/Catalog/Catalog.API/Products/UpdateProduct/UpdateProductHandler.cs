﻿
namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsUpdated);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Product Id is required");

        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("Product Name is required")
            .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");
    }
}
internal class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation($"UpdateProductHandler.Handle called with {command}");

        var product = await session.LoadAsync<Product>(command.Id);

        if (product is null)
        {
            throw new ProductNotFoundException(command.Id);
        }

        product = command.Adapt<Product>();

        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }
}
