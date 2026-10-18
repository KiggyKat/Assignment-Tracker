using System;
using System.Collections.Generic;

namespace AssignmentTracker
{
    // Coordinates records and calculations without depending on a user interface.
    public class AssignmentManager
    {
        public List<Assignment> Assignments { get; }
        public List<Course> Courses { get; }
        public DateTime SelectedDeadline { get; set; }
        public int TotalDue { get; private set; }
        public int Completed { get; private set; }
        public int Remaining { get; private set; }

        public AssignmentManager(List<Course> courses)
        {
            if (courses == null)
            {
                throw new ArgumentNullException(nameof(courses));
            }

            // Copy the collection; the individual Course objects are shared.
            Courses = new List<Course>(courses);
            Assignments = new List<Assignment>();
            SelectedDeadline = DateTime.Today;
        }

        public bool AddAssignment(Assignment assignment)
        {
            if (!IsValidAssignment(assignment))
            {
                return false;
            }

            // Identifiers are supplied by the caller and must be unique.
            foreach (Assignment existing in Assignments)
            {
                if (existing.AssignmentID == assignment.AssignmentID)
                {
                    return false;
                }
            }

            Assignments.Add(assignment);
            CalculateTotals();
            return true;
        }

        public bool MarkCompleted(int assignmentID)
        {
            foreach (Assignment assignment in Assignments)
            {
                if (assignment.AssignmentID == assignmentID)
                {
                    assignment.MarkCompleted();
                    CalculateTotals();
                    return true;
                }
            }

            return false;
        }

        public void CalculateTotals()
        {
            // Reset first so repeated calculations do not accumulate counts.
            TotalDue = 0;
            Completed = 0;
            Remaining = 0;

            foreach (Assignment assignment in Assignments)
            {
                if (assignment.IsDueBy(SelectedDeadline))
                {
                    TotalDue++;
                    if (assignment.IsCompleted)
                    {
                        Completed++;
                    }
                    else
                    {
                        Remaining++;
                    }
                }
            }
        }

        private bool IsValidAssignment(Assignment assignment)
        {
            if (assignment == null ||
                assignment.AssignmentID <= 0 ||
                string.IsNullOrWhiteSpace(assignment.Title))
            {
                return false;
            }

            foreach (Course course in Courses)
            {
                if (course.CourseID == assignment.CourseID &&
                    !string.IsNullOrWhiteSpace(course.CourseName))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
