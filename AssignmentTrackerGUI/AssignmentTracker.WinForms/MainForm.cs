using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AssignmentTracker;

namespace AssignmentTrackerGUI
{
    public partial class MainForm : Form
    {
        private readonly AssignmentManager manager;
        private int nextAssignmentID = 1;

        public MainForm()
        {
            InitializeComponent();
            // A small fixed course list keeps the beginner project manageable.
            manager = new AssignmentManager(new List<Course>
            {
                new Course { CourseID = 1, CourseName = "C# Programming" },
                new Course { CourseID = 2, CourseName = "Chemistry" },
                new Course { CourseID = 3, CourseName = "English" }
            });
            courseComboBox.DisplayMember = "CourseName";
            courseComboBox.ValueMember = "CourseID";
            courseComboBox.DataSource = manager.Courses;
            courseComboBox.SelectedIndex = -1;
            dueDatePicker.Value = DateTime.Today;
            deadlinePicker.Value = DateTime.Today.AddDays(7);
            RefreshAssignments();
            UpdateTotals();
        }

        private void AddAssignmentButton_Click(object sender, EventArgs e)
        {
            inputErrors.Clear();
            bool valid = true;

            if (string.IsNullOrWhiteSpace(titleTextBox.Text))
            {
                inputErrors.SetError(titleTextBox, "Enter an assignment title.");
                valid = false;
            }

            Course selectedCourse = courseComboBox.SelectedItem as Course;
            if (selectedCourse == null)
            {
                inputErrors.SetError(courseComboBox, "Select a course.");
                valid = false;
            }

            if (!valid)
            {
                SetStatus("Enter a title and select a course before adding.", true);
                if (string.IsNullOrWhiteSpace(titleTextBox.Text))
                {
                    titleTextBox.Focus();
                }
                else
                {
                    courseComboBox.Focus();
                }
                return;
            }

            Assignment assignment = new Assignment
            {
                AssignmentID = nextAssignmentID,
                Title = titleTextBox.Text.Trim(),
                Description = descriptionTextBox.Text.Trim(),
                CourseID = selectedCourse.CourseID,
                DueDate = dueDatePicker.Value.Date,
                IsCompleted = completedCheckBox.Checked
            };

            if (!manager.AddAssignment(assignment))
            {
                SetStatus("The assignment could not be added. Check the input.", true);
                return;
            }

            nextAssignmentID++;
            RefreshAssignments(assignment.AssignmentID);
            UpdateTotals();
            ClearInputFields();
            SetStatus("Assignment added. Totals refreshed.", false);
        }

        private void MarkCompletedButton_Click(object sender, EventArgs e)
        {
            if (assignmentsGrid.SelectedRows.Count == 0)
            {
                SetStatus("Select an assignment row first.", true);
                return;
            }

            // Row tags store the real ID; sorting the grid cannot change the target.
            int assignmentID = (int)assignmentsGrid.SelectedRows[0].Tag;
            if (manager.MarkCompleted(assignmentID))
            {
                RefreshAssignments(assignmentID);
                UpdateTotals();
                SetStatus("Assignment marked completed. Totals refreshed.", false);
            }
        }

        private void CalculateTotalsButton_Click(object sender, EventArgs e)
        {
            UpdateTotals();
            SetStatus("Totals calculated for the selected Due by date.", false);
        }

        private void ClearFieldsButton_Click(object sender, EventArgs e)
        {
            ClearInputFields();
            SetStatus("Input fields cleared. Existing assignments are unchanged.", false);
        }

        private void ClearInputFields()
        {
            titleTextBox.Clear();
            descriptionTextBox.Clear();
            courseComboBox.SelectedIndex = -1;
            dueDatePicker.Value = DateTime.Today;
            completedCheckBox.Checked = false;
            inputErrors.Clear();
            titleTextBox.Focus();
        }

        private void DeadlinePicker_ValueChanged(object sender, EventArgs e)
        {
            if (manager == null)
            {
                return; // Initialization can trigger this event before the manager exists.
            }
            UpdateTotals();
            SetStatus("Due by date changed. Totals refreshed.", false);
        }

        private void AssignmentsGrid_SelectionChanged(object sender, EventArgs e)
        {
            bool selected = assignmentsGrid.SelectedRows.Count > 0 &&
                assignmentsGrid.SelectedRows[0].Tag is int;
            markCompletedButton.Enabled = selected;
            if (!selected)
            {
                detailsTextBox.Clear();
                return;
            }

            int assignmentID = (int)assignmentsGrid.SelectedRows[0].Tag;
            foreach (Assignment assignment in manager.Assignments)
            {
                if (assignment.AssignmentID == assignmentID)
                {
                    detailsTextBox.Text = assignment.Description;
                    markCompletedButton.Enabled = !assignment.IsCompleted;
                    return;
                }
            }
        }

        private void RefreshAssignments(int selectedID = -1)
        {
            assignmentsGrid.Rows.Clear();
            foreach (Assignment assignment in manager.Assignments)
            {
                string courseName = "";
                foreach (Course course in manager.Courses)
                {
                    if (course.CourseID == assignment.CourseID)
                    {
                        courseName = course.CourseName;
                        break;
                    }
                }

                int index = assignmentsGrid.Rows.Add(assignment.AssignmentID,
                    assignment.Title, courseName, assignment.DueDate,
                    assignment.IsCompleted ? "Completed" : "Remaining");
                assignmentsGrid.Rows[index].Tag = assignment.AssignmentID;
            }

            assignmentsGrid.ClearSelection();
            foreach (DataGridViewRow row in assignmentsGrid.Rows)
            {
                if ((int)row.Tag == selectedID)
                {
                    assignmentsGrid.CurrentCell = row.Cells[1];
                    row.Selected = true;
                    break;
                }
            }
            // Row creation may raise selection events before the row tag is assigned.
            AssignmentsGrid_SelectionChanged(this, EventArgs.Empty);
        }

        private void UpdateTotals()
        {
            manager.SelectedDeadline = deadlinePicker.Value.Date;
            manager.CalculateTotals();
            totalDueValue.Text = manager.TotalDue.ToString();
            completedValue.Text = manager.Completed.ToString();
            remainingValue.Text = manager.Remaining.ToString();
            summaryCaption.Text = "Assignments due on or before " +
                deadlinePicker.Value.ToString("MMM d, yyyy") + " (includes overdue work)";
        }

        private void SetStatus(string message, bool isError)
        {
            statusLabel.ForeColor = isError
                ? System.Drawing.Color.DarkRed
                : System.Drawing.Color.FromArgb(38, 77, 60);
            statusLabel.Text = message;
        }
    }
}
