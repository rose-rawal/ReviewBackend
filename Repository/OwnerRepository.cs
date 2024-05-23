using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _dataContext;
        public OwnerRepository(DataContext dataContext) { 
            _dataContext = dataContext;
        }

        public bool CreateOwner(Owner owner)
        {
            _dataContext.Owners.Add(owner);
            return Save();
        }

        public Owner GetOwner(int ownerId)
        {
            return _dataContext.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfPokemon(int pokemonId)
        {
            return _dataContext.PokemonsOwners.Where(p=>p.PokemonId== pokemonId).Select(p=>p.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return _dataContext.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _dataContext.PokemonsOwners.Where(o=>o.OwnerId==ownerId).Select(o=>o.Pokemon).ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return _dataContext.Owners.Any(o=>o.Id==ownerId);
        }

        public bool Save()
        {
            var ownerSave = _dataContext.SaveChanges();
            return ownerSave> 0 ? true : false;
        }
    }
}
