using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _context;
        public ReviewerRepository(DataContext context)
        {
            _context = context;
        }
        public Reviewer GetReviewer(int id)
        {
            return _context.ReviewersOwner.Where(r => r.Id == id).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _context.ReviewersOwner.ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int id)
        {
            return _context.Reviews.Where(r=>r.Reviewer.Id==id).ToList();
        }

        public bool HasReviewer(int id)
        {
            return _context.ReviewersOwner.Any();
        }

        public bool PostReviewer(int reviewId, Reviewer reviewer)
        {
            var review=_context.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
            if(review==null) { 
                return false;
            }
            var reviewerData = new Reviewer()
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                LastName = reviewer.LastName,
                Reviews=new List<Review>()
            };
            _context.Add(reviewerData);
            reviewerData.Reviews.Add(review);

            return Save();
        }

        public bool Save()
        {
            var saveChanges=_context.SaveChanges();
            return saveChanges>0 ? true : false;
        }

        public bool UpdateReviewer( Reviewer reviewer)
        {
            var reviewerNew = _context.ReviewersOwner.Update(reviewer);
            return Save();
        }
    }
}
