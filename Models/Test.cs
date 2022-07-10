using System;
using System.Collections.Generic;

namespace OnlinePlatformBack.Models
{
    public class Test
    {
        public int Id{get;set;}
        public int CourseId{get;set;}
        public string Token{get;set;}
        public string Question { get; set; }
        public virtual List<Variant> Variants{get;set;} = new();

        public int RigthIndex { get; set; }
        public Test(string question, List<Variant> variants, int rIndex)
        {
            Question = question;
            Variants = variants;
            RigthIndex = rIndex;
            Token = Guid.NewGuid().ToString();
        }
        public Test()
        {
            
        }
    }
}