using Hooome.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.Interfaces;

public interface IHooomeDbContext
{
    DbSet<Request> Requests { get; set; }
    DbSet<FavoriteAddress> FavoriteAddresses { get; set; }
    DbSet<Work> Works { get; set; }
    DbSet<Complaint> Complaints { get; set; }
    DbSet<Company> Companies { get; set; }
    DbSet<RequestComment> RequestComments { get; set; }
    DbSet<Poll> Polls { get; set; }
    DbSet<PollOption> PollOptions { get; set; }
    DbSet<PollVote> PollVotes { get; set; }
    DbSet<Chat> Chats { get; set; }
    DbSet<Message> Messages { get; set; }
    DbSet<CompanyImage> CompanyImages { get; set; }
    DbSet<RequestImage> RequestImages { get; set; }
    DbSet<Address> Addresses { get; set; }
    DbSet<RequestCommentImage> RequestCommentsImages { get; set; }
    DbSet<RequestReview> RequestReviews { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
