using ReviewApp.Models;

namespace ReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int id);
        bool HasReviews(int id);
        ICollection<Review> GetReviewsofPokemon(int pokeid);
    }
}
