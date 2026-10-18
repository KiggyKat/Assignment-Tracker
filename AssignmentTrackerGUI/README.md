## Run in VS Code on Windows

Windows Forms requires Windows. Install the .NET 8 SDK (not just the runtime).

1. Extract the entire ZIP.
2. In VS Code, choose File > Open Folder and open the extracted AssignmentTrackerGUI
   folder (the folder containing AssignmentTracker.sln).
3. Choose Terminal > New Terminal.
4. Run:

       dotnet run --project AssignmentTracker.WinForms

A desktop window opens. Don't use the editor's Run button on a text file.
You can also double-click Run_GUI.cmd (easiest).

## Other commands

    dotnet build AssignmentTracker.sln
    dotnet run --project AssignmentTracker.Tests
    dotnet run --project AssignmentTracker.Demo

The tests are a console runner; they do not use Test Explorer. Visual Studio users can open AssignmentTracker.sln and set AssignmentTracker.WinForms as the startup project.

## GUI layout

One main window contains both the input and output areas:

- Input: required title, required course selection, due-date picker, optional
  multiline description, completion checkbox, Add Assignment, and Clear Fields.

- Output: a read-only assignment grid with ID, title, course, due date, and
  completion status. Select a row to view its description below the grid.

- Summary: a Due by picker, Calculate Totals button, three labeled counts
  (Total Due, Completed, Remaining), and the inclusive cutoff date.
  
- Feedback: a status message and field error indicators for missing required input.

Tab order follows the input fields, then the assignment table and summary.
The form supports DPI scaling and has a minimum size to keep controls visible.

## Working events

Event: Add Assignment Click
Visible Result: Validates title and course, creates assignment through the manager, refreshes the table and totals, and clears entry fields

Event: Mark Completed Click
Visible Result: Completes the selected assignment and refreshes status and totals. Disabled when no row or an already completed row is selected.

Event: Calculate Totals Click
Visible Result: Calls the business calculation for the selected Due by date and displays three counts

Event: Clear Fields Click
Visible Result: Clears only the entry fields and validation indicators

Event: Due by date Changed
Visible Result: Refreshes the count for the new date

Event: Assignment selection Changed
Visible Result: Displays that assignment's description and enables the completion button when appropriate

Grid rows store their assignment ID in Tag so sorting does not change which
assignment is completed.
Dates are compared by calendar day, including overdue work and the entire cutoff day. Future assignments stay visible in the grid but are excluded from earlier summaries.

## Screen mock-ups

The Mockups folder contains three labeled PNG images:
1. 01_input.png: input area with an example assignment being entered.
2. 02_results.png: assignments and a summary of two due, one completed, one remaining.
3. 03_completed.png: the same view after marking the chemistry assignment completed.

These are runtime screenshots.
Native Windows rendering, date formatting, and scaling may vary once the application opens.
The app starts with no assignments; enter the example records using the manual checklist.

## Scope and submission

Default course choices at this time are C# Programming, Chemistry, and English.
All data stays in memory and is cleared when the app closes.
There is no file saving, editing, deletion, login, or online synchronization.
