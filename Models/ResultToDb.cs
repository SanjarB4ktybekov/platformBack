namespace OnlinePlatformBack.Models
{
    public class ResultToDb
    {
        public int id { get; set; }
        public int Result{get;set;}
        public string CourseToken{get;set;}
        public ResultToDb()
        {

        }

        public ResultToDb(int res, string courseID)
        {
            Result = res;
            CourseToken = courseID;
        }

    }
}