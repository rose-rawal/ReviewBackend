﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using ReviewApp.Repository;

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

        [HttpPut("{reviewId}")]
        public IActionResult UpdateReview(int reviewId, [FromBody]ReviewDto review)
        {
            if(review==null )
                return BadRequest(ModelState);
            if(!(review.Id==reviewId))
            {
                ModelState.AddModelError("", "Review Id not matched");
                return StatusCode(408, ModelState);
            }
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var reviewMap=_mapper.Map<Review>(review);
            if(!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "error in updating review");
                return StatusCode(409, ModelState);
            }
            return Ok("Successfully updated review");
        }

        [HttpDelete("{reviewId}")]
        public IActionResult DeleteReview(int reviewId)
        {
            if (reviewId < 0)
            {
                ModelState.AddModelError("", "review Wrong");
                return StatusCode(500, ModelState);
            }
            var reviewData = _reviewRepository.GetReview(reviewId);
            if (reviewData == null)
            {
                ModelState.AddModelError("", "Error no review Found");
                return StatusCode(501, ModelState);

            }
            if (!_reviewRepository.DeleteReview(reviewData))
            {
                ModelState.AddModelError("", "Error deleting review");
                return StatusCode(501, ModelState);

            }
            return Ok("delete successful");


        }
    }
}
