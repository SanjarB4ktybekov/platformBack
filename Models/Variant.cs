namespace OnlinePlatformBack.Models
{
    public class Variant
    {
        public int id { get; set; }
        public string Answer{get;set;}
        public Variant(string answer)
        {
            Answer = answer;
        }
        public Variant ()
        {
            Answer = "";
        }
    }

    public class TestToken
    {
        public int id { get; set; }
        public string token;
        public TestToken(string answer)
        {
            token = answer;
        }
        public TestToken ()
        {
            token = "";
        }
    }
}