using System;
using AssignmentTracker;

namespace AssignmentTrackerTests
{
    internal static class AssignmentTests
    {
        public static void ConstructorSetsDefaults()
        {
            Assignment assignment = new Assignment();
            Assert.Equal(0, assignment.AssignmentID);
            Assert.Equal(string.Empty, assignment.Title);
            Assert.Equal(string.Empty, assignment.Description);
            Assert.Equal(DateTime.Today, assignment.DueDate);
            Assert.Equal(false, assignment.IsCompleted);
            Assert.Equal(0, assignment.CourseID);
        }

        public static void PropertiesStoreValues()
        {
            Assignment assignment = new Assignment
            {
                AssignmentID = 7,
                Title = "Practice",
                Description = "Practice classes",
                DueDate = new DateTime(2026, 10, 20),
                IsCompleted = true,
                CourseID = 2
            };
            Assert.Equal(7, assignment.AssignmentID);
            Assert.Equal("Practice", assignment.Title);
            Assert.Equal("Practice classes", assignment.Description);
            Assert.Equal(new DateTime(2026, 10, 20), assignment.DueDate);
            Assert.Equal(true, assignment.IsCompleted);
            Assert.Equal(2, assignment.CourseID);
        }

        public static void MarkCompletedSetsStatus()
        {
            Assignment assignment = new Assignment();
            assignment.MarkCompleted();
            Assert.Equal(true, assignment.IsCompleted);
        }

        public static void RepeatedCompletionIsSafe()
        {
            Assignment assignment = new Assignment();
            assignment.MarkCompleted();
            assignment.MarkCompleted();
            Assert.Equal(true, assignment.IsCompleted);
        }

        public static void BeforeDeadlineIsIncluded()
        {
            Assignment assignment = new Assignment { DueDate = new DateTime(2026, 10, 1) };
            Assert.Equal(true, assignment.IsDueBy(new DateTime(2026, 10, 20)));
        }

        public static void SameDayIgnoresTime()
        {
            Assignment assignment = new Assignment
            {
                DueDate = new DateTime(2026, 10, 20, 23, 59, 59)
            };
            Assert.Equal(true, assignment.IsDueBy(new DateTime(2026, 10, 20)));
        }

        public static void AfterDeadlineIsExcluded()
        {
            Assignment assignment = new Assignment { DueDate = new DateTime(2026, 10, 21) };
            Assert.Equal(false, assignment.IsDueBy(new DateTime(2026, 10, 20, 23, 59, 59)));
        }
    }
}
