using Hooome.Application.Interfaces;
using Hooome.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hooome.Persistance;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<HooomeDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(nameof(HooomeDbContext)));
        });

        services.AddScoped<IHooomeDbContext>(provider =>
            provider.GetService<HooomeDbContext>()
                ?? throw new NullReferenceException("Provider cant't be null"));

        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IRequestRepository, RequestRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IComplaintRepository, ComplaintRepository>();
        services.AddScoped<IRequestImageRepository, RequestImageRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICompanyImageRepository, CompanyImageRepository>();
        services.AddScoped<IWorkRepository, WorkRepository>();
        services.AddScoped<IPollRepository, PollRepository>();
        services.AddScoped<IPollOptionRepository, PollOptionRepository>();
        services.AddScoped<IPollVoteRepository, PollVoteRepository>();
        services.AddScoped<IRequestCommentImageRepository, RequestCommentImageRepository>();
        services.AddScoped<IRequestCommentRepository, RequestCommentRepository>();
        services.AddScoped<IRequestReviewRepository, RequestReviewRepository>();
        services.AddScoped<IInquiryRepository, InquiryRepository>();
        services.AddScoped<ISystemNotificationRepository, SystemNotificationRepository>();
        services.AddScoped<IRequestNotificationRepository, RequestNotificationRepository>();
        services.AddScoped<IWorkNotificationRepository, WorkNotificationRepository>();

        return services;
    }
}