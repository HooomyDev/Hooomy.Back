using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;

namespace Hooome.Application.Companies.Queries.GetCompanyList;

public class CompanyListLookupDto : IMapWith<Company>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public void Mapping(Profile profile) 
        => profile.CreateMap<Company, CompanyListLookupDto>();
}
