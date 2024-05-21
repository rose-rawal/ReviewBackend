using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dto;
using ReviewApp.Interfaces;

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

    }
}
