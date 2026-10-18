using System;
using System.Collections.Generic;
using AssignmentTracker;

namespace AssignmentTrackerDemo
{
    internal class Program
    {
        private static void Main()
        {
            Course programming = new Course
            {
                CourseID = 1,
                CourseName = "C# Programming",
                InstructorName = "Sample Instructor",
                Semester = "Fall 2026"
            };

            AssignmentManager manager =
                new AssignmentManager(new List<Course> { programming });
            manager.SelectedDeadline = new DateTime(2026, 10, 20);

            manager.AddAssignment(new Assignment
            {
                AssignmentID = 1,
                Title = "Class Implementation",
                Description = "Implement the three business classes.",
                CourseID = 1,
                DueDate = new DateTime(2026, 10, 18),
                IsCompleted = true
            });
            manager.AddAssignment(new Assignment
            {
                AssignmentID = 2,
                Title = "Test Methods",
                CourseID = 1,
                DueDate = new DateTime(2026, 10, 20, 23, 59, 0)
            });
            manager.AddAssignment(new Assignment
            {
                AssignmentID = 3,
                Title = "Later Assignment",
                CourseID = 1,
                DueDate = new DateTime(2026, 10, 25)
            });

            Console.WriteLine("Assignment Tracker Business Logic Demonstration");
            Console.WriteLine();
            foreach (Assignment assignment in manager.Assignments)
            {
                string status = assignment.IsCompleted ? "Completed" : "Remaining";
                Console.WriteLine(
                    "{0}: {1} | Due {2:yyyy-MM-dd} | {3}",
                    assignment.AssignmentID, assignment.Title, assignment.DueDate, status);
            }

            Console.WriteLine();
            Console.WriteLine("Due by {0:yyyy-MM-dd}", manager.SelectedDeadline);
            manager.CalculateTotals();
            DisplayTotals(manager);

            Console.WriteLine();
            Console.WriteLine("Marking assignment 2 completed...");
            manager.MarkCompleted(2);
            DisplayTotals(manager);
        }

        private static void DisplayTotals(AssignmentManager manager)
        {
            Console.WriteLine("Total due: {0}", manager.TotalDue);
            Console.WriteLine("Completed: {0}", manager.Completed);
            Console.WriteLine("Remaining: {0}", manager.Remaining);
        }
    }
}
