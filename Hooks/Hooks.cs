using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using MarsCompetitionTask.Utilities;
using MarsCompetitionTask3.Drivers;
using MarsCompetitionTask3.Models;
using MarsCompetitionTask3.Pages;
using OpenQA.Selenium;
using Reqnroll;

namespace MarsCompetitionTask3.Hooks
{
    [Binding]
    public sealed class Hooks
    {
        private static ExtentReports? _extent;
        private static ExtentTest? _scenario;
        private static IWebDriver? _driver;
        private readonly ScenarioContext _context;

        // Track data added during the scenario
        private static readonly List<EducationModel> _educationAdded = new();
        private static readonly List<CertificationModel> _certificatesAdded = new();

        public Hooks(ScenarioContext context) => _context = context;

        public static void AddToCleanupList(object data)
        {
            try
            {
                _educationAdded.Add((EducationModel)data);
            }
            catch
            {
                _certificatesAdded.Add((CertificationModel)data);

            }

        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            string currentDir = Environment.CurrentDirectory;
            // Move up three levels: bin → Debug → net8.0
            string projectRoot = Directory.GetParent(currentDir)!.Parent!.Parent!.FullName;
            string dir = Path.Combine(projectRoot, "Reports");
            if (Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var html = new ExtentSparkReporter(Path.Combine(dir, $"ExtentReport_{timeStamp}.html"));
            _extent = new ExtentReports();
            _extent.AttachReporter(html);
            _extent.AddSystemInfo("Environment", "QA");
            _extent.AddSystemInfo("Browser", "Chrome");
            _extent.AddSystemInfo("Tester", Environment.MachineName);
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _driver = WebDriverMngr.Instance.GetDriver();
            _scenario = _extent!.CreateTest(_context.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            try
            {
                // Clean up added records if any exist
                if (_educationAdded.Count > 0)
                {

                    var educationPage = new EducationPage(_driver!);

                    foreach (var record in _educationAdded)
                    {
                        try
                        {
                            if (_context.ScenarioInfo.CombinedTags[0] != "Delete" || _context.ScenarioInfo.CombinedTags[0] != "Duplicate" || _context.ScenarioInfo.CombinedTags[0] != "Invalid")
                            {
                                educationPage.DeleteEducation(record);

                            }
                            _scenario!.Log(Status.Info, $"Cleanup: Deleted record '{record.University} - {record.Degree}'");
                        }
                        catch (Exception ex)
                        {
                            _scenario!.Log(Status.Warning, $"Cleanup failed for '{record.University}': {ex.Message}");
                        }
                    }

                    _educationAdded.Clear(); // empty list after cleanup
                }

                if (_certificatesAdded.Count > 0)
                {

                    var certificationPage = new CertificationPage(_driver!);

                    foreach (var record in _certificatesAdded)
                    {
                        try
                        {
                            if (_context.ScenarioInfo.CombinedTags[0] != "Delete" || _context.ScenarioInfo.CombinedTags[0] != "Duplicate" || _context.ScenarioInfo.CombinedTags[0] != "Invalid")
                            {
                                certificationPage.DeleteCertification(record);

                            }
                            _scenario!.Log(Status.Info, $"Cleanup: Deleted record '{record.Certificate} - {record.From}'");
                        }
                        catch (Exception ex)
                        {
                            _scenario!.Log(Status.Warning, $"Cleanup failed for '{record.Certificate}': {ex.Message}");
                        }
                    }

                    _certificatesAdded.Clear(); // empty list after cleanup
                }

                // Capture screenshot if failed
                if (_context.TestError != null)
                {
                    string ssPath = ScreenshotHelper.TakeScreenshot(_driver!, _context.ScenarioInfo.Title);
                    _scenario!.Fail(_context.TestError.Message).AddScreenCaptureFromPath(ssPath);
                }
            }
            finally
            {
                WebDriverMngr.Instance.Quit(); // Always quit browser
            }
        }

        [AfterTestRun]
        public static void AfterTestRun() => _extent.Flush();
    }
}
