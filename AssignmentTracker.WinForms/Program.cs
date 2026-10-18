using System;
using System.IO;
using System.Windows.Forms;

namespace AssignmentTrackerGUI
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm form = new MainForm();

            // Optional submission support. Normal startup still opens an empty tracker.
            if (args.Length > 0 && args[0] == "--capture-evidence")
            {
                string output = Path.GetFullPath(Path.Combine(
                    Environment.CurrentDirectory, "Evidence", "Screenshots"));
                form.Shown += async (sender, e) =>
                {
                    try
                    {
                        await form.CaptureEvidenceAsync(output);
                        MessageBox.Show(form,
                            "GUI checks passed. Screenshots and GUI_TEST_RESULTS.txt saved in:\n" +
                            output + "\n\nThe app will now close.",
                            "Evidence complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception error)
                    {
                        Directory.CreateDirectory(output);
                        File.WriteAllText(Path.Combine(output, "GUI_TEST_RESULTS.txt"),
                            "FAILED: " + error);
                        Environment.ExitCode = 1;
                        MessageBox.Show(form, error.Message,
                            "Evidence check failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        form.Close();
                    }
                };
            }

            Application.Run(form);
        }
    }
}
