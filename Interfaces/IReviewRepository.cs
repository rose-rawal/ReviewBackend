using ReviewApp.Models;

namespace ReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int id);
        bool HasReviews(int id);
        ICollection<Review> GetReviewsofPokemon(int pokeid);
        bool PostReview(int reviewerId,int pokeId,Review review);
        bool Save();
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool DeleteMultipleReview(ICollection<Review> reviews);
    }

}
