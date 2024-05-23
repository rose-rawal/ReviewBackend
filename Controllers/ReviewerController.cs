using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Controllers
{
    
    [Route("[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IReviewerRepository _reviewer;
        public ReviewerController(IReviewerRepository reviewer,IMapper mapper)
        {
            _mapper = mapper;
            _reviewer  =reviewer;
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

    }
}
