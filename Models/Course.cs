using System;
using System.Collections.Generic;

namespace OnlinePlatformBack.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public virtual List<Test> Tests { get; set; } = new();
        public virtual List<TestToken> TestsTokens { get; set; } = new();
        public Course(string title, string content)
        {
            Title = title;
            Content = content;
            Token = Guid.NewGuid().ToString();
        }

        public Course(string title, string content, ICollection<Test> tests)
        {
            Title = title;
            Content = content;
            Tests = (List<Test>)tests;
            Token = Guid.NewGuid().ToString();

             foreach (var item in tests)
            {
                var token = new TestToken(item.Token.ToString());
                TestsTokens.Add(token);
            }
        }

        public void addTest(Test test)
        {
            Tests.Add(test);
            var token = new TestToken(test.Token.ToString());
            TestsTokens.Add(token);
        }

        public void addTestsList(List<Test> _tests)
        {
            Tests.AddRange(_tests);
            foreach (var item in _tests)
            {
                var token = new TestToken(item.Token.ToString());
                TestsTokens.Add(token);
            }
        }
    }
}