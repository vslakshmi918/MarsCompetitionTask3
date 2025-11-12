using MarsCompetitionTask3.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MarsCompetitionTask3.Pages
{
    public class CertificationPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly WebDriverWait _waitForToaster;

        public CertificationPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            _waitForToaster = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
        }

        public void AddCertification(CertificationModel data, bool isCleanupNeeded = false)
        {
            try
            {
                var certificateInput = _driver.FindElement(By.XPath("//input[@placeholder='Certificate or Award']"));
                certificateInput.Clear();
                certificateInput.SendKeys(data.Certificate!);

                var yearDropdown = _driver.FindElement(By.Name("certificationYear"));
                var options = yearDropdown.FindElements(By.TagName("option"));

                foreach (var option in options)
                {
                    if (option.Text.Trim().Equals(data.Year, StringComparison.OrdinalIgnoreCase))
                    {
                        option.Click();
                        break;
                    }
                }

                var fromInput = _driver.FindElement(By.XPath("//input[@placeholder='Certified From (e.g. Adobe)']"));
                fromInput.Clear();
                fromInput.SendKeys(data.From!);

                if (isCleanupNeeded == true)
                {
                    Hooks.Hooks.AddToCleanupList(data);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding certification.", ex);
            }
            
        }

        public void ClickAddNew()
        {
            try
            {
                var addNewButton = _wait.Until(d =>
                    d.FindElement(By.XPath("//div[@data-tab='fourth']//div[text()='Add New' and contains(@class, 'button')]")));
                addNewButton.Click();
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new Exception("Timed out waiting for Add New button.", ex);
            }
            catch (NoSuchElementException ex)
            {
                throw new Exception("Add New button not found.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error clicking Add New button.", ex);
            }
        }

        public void ClickAdd()
        {
            var addButton = _driver.FindElement(By.XPath("//input[@type='button' and @value='Add']"));
            addButton.Click();
            _wait.Until(driver =>
                driver.FindElements(By.XPath("//table[@class='ui fixed table']//tr")).Count > 0);
        }

        public void ClickUpdate()
        {
            try
            {
                var updateButton = _wait.Until(d =>
                    d.FindElement(By.XPath("//input[@type='button' and @value='Update']")));
                updateButton.Click();
            }
            catch (NoSuchElementException ex)
            {
                throw new Exception("Update button not found.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error clicking Update button.", ex);
            }
        }

        public bool IsAddCertificationFormVisible()
        {
            try
            {
                var certificateInput = _driver.FindElement(By.XPath("//input[@placeholder='Certificate or Award']"));
                var yearDropdown = _driver.FindElement(By.Name("certificationYear"));
                var fromInput = _driver.FindElement(By.XPath("//input[@placeholder='Certified From (e.g. Adobe)']"));


                return certificateInput.Displayed && yearDropdown.Displayed &&
                       fromInput.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsCertificationAdded(string certificate, string from)
        {
            try
            {
                // Wait until a row with matching university and degree is visible
                var row = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                    By.XPath($"//table[@class='ui fixed table']//tr[td[contains(text(), '{certificate}')] and td[contains(text(), '{from}')]]")));

                return row != null;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"Certification record '{certificate} - {from}' not found within timeout.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while verifying Certification record: {ex.Message}");
                return false;
            }
        }

        public bool IsCertificationListed(CertificationModel data)
        {
            try
            {
                // Wait for either the record to appear or timeout
                var rows = _wait.Until(driver =>
                    driver.FindElements(By.XPath(
                        $"//table[@class='ui fixed table']//tr[td[text()='{data.Certificate}'] and " +
                        $"td[text()='{data.From}'] and " +
                        $"td[text()='{data.Year}']]"))
                );

                return rows.Count > 0;
            }
            catch (WebDriverTimeoutException)
            {
                // Timeout means record is not listed
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking if Certification record is listed: {ex.Message}");
                return false;
            }
        }

        public void EditCertification(CertificationModel oldData, CertificationModel newData)
        {
            try
            {
                // Wait for the correct row to be present before clicking edit
                var editIcon = _wait.Until(driver =>
                    driver.FindElement(By.XPath(
                        $"//table[@class='ui fixed table']//tr[td[text()='{oldData.Certificate}'] and " +
                        $"td[text()='{oldData.From}'] and " +
                        $"td[text()='{oldData.Year}']]//i[contains(@class,'write')]")));

                editIcon.Click();

                if (IsAddCertificationFormVisible())
                {
                    AddCertification(newData, isCleanupNeeded: true);
                    ClickUpdate();
                }
            }
            catch (NoSuchElementException)
            {
                throw new Exception($"Edit icon not found for record: {oldData.Certificate}, {oldData.From}, {oldData.Year}");
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception("Timed out waiting for the Certification edit form to appear.");
            }
        }

        public void DeleteCertification(CertificationModel data)
        {
            try
            {
                var deleteIcon = _driver.FindElement(By.XPath(
                    $"//table[@class='ui fixed table']//tr[td[text()='{data.Certificate}'] and " +
                    $"td[text()='{data.From}'] and " +
                    $"td[text()='{data.Year}']]//i[contains(@class,'remove')]"));

                deleteIcon.Click();

                // Wait until the specific row is no longer present
                _wait.Until(driver =>
                    driver.FindElements(By.XPath(
                        $"//table[@class='ui fixed table']//tr[td[text()='{data.Certificate}'] and " +
                        $"td[text()='{data.From}'] and " +
                        $"td[text()='{data.Year}']]")).Count == 0);
            }
            catch (NoSuchElementException)
            {
                throw new Exception($"Delete icon not found for record: {data.Certificate}, {data.From}, {data.Year}");
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception($"Record was not removed from the table after clicking delete: {data.Certificate} - {data.From}");
            }
        }

        public bool IsErrorMessageDisplayed(string message)
        {
            try
            {

                var element = _waitForToaster.Until(driver =>
                {
                    var elements = driver.FindElements(By.CssSelector("div.ns-box-inner"));
                    return elements.FirstOrDefault(); // returns null until at least one is found
                });

                if (element == null)
                    return false;

                string data = element.Text.Trim();
                return data.Contains(message);
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Toast did not appear in time.");
                return false;
            }
            catch (StaleElementReferenceException)
            {
                Console.WriteLine("Toast disappeared before reading text.");
                return false;
            }
        }
    }
}
