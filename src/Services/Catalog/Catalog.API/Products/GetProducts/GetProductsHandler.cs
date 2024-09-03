﻿
using Marten.Pagination;

namespace Catalog.API.Products.GetProducts;

public record GetProductQuery(int? PageNo = 1, int? PageSize = 10) : IQuery<GetProductResult>;
public record GetProductResult(IEnumerable<Product> Products);
internal class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductQuery, GetProductResult>
{
    public async Task<GetProductResult> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>().ToPagedListAsync(query.PageNo ?? 1, query.PageSize ?? 10, cancellationToken);

        return new GetProductResult(products);
    }
}
