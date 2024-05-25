using ReviewApp.Models;

namespace ReviewApp.Interfaces
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int id);
        ICollection<Review> GetReviewsByReviewer(int id);
        bool HasReviewer(int id);
        bool PostReviewer(int reviewId,Reviewer reviewer);
        bool Save();
        bool UpdateReviewer(Reviewer reviewer);
    }
}
