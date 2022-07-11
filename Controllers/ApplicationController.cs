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
    public class UserResult
    {
        public User User;
        public List<string> Result { get; set; }
        public UserResult(User user, List<string> res)
        {
            this.User = user;
            Result = res;
        }
        public UserResult()
        {

        }
    }
    public class Points
    {
        public int Result { get; set; }
    }
    public class CourseInfo
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
    public class UserInfo
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class TestInfo
    {
        public string Question { get; set; }
        public int RigthIndex { get; set; }
        public virtual List<Variant> Variants { get; set; } = new();
    }

    [ApiController]
    [Produces("application/json")]
    public partial class ApplicationController : ControllerBase
    {
        private ApplicationContext db;
        public ApplicationController(ApplicationContext context)
        {
            db = context;
            if (db.Users.Count() == 0)
            {
                db.Users.Add(new User("Санжар", "12345", courses1));
                db.Users.Add(new User("Бактыбеков", "54321"));
                db.SaveChanges();
            }
        }
        #region Начальные данные
        public static List<Variant> variants_1 = new List<Variant> { new Variant("Красный"), new Variant("Синий"), new Variant("Желтый"), new Variant("Зеленый") };

        public static List<Variant> variants_2 = new List<Variant> { new Variant("1"), new Variant("2"), new Variant("3"), new Variant("4") };

        public static List<Variant> variants = new List<Variant> { new Variant("1"), new Variant("2"), new Variant("3"), new Variant("4") };
        public static Test test = new Test("2 + 2", variants, 3);
        public static Test test_1 = new Test("Выберите лишнее", variants_1, 2);
        public static Test test_2 = new Test("1 + 2", variants_2, 2);

        public static List<Test> tests = new List<Test> { test, test_1, test_2 };
        public static List<Course> courses1 = new()
        {
            new Course("Курс от Санжара", "Знак '+' - означает сложение. 1 + 1 = 2",tests)
        };

        #endregion
        [Route("[controller]/getMyCourses")]
        [HttpGet]
        public async Task<IActionResult> GetMyCourses()
        {
            string token = Request.Headers["userToken"];
            if (token == null)
            {
                return BadRequest("Как ты запрос без токена отправил?");
            }
            var user = await db.Users.FirstOrDefaultAsync(u => u.Token == token);
            if (user == null)
            {
                return BadRequest("Пользователь не найден");
            }
            return Ok(user.Courses);
        }

        [Route("[controller]/removeMyCourse")]
        [HttpGet]
        public async Task<IActionResult> RemoveMyCourse()
        {
            string token = Request.Headers["courseToken"];
            var course = await db.Courses.FirstOrDefaultAsync(c => c.Token == token);
            db.Courses.Remove(course);
            db.SaveChanges();
            return Ok("Курс удален!");
        }

        [Route("[controller]/saveResToDb")]
        [HttpPost]
        public async Task<IActionResult> SaveResToDb(Points points)
        {
            string token = Request.Headers["courseToken"];
            string userToken = Request.Headers["userToken"];
            var course = await db.Courses.FirstOrDefaultAsync(c => c.Token == token);
            if (course == null)
            {
                return NotFound("Njt fount 1");
            }
            var user = await db.Users.FirstOrDefaultAsync(c => c.Token == userToken);
            if (user == null)
            {
                return BadRequest("Njt fount 2");
            }
            ResultToDb res = new ResultToDb(points.Result, course.Token.ToString());
            user.AddResult(res);
            db.Users.Update(user);
            db.SaveChanges();
            return Ok("Результат добавлен!");
        }

        [Route("[controller]/createEmptyCourse")]
        [HttpPost]
        public async Task<IActionResult> CreateEmptyCourse(CourseInfo courseInfo)
        {
            string token = Request.Headers["userToken"];
            if (token == null)
            {
                return BadRequest("Как ты запрос без токена отправил?");
            }
            var user = await db.Users.FirstOrDefaultAsync(u => u.Token == token);
            if (user == null)
            {
                return BadRequest("Пользователя нет");
            }

            Course course = new Course(courseInfo.Title, courseInfo.Content);
            user.AddCourse(course);
            db.SaveChanges();
            return Ok(course.Token);
        }

        [Route("[controller]/addTestToCourse")]
        [HttpPost]
        public async Task<IActionResult> AddTestToCourse([FromBody] TestInfo testInfo)
        {
            string token = Request.Headers["courseToken"];
            if (token == null)
            {
                return BadRequest("Нет токена");
            }
            var course = await db.Courses.FirstOrDefaultAsync(u => u.Token == token);
            if (course == null)
            {
                return BadRequest("Нет курса");
            }
            Test test = new Test(testInfo.Question, testInfo.Variants, testInfo.RigthIndex);
            course.addTest(test);
            db.SaveChanges();
            return Ok("Тест добавлен!");
        }

        [Route("[controller]/getUserResult")]
        [HttpGet]
        public async Task<IActionResult> GetUserResult()
        {
            string token = Request.Headers["userToken"];
            var user = await db.Users.FirstOrDefaultAsync(u => u.Token == token);
            var res = user.Results;
            List<string> list = new();
            foreach (var item in res)
            {
                var c = await db.Courses.FirstOrDefaultAsync(c => c.Token == item.CourseToken);
                string title = c.Title;
                //User u = FindAuthor(c.Token);
                list.Add($"Пройден курс '{title}'");
            }

            UserResult userResult = new UserResult(user, list);
            return Ok(userResult);
        }
        

        [Route("[controller]/getUser")]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            string token = Request.Headers["userToken"];
            var user = await db.Users.FirstOrDefaultAsync(u => u.Token == token);
            return Ok(user);
        }

        

        [Route("[controller]")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await db.Users.ToListAsync();
            return Ok(users);
        }

        [Route("[controller]/getGlobalCourses")]
        [HttpGet]
        public async Task<IActionResult> GetGlobalCourses()
        {
            string token = Request.Headers["userToken"];
            if (token == null)
            {
                return BadRequest("Токена нет дружочек");
            }
            var user = await db.Users.FirstOrDefaultAsync(u => u.Token == token);
            var globalCourses = await db.Courses.ToArrayAsync();
            return Ok(globalCourses);
        }

        [Route("[controller]/getGlobalCourse")]
        [HttpGet]
        public async Task<IActionResult> GetGlobalCourse()
        {
            string token = Request.Headers["courseToken"];
            if (token == null)
            {
                return BadRequest("Токена нет дружочек");
            }
            var course = await db.Courses.FirstOrDefaultAsync(c => c.Token == token);
            if (course == null)
            {
                return BadRequest("Такого курса нет,дружок");
            }
            return Ok(course);
        }

        [Route("[controller]/getTestsList")]
        [HttpGet]
        public async Task<IActionResult> GetTestsList()
        {
            string token = Request.Headers["courseToken"];
            if (token == null)
            {
                return BadRequest("Токена нет дружочек");
            }
            var course = await db.Courses.FirstOrDefaultAsync(c => c.Token == token);
            if (course == null)
            {
                return BadRequest("Такого курса нет,дружок");
            }
            var index = course.Id;
            if (index == 0)
            {
                return BadRequest("Нет курс айди");
            }
            var tests = db.Tests.Where(t => t.CourseId == index);
            return Ok(tests);
        }
    };
}