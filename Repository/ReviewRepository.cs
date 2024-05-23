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

        

        public bool PostReview(int reviewerId, int pokeId, Review review)
        {
            var reviewerData = _reviewContext.ReviewersOwner.Where(r => r.Id == reviewerId).FirstOrDefault();
            var pokemonData=_reviewContext.Pokemons.Where(p=>p.Id==pokeId).FirstOrDefault();
            if(reviewerData==null || pokemonData==null)
                return false;
            var reviewData = new Review()
            {
                Pokemon = pokemonData,
                Reviewer = reviewerData,
                Id = review.Id,
                Text=review.Text,
                Title=review.Title,
                Rating = review.Rating,
            };

            _reviewContext.Reviews.Add(reviewData);
            return Save();
        }

        public bool Save()
        {
            var saveChange=_reviewContext.SaveChanges();
            return saveChange>0 ? true : false;
        }
    }
}
