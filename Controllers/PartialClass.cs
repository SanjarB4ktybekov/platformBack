using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using OnlinePlatformBack.Models;
using System.Diagnostics;

namespace OnlinePlatformBack.Controllers
{
    public partial class ApplicationController : ControllerBase
    {
        [Route("[controller]/login")]
        [HttpPost]
        public async Task<IActionResult> Login(UserInfo info)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Password == info.Password && u.Name == info.Name);
            if (user == null)
            {
                return NotFound(info);
            }
            return Ok(user.Token);
        }

        [Route("[controller]/signup")]
        [HttpPost]
        public async Task<IActionResult> Signup(UserInfo info)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Password == info.Password && u.Name == info.Name);
            if (user == null)
            {
                user = new User(info.Name, info.Password);
                await db.Users.AddAsync(user);
                db.SaveChanges();
                return Ok(user.Token);
            }
            else
            {
                return BadRequest("Пользователь уже зарегистрирован");
            }
        }

        
    }
}