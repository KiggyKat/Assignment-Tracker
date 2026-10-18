using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AssignmentTrackerGUI
{
    public partial class MainForm
    {
        private IContainer components;
        private TextBox titleTextBox;
        private TextBox descriptionTextBox;
        private ComboBox courseComboBox;
        private DateTimePicker dueDatePicker;
        private CheckBox completedCheckBox;
        private Button addAssignmentButton;
        private Button clearFieldsButton;
        private DataGridView assignmentsGrid;
        private Button markCompletedButton;
        private TextBox detailsTextBox;
        private DateTimePicker deadlinePicker;
        private Button calculateTotalsButton;
        private Label totalDueValue;
        private Label completedValue;
        private Label remainingValue;
        private Label summaryCaption;
        private Label statusLabel;
        private ErrorProvider inputErrors;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            SuspendLayout();
            Text = "Assignment Tracker";
            ClientSize = new Size(1120, 750);
            MinimumSize = new Size(1136, 789);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10F);
            BackColor = Color.FromArgb(245, 247, 250);
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScroll = true;

            Label heading = MakeLabel("Assignment Tracker", 24, 18, 600, 36);
            heading.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            Controls.Add(heading);
            Controls.Add(MakeLabel("Record coursework and see what is due by your chosen date.",
                26, 60, 1000, 26));

            GroupBox inputGroup = new GroupBox
            {
                Text = "Add an assignment",
                Location = new Point(24, 104),
                Size = new Size(320, 582),
                TabIndex = 0
            };
            inputGroup.Controls.Add(MakeLabel("Assignment title *", 18, 32, 278, 24));
            titleTextBox = new TextBox
            {
                Location = new Point(18, 60), Size = new Size(274, 28),
                MaxLength = 120, TabIndex = 0, AccessibleName = "Assignment title"
            };
            inputGroup.Controls.Add(titleTextBox);
            inputGroup.Controls.Add(MakeLabel("Course *", 18, 104, 274, 24));
            courseComboBox = new ComboBox
            {
                Location = new Point(18, 132), Size = new Size(274, 28),
                DropDownStyle = ComboBoxStyle.DropDownList, TabIndex = 1,
                AccessibleName = "Course"
            };
            inputGroup.Controls.Add(courseComboBox);
            inputGroup.Controls.Add(MakeLabel("Due date", 18, 176, 274, 24));
            dueDatePicker = new DateTimePicker
            {
                Location = new Point(18, 204), Size = new Size(274, 28),
                Format = DateTimePickerFormat.Custom, CustomFormat = "MMM dd, yyyy",
                TabIndex = 2, AccessibleName = "Assignment due date"
            };
            inputGroup.Controls.Add(dueDatePicker);
            inputGroup.Controls.Add(MakeLabel("Description (optional)", 18, 248, 274, 24));
            descriptionTextBox = new TextBox
            {
                Location = new Point(18, 276), Size = new Size(274, 116),
                Multiline = true, ScrollBars = ScrollBars.Vertical,
                MaxLength = 2000, TabIndex = 3, AccessibleName = "Assignment description"
            };
            inputGroup.Controls.Add(descriptionTextBox);
            completedCheckBox = new CheckBox
            {
                Text = "Already completed", Location = new Point(18, 408),
                Size = new Size(274, 28), TabIndex = 4
            };
            inputGroup.Controls.Add(completedCheckBox);
            addAssignmentButton = MakeButton("Add Assignment", 18, 454, 274, 38, 5);
            addAssignmentButton.Click += AddAssignmentButton_Click;
            inputGroup.Controls.Add(addAssignmentButton);
            clearFieldsButton = MakeButton("Clear Fields", 18, 502, 274, 34, 6);
            clearFieldsButton.Click += ClearFieldsButton_Click;
            inputGroup.Controls.Add(clearFieldsButton);
            inputGroup.Controls.Add(MakeLabel("* Required fields", 18, 544, 274, 24));
            Controls.Add(inputGroup);

            GroupBox recordsGroup = new GroupBox
            {
                Text = "Assignments and selected description",
                Location = new Point(364, 104),
                Size = new Size(732, 364),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                TabIndex = 1
            };
            assignmentsGrid = new DataGridView
            {
                Location = new Point(18, 30), Size = new Size(696, 200),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true, AllowUserToAddRows = false,
                AllowUserToDeleteRows = false, AllowUserToResizeRows = false,
                RowHeadersVisible = false, MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White, TabIndex = 0,
                AccessibleName = "Assignment list"
            };
            assignmentsGrid.Columns.Add("AssignmentID", "ID");
            assignmentsGrid.Columns.Add("Title", "Assignment");
            assignmentsGrid.Columns.Add("Course", "Course");
            assignmentsGrid.Columns.Add("DueDate", "Due date");
            assignmentsGrid.Columns.Add("Status", "Status");
            assignmentsGrid.Columns[0].FillWeight = 30;
            assignmentsGrid.Columns[1].FillWeight = 160;
            assignmentsGrid.Columns[2].FillWeight = 110;
            assignmentsGrid.Columns[3].FillWeight = 90;
            assignmentsGrid.Columns[4].FillWeight = 95;
            assignmentsGrid.Columns[3].ValueType = typeof(System.DateTime);
            assignmentsGrid.Columns[3].DefaultCellStyle.Format = "MMM dd, yyyy";
            assignmentsGrid.RowTemplate.Height = 30;
            // Subscribe after rows are populated in RefreshAssignments.
            // The handler also guards against a row that has not received its ID tag.
            assignmentsGrid.SelectionChanged += AssignmentsGrid_SelectionChanged;
            recordsGroup.Controls.Add(assignmentsGrid);
            markCompletedButton = MakeButton("Mark Completed", 18, 240, 174, 34, 1);
            markCompletedButton.Enabled = false;
            markCompletedButton.Click += MarkCompletedButton_Click;
            recordsGroup.Controls.Add(markCompletedButton);
            recordsGroup.Controls.Add(MakeLabel("Select a row to view its description.",
                206, 244, 490, 26));
            detailsTextBox = new TextBox
            {
                Location = new Point(18, 284), Size = new Size(696, 60),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical,
                BackColor = Color.White, TabIndex = 2,
                AccessibleName = "Selected assignment description"
            };
            recordsGroup.Controls.Add(detailsTextBox);
            Controls.Add(recordsGroup);

            GroupBox totalsGroup = new GroupBox
            {
                Text = "Deadline summary",
                Location = new Point(364, 484), Size = new Size(732, 202),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                TabIndex = 2
            };
            totalsGroup.Controls.Add(MakeLabel("Due by", 18, 32, 70, 28));
            deadlinePicker = new DateTimePicker
            {
                Location = new Point(92, 30), Size = new Size(200, 28),
                Format = DateTimePickerFormat.Custom, CustomFormat = "MMM dd, yyyy",
                TabIndex = 0, AccessibleName = "Summary cutoff date"
            };
            deadlinePicker.ValueChanged += DeadlinePicker_ValueChanged;
            totalsGroup.Controls.Add(deadlinePicker);
            calculateTotalsButton = MakeButton("Calculate Totals", 310, 28, 174, 34, 1);
            calculateTotalsButton.Click += CalculateTotalsButton_Click;
            totalsGroup.Controls.Add(calculateTotalsButton);
            summaryCaption = MakeLabel("", 18, 74, 696, 28);
            summaryCaption.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            totalsGroup.Controls.Add(summaryCaption);
            totalsGroup.Controls.Add(MakeLabel("TOTAL DUE", 18, 112, 210, 24));
            totalsGroup.Controls.Add(MakeLabel("COMPLETED", 250, 112, 210, 24));
            totalsGroup.Controls.Add(MakeLabel("REMAINING", 482, 112, 210, 24));
            totalDueValue = MakeLabel("0", 18, 140, 210, 42);
            completedValue = MakeLabel("0", 250, 140, 210, 42);
            remainingValue = MakeLabel("0", 482, 140, 210, 42);
            foreach (Label value in new Label[] { totalDueValue, completedValue, remainingValue })
            {
                value.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
                totalsGroup.Controls.Add(value);
            }
            Controls.Add(totalsGroup);
            statusLabel = MakeLabel("Ready. Assignments stay in memory until the app closes.",
                26, 706, 1068, 28);
            statusLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            statusLabel.AccessibleName = "Application status";
            Controls.Add(statusLabel);
            inputErrors = new ErrorProvider(components)
            {
                ContainerControl = this,
                BlinkStyle = ErrorBlinkStyle.NeverBlink
            };
            AcceptButton = addAssignmentButton;
            ResumeLayout(false);
        }

        private static Label MakeLabel(string text, int x, int y, int width, int height)
        {
            return new Label
            {
                Text = text, Location = new Point(x, y), Size = new Size(width, height),
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private static Button MakeButton(string text, int x, int y,
            int width, int height, int tabIndex)
        {
            return new Button
            {
                Text = text, Location = new Point(x, y), Size = new Size(width, height),
                TabIndex = tabIndex, UseVisualStyleBackColor = true
            };
        }
    }
}
