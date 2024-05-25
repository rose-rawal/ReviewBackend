using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using ReviewApp.Repository;

namespace ReviewApp.Controllers
{
    
    [Route("[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IReviewerRepository _reviewer;
        private readonly IReviewRepository _review;

        public ReviewerController(IReviewerRepository reviewer,IReviewRepository review,IMapper mapper)
        {
            _mapper = mapper;
            _reviewer  =reviewer;
            _review = review;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult GetReviewers() {
        if(!ModelState.IsValid)
                return BadRequest(ModelState);
        return Ok(_reviewer.GetReviewers());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]

        public IActionResult GetReviewers(int id)
        {
            if(!_reviewer.HasReviewer(id))
                return NotFound();
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(_reviewer.GetReviewer(id));
        }

        [HttpGet("{id}/reviewsByReviewer")]
        public IActionResult GetReviewsByReviewer(int id)
        {
            if(!_reviewer.HasReviewer(id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(_reviewer.GetReviewsByReviewer(id));
        }


        [HttpPost]
        public IActionResult PostReviewer([FromQuery]int reviewId, [FromBody]ReviewerDto reviewerData)
        {
            if(reviewerData==null || reviewId<0)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if(_reviewer.GetReviewers().Where(r=>r.Id==reviewerData.Id).Any())
            {
                ModelState.AddModelError("", "Error Reviewer already exists");
                return StatusCode(456, ModelState);
            }
            var reviewerMap = _mapper.Map<Reviewer>(reviewerData);
            if(!_reviewer.PostReviewer(reviewId, reviewerMap))
            {
                ModelState.AddModelError("", "Error in creating Reviewer");
                return StatusCode(466, ModelState);
            }
            return Ok("Success in creating reviewer");
        }


        [HttpPut("{reviewerId}")]
        public IActionResult UpdatedReviewer(int reviewerId, [FromBody]ReviewerDto reviewer)
        {
            if(reviewer==null)
                return BadRequest(ModelState);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            if(!_reviewer.HasReviewer(reviewerId))
            {
                ModelState.AddModelError("", "Error couldnt find reviewer");
                return StatusCode(407, ModelState);

            }
            var reviwerMap=_mapper.Map<Reviewer>(reviewer);
            if(!_reviewer.UpdateReviewer(reviwerMap))
            {
                ModelState.AddModelError("", "Error couldnt update the reviewer");
                return StatusCode(406,ModelState);
            }
            return Ok("Success in updating reviewer");
        }


        [HttpDelete("{reviewerId}")]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (reviewerId < 0)
            {
                ModelState.AddModelError("", "reviewer Wrong");
                return StatusCode(500, ModelState);
            }
            var reviewerData = _reviewer.GetReviewer(reviewerId);
            if (reviewerData == null)
            {
                ModelState.AddModelError("", "Error no reviewer Found");
                return StatusCode(501, ModelState);

            }
            var reviewToDelete=_reviewer.GetReviewsByReviewer(reviewerId);
           
            if(reviewToDelete.Any() && !_review.DeleteMultipleReview(reviewToDelete))
            {
                
                ModelState.AddModelError("", "Error deleting review of reviewer");
                return StatusCode(501, ModelState);

            }
            if (!_reviewer.DeleteReviewer(reviewerData))
            {
                ModelState.AddModelError("", "Error deleting reviewer");
                return StatusCode(501, ModelState);

            }
            return Ok("delete successful");


        }

    }
}
