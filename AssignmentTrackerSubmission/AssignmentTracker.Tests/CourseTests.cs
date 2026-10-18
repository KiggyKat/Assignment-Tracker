using AssignmentTracker;

namespace AssignmentTrackerTests
{
    internal static class CourseTests
    {
        public static void ConstructorSetsDefaults()
        {
            Course course = new Course();
            Assert.Equal(0, course.CourseID);
            Assert.Equal(string.Empty, course.CourseName);
            Assert.Equal(string.Empty, course.InstructorName);
            Assert.Equal(string.Empty, course.Semester);
        }

        public static void PropertiesStoreValues()
        {
            Course course = new Course
            {
                CourseID = 1,
                CourseName = "Programming",
                InstructorName = "Sample Instructor",
                Semester = "Fall 2026"
            };
            Assert.Equal(1, course.CourseID);
            Assert.Equal("Programming", course.CourseName);
            Assert.Equal("Sample Instructor", course.InstructorName);
            Assert.Equal("Fall 2026", course.Semester);
        }
    }
}
