using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using OnlinePlatformBack.Models;


namespace OnlinePlatformBack.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class LoginController : ControllerBase
    {
        private ApplicationContext db;
        public LoginController(ApplicationContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await db.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserInfo info)
        {
            if (info == null)
            {
                return BadRequest("Не пришли данные");
            }
            var user = await db.Users.FirstOrDefaultAsync(u => u.Password == info.Password && u.Name == info.Name);
            if (user == null)
            {
                return NotFound(info);
            }
           return Ok(user.Token);
        }
    }
}
