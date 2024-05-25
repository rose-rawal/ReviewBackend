using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;


namespace ReviewApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OwnerController:Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public OwnerController(IOwnerRepository ownerRepository,ICountryRepository countryRepository,IMapper mapper) { 
            _mapper = mapper;
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
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

        [HttpGet("{pokeid}/getownerofpokemon")]
        [ProducesResponseType(200,Type =typeof(ICollection<Owner>))]
        [ProducesResponseType(400)]

        public IActionResult getOwmerByPokemon(int pokeid)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(_mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwnerOfPokemon(pokeid)));
        }

        [HttpGet("{ownerid}/getpokemonbyOwner")]
        [ProducesResponseType(200, Type =typeof(ICollection<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult getPokemonByOwner(int ownerid)
        {
            if(!_ownerRepository.OwnerExists(ownerid))
                return NotFound();
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(_mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonByOwner(ownerid)));
        }

        [HttpPost]
        public IActionResult postOwner([FromQuery] int countryId,[FromBody]OwnerDto owner)
        {
            if(owner==null)
                return BadRequest(ModelState);
            if (_ownerRepository.GetOwners().Any(o => (o.FirstName + owner.LastName).ToUpper() == (owner.FirstName + owner.LastName).ToUpper()))
            {
                ModelState.AddModelError("", "Error Owner Already Exists");
                return StatusCode(407, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ownerMap=_mapper.Map<Owner>(owner);
            ownerMap.Country=_countryRepository.GetCountry(countryId);
            if(!_ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Owner Creation error generated");
                return StatusCode(443,ModelState);
            }
            return Ok("Owner Creation Successful");
        }

        [HttpPut("{ownerId}")]
        public IActionResult UpdateOwner(int ownerId,[FromBody]OwnerDto owner)
        {
            if (owner == null)
                return BadRequest(ModelState);
            if (owner.Id != ownerId)
                return BadRequest(ModelState);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var ownerMap=_mapper.Map<Owner>(owner);
            if(!_ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Error in updating Owner");
                StatusCode(406, ModelState);
            }
            return Ok("Successfully Updated Owner");
        }
    }
}
