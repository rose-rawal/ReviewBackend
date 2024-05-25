using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class CountryController:Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public CountryController(ICountryRepository countryRepository,IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type=typeof(ICollection<Country>))]
        public IActionResult GetCountries()
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_mapper.Map<List<CountryDto>>(_countryRepository.GetCountries()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200,Type = typeof(Country))]
        public IActionResult GetCountry(int id)
        {
            if (!_countryRepository.IsCountry(id))
                return NotFound();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_mapper.Map<CountryDto>(_countryRepository.GetCountry(id)));
        }

        [HttpGet("{ownerId}/countryByOwner")]
        [ProducesResponseType(200,Type =typeof(Country))]
        public IActionResult GetCountryByOwner(int ownerId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(_mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId)));
        }

        [HttpGet("{countryId}/OwnerByCountry")]
        [ProducesResponseType(200,Type =typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnerByCountry(int countryId)
        {
            if (!_countryRepository.IsCountry(countryId))
                return NotFound();
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_mapper.Map<List<OwnerDto>>(_countryRepository.GetOwnerByCountry(countryId)));
        }

        [HttpPost]

        public IActionResult PostCountry(CountryDto country) {
            if(country == null)
                return BadRequest(ModelState);
            if (_countryRepository.GetCountries().Where(c => c.Name.ToUpper() == country.Name.ToUpper()).Any())
            {
                ModelState.AddModelError("", "Already the country Exists");
                return StatusCode(442,ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var countryMap = _mapper.Map<Country>(country);
            if (!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Create a country failed");
                return StatusCode(500, ModelState);
            }
            return Ok("Success in creating Country");

        }


        [HttpPut("{countryId}")]
        public IActionResult UpdateCountry([FromBody]CountryDto country)
        {
            if(country==null)
                return BadRequest(ModelState);
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);
            var countryMap=_mapper.Map<Country>(country);
            if(_countryRepository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Error in updating country");
                StatusCode(405, ModelState);
            }
            return Ok("Successfully updated country");
        }
    }
}
