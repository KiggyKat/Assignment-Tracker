## Run from a terminal

Open a terminal in the extracted folder containing AssignmentTracker.sln:

    dotnet build AssignmentTracker.sln
    dotnet run --project AssignmentTracker.Tests
    dotnet run --project AssignmentTracker.Demo

The test runner returns exit code 0 when all tests pass and 1 when any test fails.

## Business classes and UML mapping

Class: Assignment
Properties: AssignmentID, Title, Description, DueDate, IsCompleted, CourseID
Methods: Assignment(), MarkCompleted(), IsDueDate(DateTime deadline)

Class: Course
Properties: CourseID, CourseName, InstructorName, Semester
Methods: Course()

Class: AssignmentManager
Properties: Assignments, Courses, SelectedDeadline, TotalDue, Completed, Remaining
Methods: AssignmentManager(List<Course> courses), AddAssignment(Assignment assignment), MarkCompleted(int assignmentID), CalculateTotals(), private IsValidAssignment(Assignment assignment)

## Behavior and validation

- The caller supplies positive, unique assignment IDs and a nonblank title.
- AddAssignment returns false for null, invalid, or duplicate records, including
  a CourseID that does not match a course with a nonblank name.
- Course choices are supplied to the constructor, as planned. A null course
  list throws ArgumentNullException; an empty list is allowed.
- MarkCompleted returns true for an existing ID, including an already-completed
  assignment, and false for an unknown ID.
- IsDueBy compares calendar dates, including overdue assignments and the entire
  cutoff day.
- CalculateTotals resets counts before counting only assignments due on or
  before SelectedDeadline. TotalDue always equals Completed plus Remaining
  after calculation.
- Successful additions and completion updates refresh counts automatically.
  After changing SelectedDeadline, call CalculateTotals(), as the planned
  Calculate Totals button will do.
- Lists and individual records remain mutable to match the UML. Application
  code should use AddAssignment and MarkCompleted rather than inserting records
  directly. If a record's public properties are changed directly, call
  CalculateTotals again. The supplied course list is copied, but its Course
  objects are shared.
- Data stays in memory. File saving, editing/deletion screens, notifications,
  and online features are outside this milestone.

## Tests and maintenance

AssignmentTests.cs checks defaults, property values, completion, and date
boundaries. CourseTests.cs checks defaults and property values.
AssignmentManagerTests.cs checks valid and invalid additions, duplicate IDs,
empty lists, unknown IDs, repeat calculations, completion changes, and cutoff
changes. The mixed-record test checks the proposal's five-due, two-completed,
three-remaining example and excludes later assignments.

There are 24 test methods. Each test creates its own records and manager, so
tests can be repeated without altering another test's data. Tests use fixed
dates for deadline calculations.

When adding or changing a business method, add or update a related test method,
register it in AssignmentTracker.Tests/Program.cs with Run(...), and rerun the
entire Tests project. Keep existing tests unless the intended behavior changes.

## Sample demonstration

The demo starts with three assignments. Two are due by October 20, 2026:
one completed and one remaining. The third is due later and is excluded.
The demo marks assignment 2 completed and shows the counts changing from
2 total / 1 completed / 1 remaining to 2 total / 2 completed / 0 remaining.