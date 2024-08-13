﻿namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Name is Required");
        RuleFor(p => p.Category).NotEmpty().WithMessage("Category is Required");
        RuleFor(p => p.Description).NotEmpty().WithMessage("Description is Required");
        RuleFor(p => p.ImageFile).NotEmpty().WithMessage("ImageFile is Required");
        RuleFor(p => p.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}
internal class CreateProductHandler(IDocumentSession session, IValidator<CreateProductCommand> validator)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        //var result = await validator.ValidateAsync(command);
        //var errors = result.Errors.Select(e => e.ErrorMessage).ToList();

        //if (errors.Any())
        //{
        //    throw new ValidationException(errors.FirstOrDefault());
        //}

        var product = new Product
        {
            Price = command.Price,
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile
        };

        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }
}
