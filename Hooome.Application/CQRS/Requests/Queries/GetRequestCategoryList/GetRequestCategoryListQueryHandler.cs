using Hooome.Domain.Enums;
using MediatR;
using System.ComponentModel;
using System.Reflection;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestCategoryList;

public class GetRequestCategoryListQueryHandler
    : IRequestHandler<GetRequestCategoryListQuery, RequestCategoryListVm>
{
    public Task<RequestCategoryListVm> Handle(GetRequestCategoryListQuery request, CancellationToken cancellationToken)
    {
        var categories = Enum.GetValues<RequestCategory>()
            .Select(category => new RequestCategoryListLookupDto
            {
                Code = (int)category,
                Name = category
                    .GetType()
                    .GetField(category.ToString())
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description
                ?? category.ToString()
            })
            .OrderByDescending(category => category.Code)
            .ToList();

        return Task.FromResult(new RequestCategoryListVm
        {
            Categories = categories
        });
    }
}
