using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;


namespace ReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _reviewContext;
        public ReviewRepository(DataContext reviewContext)
        {
            _reviewContext = reviewContext;
        }
        public Review GetReview(int id)
        {
            return _reviewContext.Reviews.Where(r => r.Id == id).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _reviewContext.Reviews.ToList();
        }

        public ICollection<Review> GetReviewsofPokemon(int pokeid)
        {
            return _reviewContext.Reviews.Where(r=>r.Pokemon.Id == pokeid).ToList();
        }

        public bool HasReviews(int id)
        {
            return _reviewContext.Reviews.Any();
        }
    }
}
