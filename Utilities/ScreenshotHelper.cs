using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsCompetitionTask.Utilities
{
    public static class ScreenshotHelper
    {
        public static string TakeScreenshot(IWebDriver driver, string prefix)
        {
            try
            {
                var ss = ((ITakesScreenshot)driver).GetScreenshot();


                string currentDir = Environment.CurrentDirectory;
                // Move up three levels: bin → Debug → net8.0
                string projectRoot = Directory.GetParent(currentDir)!.Parent!.Parent!.FullName;

                string dir = Path.Combine(projectRoot, "Reports");
                if (Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                string file = Path.Combine(dir, $"{prefix}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                ss.SaveAsFile(file);
                return file;
            }
            catch { return string.Empty; }
        }
    }
}
