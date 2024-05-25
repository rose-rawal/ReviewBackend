using ReviewApp.Models;

namespace ReviewApp.Interfaces
{
    public interface ILoginRepository
    {
        Owner Login(string firstName, string lastName);
        string GenerateToken(Owner owner);
    }

}
