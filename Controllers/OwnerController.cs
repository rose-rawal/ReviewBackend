using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Interfaces;
using ReviewApp.Models;


namespace ReviewApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OwnerController:Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;
        public OwnerController(IOwnerRepository ownerRepository,IMapper mapper) { 
            _mapper = mapper;
            _ownerRepository = ownerRepository;
        }


        [HttpGet]
        [ProducesResponseType(200,Type =typeof(ICollection<Owner>))]
        public IActionResult GetOwners() {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }
            return Ok(_mapper.Map<List<Owner>>(_ownerRepository.GetOwners()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200,Type =typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int id) {
            if (!_ownerRepository.OwnerExists(id))
                return NotFound();
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_mapper.Map<Owner>(_ownerRepository.GetOwner(id)));
        }

        []
    }
}
