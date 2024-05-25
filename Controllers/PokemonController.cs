
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using System.Collections.Generic;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        public PokemonController(IPokemonRepository pokemonRepository,IReviewRepository reviewRepository, IMapper mapper)
        {
            this._pokemonRepository = pokemonRepository;
            _reviewRepository = reviewRepository;
            this._mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound();
            }
            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemon);
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if(!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();
            var rating=_pokemonRepository.GetPokemonRating(pokeId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(rating);

        }

        [HttpPost]
        public IActionResult createPokemon([FromQuery]int ownerId, [FromQuery]int categoryId, [FromBody]PokemonDto pokemon)
        {
            if( pokemon==null)
                return BadRequest(ModelState);
            var pokemonData = _pokemonRepository.GetPokemons().Any(p=>p.Name==pokemon.Name);
            if (pokemonData)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(405, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var pokemonMap = _mapper.Map<Pokemon>(pokemon);
            if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Error in generating Pokemon");
                return StatusCode(407,ModelState);
            }
            return Ok("Success in creating Pokemon");
        }

        [HttpPut("{pokemonId}")]
        public IActionResult UpdatePokemon(int pokemonId, [FromQuery]int catId, [FromQuery]int ownerId,PokemonDto pokemon)
        {
            if(pokemon==null)
                return BadRequest(ModelState) ;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if(!_pokemonRepository.PokemonExists(pokemon.Id))
            {
                ModelState.AddModelError("", "Pokemon Doesnot Exist");
                return StatusCode(407, ModelState);
            }
            var pokemonMapper = _mapper.Map<Pokemon>(pokemon);
            if (!_pokemonRepository.UpdatePokemon(pokemonMapper))
            {
                ModelState.AddModelError("", "Error in Updating Pokemon");
                return StatusCode(408, ModelState);
            }

            return Ok("Successfully updated pokemon");
        }


        [HttpDelete("{pokeId}")]
        public IActionResult DeleteCountry(int pokeId)
        {
            if (pokeId < 0)
            {
                ModelState.AddModelError("", "pokemon Wrong");
                return StatusCode(500, ModelState);
            }
            var pokemonData = _pokemonRepository.GetPokemon(pokeId);
            
            if (pokemonData == null)
            {
                ModelState.AddModelError("", "Error no pokemon Found");
                return StatusCode(501, ModelState);

            }
            var reviewData=_reviewRepository.GetReviewsofPokemon(pokeId);
            if(!_reviewRepository.DeleteMultipleReview(reviewData))
            {
                ModelState.AddModelError("", "Error in deleting reviews associated with pokemon");
                return StatusCode(507,ModelState);  
            }
            if (!_pokemonRepository.DeletePokemon(pokemonData))
            {
                ModelState.AddModelError("", "Error deleting pokemon");
                return StatusCode(501, ModelState);

            }
            return Ok("delete successful");


        }
    }
}
