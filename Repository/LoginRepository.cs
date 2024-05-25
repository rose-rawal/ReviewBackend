using Microsoft.IdentityModel.Tokens;
using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReviewApp.Repository
{
    public class LoginRepository : ILoginRepository
    {
        public readonly DataContext _context;
        private readonly IConfiguration _config;

        public LoginRepository(DataContext context,IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public string GenerateToken(Owner owner)
        {
            var securityKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials=new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            var claims = new []
            {
                new Claim(ClaimTypes.NameIdentifier,owner.FirstName),
                new Claim(ClaimTypes.Surname,owner.LastName),
            };
            var token=new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires:DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Owner Login(string firstName, string lastName)
        {
            var loginData=_context.Owners.FirstOrDefault(o => o.FirstName == firstName && o.LastName == lastName);
            if (loginData == null)
            {
                return null;
            }
            return loginData;

        }
    }
}
