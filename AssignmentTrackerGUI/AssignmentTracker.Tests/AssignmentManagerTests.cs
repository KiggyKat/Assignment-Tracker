using System;
using System.Collections.Generic;
using AssignmentTracker;

namespace AssignmentTrackerTests
{
    internal static class AssignmentManagerTests
    {
        private static AssignmentManager CreateManager()
        {
            AssignmentManager manager = new AssignmentManager(new List<Course>
            {
                new Course { CourseID = 1, CourseName = "Programming" },
                new Course { CourseID = 2, CourseName = "Chemistry" }
            });
            manager.SelectedDeadline = new DateTime(2026, 10, 20);
            return manager;
        }

        private static Assignment MakeAssignment(int id, int day, bool completed = false)
        {
            return new Assignment
            {
                AssignmentID = id,
                Title = "Assignment " + id,
                CourseID = 1,
                DueDate = new DateTime(2026, 10, day),
                IsCompleted = completed
            };
        }

        private static void CheckTotals(AssignmentManager manager,
            int total, int completed, int remaining)
        {
            Assert.Equal(total, manager.TotalDue);
            Assert.Equal(completed, manager.Completed);
            Assert.Equal(remaining, manager.Remaining);
            Assert.Equal(manager.TotalDue, manager.Completed + manager.Remaining);
        }

        public static void ConstructorInitializesState()
        {
            List<Course> courses = new List<Course>
            {
                new Course { CourseID = 1, CourseName = "Programming" }
            };
            AssignmentManager manager = new AssignmentManager(courses);
            Assert.Equal(0, manager.Assignments.Count);
            Assert.Equal(1, manager.Courses.Count);
            Assert.Equal(DateTime.Today, manager.SelectedDeadline);
            CheckTotals(manager, 0, 0, 0);
            courses.Clear();
            Assert.Equal(1, manager.Courses.Count);
        }

        public static void NullCoursesAreRejected()
        {
            Assert.Throws<ArgumentNullException>(() => new AssignmentManager(null));
        }

        public static void ValidAssignmentIsAdded()
        {
            AssignmentManager manager = CreateManager();
            Assignment assignment = MakeAssignment(1, 18);
            assignment.CourseID = 2;
            Assert.Equal(true, manager.AddAssignment(assignment));
            Assert.Equal(1, manager.Assignments.Count);
            Assert.Equal(assignment, manager.Assignments[0]);
            CheckTotals(manager, 1, 0, 1);
        }

        public static void NullAssignmentIsRejected()
        {
            AssignmentManager manager = CreateManager();
            Assert.Equal(false, manager.AddAssignment(null));
            Assert.Equal(0, manager.Assignments.Count);
            CheckTotals(manager, 0, 0, 0);
        }

        public static void BlankTitlesAreRejected()
        {
            AssignmentManager manager = CreateManager();
            foreach (string title in new string[] { "", "   ", null })
            {
                Assignment assignment = MakeAssignment(1, 18);
                assignment.Title = title;
                Assert.Equal(false, manager.AddAssignment(assignment));
            }
            Assert.Equal(0, manager.Assignments.Count);
        }

        public static void InvalidCoursesAreRejected()
        {
            AssignmentManager manager = CreateManager();
            Assignment assignment = MakeAssignment(1, 18);
            assignment.CourseID = 99;
            Assert.Equal(false, manager.AddAssignment(assignment));
            manager.Courses.Add(new Course { CourseID = 3, CourseName = "  " });
            assignment.CourseID = 3;
            Assert.Equal(false, manager.AddAssignment(assignment));
            AssignmentManager noCourses = new AssignmentManager(new List<Course>());
            Assert.Equal(false, noCourses.AddAssignment(MakeAssignment(1, 18)));
            Assert.Equal(0, manager.Assignments.Count);
        }

        public static void InvalidIdsAreRejected()
        {
            AssignmentManager manager = CreateManager();
            Assert.Equal(false, manager.AddAssignment(MakeAssignment(0, 18)));
            Assert.Equal(false, manager.AddAssignment(MakeAssignment(-1, 18)));
            Assert.Equal(0, manager.Assignments.Count);
        }

        public static void DuplicateIdIsRejected()
        {
            AssignmentManager manager = CreateManager();
            Assert.Equal(true, manager.AddAssignment(MakeAssignment(1, 18)));
            Assert.Equal(false, manager.AddAssignment(MakeAssignment(1, 19, true)));
            Assert.Equal(1, manager.Assignments.Count);
            CheckTotals(manager, 1, 0, 1);
        }

        public static void EmptyListHasZeroTotals()
        {
            AssignmentManager manager = CreateManager();
            manager.CalculateTotals();
            CheckTotals(manager, 0, 0, 0);
        }

        public static void MixedTotalsAreCorrect()
        {
            AssignmentManager manager = CreateManager();
            // Five qualify: two completed, three remaining; future work is excluded.
            manager.AddAssignment(MakeAssignment(1, 1, true));
            manager.AddAssignment(MakeAssignment(2, 18, true));
            manager.AddAssignment(MakeAssignment(3, 19));
            manager.AddAssignment(MakeAssignment(4, 20));
            Assignment cutoffAssignment = MakeAssignment(5, 20);
            cutoffAssignment.DueDate = cutoffAssignment.DueDate.AddHours(23).AddMinutes(59);
            manager.AddAssignment(cutoffAssignment);
            manager.AddAssignment(MakeAssignment(6, 21, true));
            manager.AddAssignment(MakeAssignment(7, 25));
            manager.CalculateTotals();
            CheckTotals(manager, 5, 2, 3);
        }

        public static void MarkCompletedUpdatesTotals()
        {
            AssignmentManager manager = CreateManager();
            manager.AddAssignment(MakeAssignment(1, 18));
            Assert.Equal(true, manager.MarkCompleted(1));
            Assert.Equal(true, manager.Assignments[0].IsCompleted);
            CheckTotals(manager, 1, 1, 0);
            Assert.Equal(true, manager.MarkCompleted(1));
            CheckTotals(manager, 1, 1, 0);
            manager.AddAssignment(MakeAssignment(2, 25));
            Assert.Equal(true, manager.MarkCompleted(2));
            CheckTotals(manager, 1, 1, 0);
        }

        public static void UnknownIdChangesNothing()
        {
            AssignmentManager manager = CreateManager();
            manager.AddAssignment(MakeAssignment(1, 18));
            Assert.Equal(false, manager.MarkCompleted(99));
            Assert.Equal(false, manager.Assignments[0].IsCompleted);
            CheckTotals(manager, 1, 0, 1);
        }

        public static void RepeatedCalculationsResetCounts()
        {
            AssignmentManager manager = CreateManager();
            manager.AddAssignment(MakeAssignment(1, 18, true));
            manager.AddAssignment(MakeAssignment(2, 20));
            manager.CalculateTotals();
            manager.CalculateTotals();
            CheckTotals(manager, 2, 1, 1);
        }

        public static void ChangingDeadlineUpdatesTotals()
        {
            AssignmentManager manager = CreateManager();
            manager.AddAssignment(MakeAssignment(1, 18, true));
            manager.AddAssignment(MakeAssignment(2, 20));
            manager.AddAssignment(MakeAssignment(3, 25));
            manager.SelectedDeadline = new DateTime(2026, 10, 25);
            manager.CalculateTotals();
            CheckTotals(manager, 3, 1, 2);
            manager.SelectedDeadline = new DateTime(2026, 10, 18);
            manager.CalculateTotals();
            CheckTotals(manager, 1, 1, 0);
            manager.SelectedDeadline = new DateTime(2026, 9, 30);
            manager.CalculateTotals();
            CheckTotals(manager, 0, 0, 0);
        }

        public static void NoQualifyingAssignmentsHasZeroTotals()
        {
            AssignmentManager manager = CreateManager();
            manager.AddAssignment(MakeAssignment(1, 25, true));
            manager.AddAssignment(MakeAssignment(2, 26));
            manager.CalculateTotals();
            CheckTotals(manager, 0, 0, 0);
        }
    }
}
