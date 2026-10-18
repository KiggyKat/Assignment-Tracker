using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace AssignmentTrackerGUI
{
    public partial class MainForm
    {
        // Runs on the form's UI thread after Shown. Uses the same controls and
        // button handlers as a person, and captures actual rendered form controls.
        internal async Task CaptureEvidenceAsync(string folder)
        {
            Directory.CreateDirectory(folder);
            List<string> results = new List<string>();
            await Task.Delay(400);
            CheckEvidence(manager.Assignments.Count == 0, "Fresh session is empty", results);

            addAssignmentButton.PerformClick();
            CheckEvidence(manager.Assignments.Count == 0 &&
                inputErrors.GetError(titleTextBox).Length > 0 &&
                inputErrors.GetError(courseComboBox).Length > 0,
                "Add button rejects missing title and course", results);
            await CaptureFormAsync(folder, "01_validation.png");

            titleTextBox.Text = "Class Implementation";
            courseComboBox.SelectedIndex = 0;
            dueDatePicker.Value = new DateTime(2026, 10, 18);
            descriptionTextBox.Text = "Implement and test the tracker classes.";
            completedCheckBox.Checked = true;
            addAssignmentButton.PerformClick();

            titleTextBox.Text = "Chemistry worksheet";
            courseComboBox.SelectedIndex = 1;
            dueDatePicker.Value = new DateTime(2026, 10, 20);
            descriptionTextBox.Text = "Complete chapter 4 exercises.";
            completedCheckBox.Checked = false;
            await CaptureFormAsync(folder, "02_assignment_input.png");
            addAssignmentButton.PerformClick();

            titleTextBox.Text = "Essay draft";
            courseComboBox.SelectedIndex = 2;
            dueDatePicker.Value = new DateTime(2026, 10, 25);
            completedCheckBox.Checked = false;
            addAssignmentButton.PerformClick();
            CheckEvidence(manager.Assignments.Count == 3 &&
                assignmentsGrid.Rows.Count == 3, "Add button creates three records", results);

            deadlinePicker.Value = new DateTime(2026, 10, 20);
            calculateTotalsButton.PerformClick();
            CheckEvidence(totalDueValue.Text == "2" &&
                completedValue.Text == "1" && remainingValue.Text == "1",
                "Calculate button includes cutoff day and excludes later work", results);

            assignmentsGrid.ClearSelection();
            assignmentsGrid.CurrentCell = assignmentsGrid.Rows[1].Cells[1];
            assignmentsGrid.Rows[1].Selected = true;
            CheckEvidence(detailsTextBox.Text == "Complete chapter 4 exercises." &&
                markCompletedButton.Enabled, "Row selection displays description", results);
            await CaptureFormAsync(folder, "03_deadline_results.png");

            markCompletedButton.PerformClick();
            CheckEvidence(totalDueValue.Text == "2" &&
                completedValue.Text == "2" && remainingValue.Text == "0" &&
                !markCompletedButton.Enabled,
                "Completion button refreshes status, counts, and button state", results);
            await CaptureFormAsync(folder, "04_completed_results.png");

            deadlinePicker.Value = new DateTime(2026, 10, 25);
            CheckEvidence(totalDueValue.Text == "3" && remainingValue.Text == "1",
                "Date change event refreshes counts", results);

            titleTextBox.Text = "Temporary text";
            descriptionTextBox.Text = "Clear this input only.";
            clearFieldsButton.PerformClick();
            CheckEvidence(titleTextBox.Text == "" && descriptionTextBox.Text == "" &&
                courseComboBox.SelectedIndex == -1 && manager.Assignments.Count == 3,
                "Clear button resets input and preserves existing records", results);

            deadlinePicker.Value = new DateTime(2026, 10, 17);
            calculateTotalsButton.PerformClick();
            CheckEvidence(totalDueValue.Text == "0" && completedValue.Text == "0" &&
                remainingValue.Text == "0", "Earlier cutoff produces zero counts", results);
            results.Add("Passed: 9; Failed: 0");
            results.Add("Actual Windows Forms captures from the running application.");
            File.WriteAllLines(Path.Combine(folder, "GUI_TEST_RESULTS.txt"), results);
        }

        private async Task CaptureFormAsync(string folder, string name)
        {
            // Let pending paint messages finish before capturing the real form.
            await Task.Delay(300);
            Refresh();
            using (Bitmap image = new Bitmap(Width, Height))
            {
                DrawToBitmap(image, new Rectangle(0, 0, Width, Height));
                image.Save(Path.Combine(folder, name), ImageFormat.Png);
            }
        }

        private static void CheckEvidence(bool condition, string name, List<string> results)
        {
            if (!condition)
            {
                throw new InvalidOperationException("GUI check failed: " + name);
            }
            results.Add("PASS: " + name);
        }
    }
}
