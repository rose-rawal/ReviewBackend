using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;
        public CountryRepository(DataContext context) { 
            _context=context;
        }

        public bool CreateCountry(Country country)
        {
            _context.Countries.Add(country);
            return Save();
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.ToList();
        }

        public Country GetCountry(int id)
        {
            return _context.Countries.Where(c => c.Id == id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).Select(o=>o.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerByCountry(int countryId)
        {
            return _context.Countries.Where(c=>c.Id==countryId).SelectMany(c=>c.Owners).ToList();
        }

        public bool IsCountry(int id)
        {
            return _context.Countries.Any(c=>c.Id==id);
        }

        public bool Save()
        {
            var countrySave = _context.SaveChanges();
            return countrySave > 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _context.Countries.Update(country);
            return Save();
        }

        public bool DeleteCountry(Country country) { 
            _context.Countries.Remove(country);
            return Save();
        }
    }
}
