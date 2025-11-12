using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsCompetitionTask3.Drivers
{
    public sealed class WebDriverMngr
    {
        private static readonly Lazy<WebDriverMngr> lazy = new(() => new WebDriverMngr());
        public static WebDriverMngr Instance => lazy.Value;

        private IWebDriver _driver;
        private WebDriverMngr() { }

        public IWebDriver GetDriver()
        {
            if (_driver == null)
            {
                var options = new ChromeOptions();
                options.AddArgument("--start-maximized");
                _driver = new ChromeDriver(options);
            }
            return _driver;
        }

        public void Quit()
        {
            try { _driver?.Quit(); } catch { }
            _driver = null;
        }
    }
}
