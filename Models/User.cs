using System;
using System.Collections.Generic;

namespace OnlinePlatformBack.Models
{
    public class User
    {
        public int Id{get;set;}
        public string Name { get; set; }
        public string Password{get;set;}
        public string Token{get;set;}
        public virtual List<Course> Courses{get;set;} = new();
        public User(string name, string password, ICollection<Course> courses = null)
        {
            Name = name;
            Password = password;
            Courses = (List<Course>)courses;
            Token = Guid.NewGuid().ToString();
        }

        public User(string name, string password)
        {
            Name = name;
            Password = password;
            Token = Guid.NewGuid().ToString();
        }

        public void AddCourse(Course course)
        {
            Courses.Add(course);
        }
    }
}