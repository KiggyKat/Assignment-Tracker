using System;

namespace AssignmentTrackerTests
{
    // A simple test runner avoids third-party packages in this beginner project.
    internal class Program
    {
        private static int passed;
        private static int failed;

        private static int Main()
        {
            Run("Assignment constructor defaults", AssignmentTests.ConstructorSetsDefaults);
            Run("Assignment stores property values", AssignmentTests.PropertiesStoreValues);
            Run("MarkCompleted sets completion status", AssignmentTests.MarkCompletedSetsStatus);
            Run("Repeated completion is safe", AssignmentTests.RepeatedCompletionIsSafe);
            Run("Due date before deadline is included", AssignmentTests.BeforeDeadlineIsIncluded);
            Run("Entire cutoff day is included", AssignmentTests.SameDayIgnoresTime);
            Run("Due date after deadline is excluded", AssignmentTests.AfterDeadlineIsExcluded);
            Run("Course constructor defaults", CourseTests.ConstructorSetsDefaults);
            Run("Course stores property values", CourseTests.PropertiesStoreValues);
            Run("Manager constructor initializes collections", AssignmentManagerTests.ConstructorInitializesState);
            Run("Null course list is rejected", AssignmentManagerTests.NullCoursesAreRejected);
            Run("Valid assignment is added", AssignmentManagerTests.ValidAssignmentIsAdded);
            Run("Null assignment is rejected", AssignmentManagerTests.NullAssignmentIsRejected);
            Run("Empty and blank titles are rejected", AssignmentManagerTests.BlankTitlesAreRejected);
            Run("Missing and unnamed courses are rejected", AssignmentManagerTests.InvalidCoursesAreRejected);
            Run("Nonpositive assignment IDs are rejected", AssignmentManagerTests.InvalidIdsAreRejected);
            Run("Duplicate assignment ID is rejected", AssignmentManagerTests.DuplicateIdIsRejected);
            Run("Empty list produces zero totals", AssignmentManagerTests.EmptyListHasZeroTotals);
            Run("Mixed deadline and status totals are correct", AssignmentManagerTests.MixedTotalsAreCorrect);
            Run("MarkCompleted updates manager totals", AssignmentManagerTests.MarkCompletedUpdatesTotals);
            Run("Unknown assignment ID changes nothing", AssignmentManagerTests.UnknownIdChangesNothing);
            Run("Repeated calculations reset counts", AssignmentManagerTests.RepeatedCalculationsResetCounts);
            Run("Changing deadline recalculates the selected group", AssignmentManagerTests.ChangingDeadlineUpdatesTotals);
            Run("No qualifying assignments produces zero totals", AssignmentManagerTests.NoQualifyingAssignmentsHasZeroTotals);

            Console.WriteLine();
            Console.WriteLine("Passed: {0}; Failed: {1}; Total: {2}",
                passed, failed, passed + failed);
            return failed == 0 ? 0 : 1;
        }

        private static void Run(string name, Action test)
        {
            try
            {
                test();
                passed++;
                Console.WriteLine("PASS: " + name);
            }
            catch (Exception error)
            {
                failed++;
                Console.WriteLine("FAIL: " + name + " - " + error.Message);
            }
        }
    }

    internal static class Assert
    {
        public static void Equal<T>(T expected, T actual)
        {
            if (!object.Equals(expected, actual))
            {
                throw new Exception("Expected " + expected + "; received " + actual + ".");
            }
        }

        public static void Throws<T>(Action action) where T : Exception
        {
            try
            {
                action();
            }
            catch (T)
            {
                return;
            }

            throw new Exception("Expected exception " + typeof(T).Name + ".");
        }
    }
}
