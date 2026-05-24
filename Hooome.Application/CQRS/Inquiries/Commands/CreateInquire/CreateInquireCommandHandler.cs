using AutoMapper;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Inquiries.Commands.CreateInquire;

public class CreateInquireCommandHandler(IInquiryRepository inquiryRepo, IMapper mapper)
    : IRequestHandler<CreateInquiryCommand, Guid>
{
    public async Task<Guid> Handle(CreateInquiryCommand request, CancellationToken cancellationToken)
    {
        var inquiry = mapper.Map<Inquiry>(request);
        inquiry.Id = Guid.NewGuid();
        inquiry.CreatedAt = DateTime.UtcNow;

        await inquiryRepo.Create(inquiry, cancellationToken);

        return inquiry.Id;
    }
}