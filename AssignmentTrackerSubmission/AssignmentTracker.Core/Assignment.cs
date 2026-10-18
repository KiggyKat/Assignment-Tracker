using System;

namespace AssignmentTracker
{
    // Represents one assignment. CourseID connects it to a Course record.
    public class Assignment
    {
        public int AssignmentID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int CourseID { get; set; }

        public Assignment()
        {
            Title = string.Empty;
            Description = string.Empty;
            DueDate = DateTime.Today;
            IsCompleted = false;
        }

        public void MarkCompleted()
        {
            IsCompleted = true;
        }

        public bool IsDueBy(DateTime deadline)
        {
            // Ignore times so the entire cutoff date is included.
            return DueDate.Date <= deadline.Date;
        }
    }
}
