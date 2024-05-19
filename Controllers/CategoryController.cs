using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class CategoryController :Controller
    {
        private readonly ICategoryRepository _category;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryRepository category,IMapper mapper)
        {
            _category= category;
            _mapper= mapper;
        }
        [HttpGet]
        [ProducesResponseType(200,Type=typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]
        public IActionResult GetCategories()
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_mapper.Map<List<CategoryDto>>(_category.GetCategories()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200,Type=typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int id) {
            if (!_category.CategoryExists(id))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(_mapper.Map<CategoryDto>(_category.GetCategory(id)));
        }


        [HttpGet("{id}/pokemon")]
        [ProducesResponseType(200,Type =typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]

        public IActionResult GetPokemonByCategory(int id) {
            if (!_category.CategoryExists(id))
                return NotFound();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_mapper.Map<List<PokemonDto>>(_category.GetPokemonByCategory(id)));
        }
    }
}
