using Hooome.Application.Interfaces;
using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Hooome.Persistance;

public sealed class HooomeDbContext(DbContextOptions<HooomeDbContext> options) 
    : DbContext(options), IHooomeDbContext
{
    public DbSet<Request> Requests { get; set; }
    public DbSet<FavoriteAddress> FavoriteAddresses { get; set; }
    public DbSet<Work> Works { get; set; }
    public DbSet<Complaint> Complaints { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<RequestComment> RequestComments { get; set; }
    public DbSet<Poll> Polls { get; set; }
    public DbSet<PollOption> PollOptions { get; set; }
    public DbSet<PollVote> PollVotes { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<CompanyImage> CompanyImages { get; set; }
    public DbSet<RequestImage> RequestImages { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<RequestCommentImage> RequestCommentsImages { get; set; }
    public DbSet<RequestReview> RequestReviews { get; set; }
    public DbSet<Inquiry> Inquiries { get; set; }
    public DbSet<SystemNotification> SystemNotifications { get; set; }
    public DbSet<WorkNotification> WorkNotifications { get; set; }
    public DbSet<RequestNotification> RequestNotifications { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
