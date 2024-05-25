using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Interfaces;

namespace ReviewApp.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class LoginController:Controller
    {
        private readonly ILoginRepository _login;
        public LoginController(ILoginRepository login)
        {
            _login = login;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(string firstName,string lastName)
        {
            if(firstName == null || lastName == null)
            {
                return BadRequest(ModelState);
            }
            var user=_login.Login(firstName, lastName);
            if (user == null)
                return NotFound();
            var token=_login.GenerateToken(user);
            return Ok(token);
        }
    }
}
