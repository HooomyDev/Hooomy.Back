using Hooome.Domain;

namespace Hooome.Application.Interfaces;

public interface ICompanyImageRepository : IRepository<CompanyImage>
{
    Task<CompanyImage?> GetLogoByCompanyId(Guid companyId, CancellationToken cancellationToken = default);
}
