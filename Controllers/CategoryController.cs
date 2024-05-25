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


        [HttpPost]
        public IActionResult postCategory([FromBody] CategoryDto categoryCreate)
        {
            if(categoryCreate == null)
                return BadRequest(ModelState);
            var category=_category.GetCategories().Where(c=>c.Name.Trim().ToUpper()==categoryCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();
            if(category != null)
            {
                ModelState.AddModelError("", "Error category already exists");
                return StatusCode(422,ModelState);
            }
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var categoryMap = _mapper.Map<Category>(categoryCreate);
            if (!_category.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Category Creating Failed");
                return StatusCode(500, ModelState);
            }
            return Ok("Success in creating Category");
        }


        [HttpPut("{categoryId}")]
        public IActionResult UpdateCategory(int categoryId,[FromBody] CategoryDto category)
        {
            if(category==null)
                return BadRequest(ModelState);
            if(categoryId!=category.Id)
                return BadRequest(ModelState);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var categoryMap=_mapper.Map<Category>(category);
            if(!_category.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Error in updating Category");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated Data");
        }

    }
}
