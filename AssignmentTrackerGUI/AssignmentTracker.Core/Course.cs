namespace AssignmentTracker
{
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public string InstructorName { get; set; }
        public string Semester { get; set; }

        public Course()
        {
            CourseName = string.Empty;
            InstructorName = string.Empty;
            Semester = string.Empty;
        }
    }
}
