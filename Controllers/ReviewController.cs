using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        public ReviewController(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        [HttpGet("{pokeid}/getreviewByPookemon")]
        [ProducesResponseType(200, Type = typeof(List<Review>))]
        public IActionResult getReviewByPokemon(int pokeid)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(_mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsofPokemon(pokeid)));
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Review>))]
        public IActionResult GetResults()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(_mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        public IActionResult GetResult(int id)
        {
            if (!_reviewRepository.HasReviews(id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(_mapper.Map<ReviewDto>(_reviewRepository.GetReview(id)));
        }


        [HttpPost]
        public IActionResult PostReview([FromQuery] int pokeId, [FromQuery] int ownerId, [FromBody] ReviewDto reviewData)
        {
            if (pokeId < 0 || ownerId < 0||reviewData==null)
                return BadRequest(ModelState);
            if(_reviewRepository.GetReviews().Any(r=>r.Title ==reviewData.Title))
            {
                ModelState.AddModelError("", "Review Already Exists");
                return StatusCode(422, ModelState);
            }
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewMap = _mapper.Map<Review>(reviewData);
            if(!_reviewRepository.PostReview(pokeId,ownerId,reviewMap))
            {
                ModelState.AddModelError("", "Error during creating review");
                return StatusCode(423,ModelState);
            }
            return Ok("Review Posting Successfull");
        }

        
    }
}
