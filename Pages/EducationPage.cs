using MarsCompetitionTask3.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsCompetitionTask3.Pages
{
    public class EducationPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly WebDriverWait _waitForToaster;

        public EducationPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            _waitForToaster = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
        }


        // Delete button (❌ remove icon)
        private IWebElement DeleteButton(string language) =>
            _driver.FindElement(By.XPath($"//td[text()='{language}']/following-sibling::td//i[contains(@class,'remove icon')]"));

        private IWebElement EducationTab => _driver.FindElement(By.XPath("//a[text()='Education']"));
        private IWebElement AddNewButton => _driver.FindElement(By.XPath("//div[@data-tab='third']//button[text()='Add New']"));
        private IWebElement UniversityInput => _driver.FindElement(By.Name("instituteName"));
        private IWebElement CountryDropdown => _driver.FindElement(By.Name("country"));
        private IWebElement TitleDropdown => _driver.FindElement(By.Name("title"));
        private IWebElement DegreeInput => _driver.FindElement(By.Name("degree"));
        private IWebElement GraduationYearDropdown => _driver.FindElement(By.Name("yearOfGraduation"));
        private IWebElement AddButton => _driver.FindElement(By.XPath("//input[@value='Add']"));
        private IWebElement UpdateButton => _driver.FindElement(By.XPath("//input[@value='Update']"));

        public void NavigateToEducationTab() => EducationTab.Click();


        public void ClickAddNewButton()
        {
            try
            {
                var addNewButton = _wait.Until(d =>
                    d.FindElement(By.XPath("//div[text()='Add New' and contains(@class, 'button')]")));
                addNewButton.Click();
                _wait.Until(d => UniversityInput.Displayed);
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

        public void AddEducation(EducationModel data, bool isCleanupNeeded = false)
        {

            var UniversityInput = _driver.FindElement(By.XPath("//input[@placeholder='College/University Name']"));
            UniversityInput.Clear();
            UniversityInput.SendKeys(data.University!);

            var CountryDropdown = _driver.FindElement(By.Name("country"));
            var options = CountryDropdown.FindElements(By.TagName("option"));

            foreach(var option in options)
            {
                if(option.Text.Trim().Equals(data.Country, StringComparison.OrdinalIgnoreCase))
                {
                    option.Click();
                    break;
                }
            }

            var TitleDropdown = _driver.FindElement(By.Name("title"));
            var titleOptions = TitleDropdown.FindElements(By.TagName("option"));
            foreach (var option in titleOptions)
            {
                if (option.Text.Trim().Equals(data.Title, StringComparison.OrdinalIgnoreCase))
                {
                    option.Click();
                    break;
                }
            }

            var DegreeInput = _driver.FindElement(By.XPath("//input[@placeholder='Degree']"));
            DegreeInput.Clear();
            DegreeInput.SendKeys(data.Degree!);


            var GraduationYearDropdown = _driver.FindElement(By.Name("yearOfGraduation"));
            var yearOptions = GraduationYearDropdown.FindElements(By.TagName("option"));
            foreach (var option in yearOptions)
            {
                if (option.Text.Trim().Equals(data.GraduationYear, StringComparison.OrdinalIgnoreCase))
                {
                    option.Click();
                    break;
                }
            }
            if (isCleanupNeeded == true)
            {
                Hooks.Hooks.AddToCleanupList(data);
            }
        }

        public void ClickAddNew()
        {
            try
            {
                var addNewButton = _wait.Until(d =>
                    d.FindElement(By.XPath("//div[@data-tab='third']//div[text()='Add New' and contains(@class, 'button')]")));
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

        public bool IsAddEducationFormVisible()
        {
            try
            {
                var UniversityInput = _driver.FindElement(By.XPath("//input[@placeholder='College/University Name']"));
                var CountryDropdown = _driver.FindElement(By.Name("country"));
                var TitleDropdown = _driver.FindElement(By.Name("title"));
                var DegreeInput = _driver.FindElement(By.XPath("//input[@placeholder='Degree']"));
                var GraduationYearDropdown = _driver.FindElement(By.Name("yearOfGraduation"));

                return UniversityInput.Displayed && CountryDropdown.Displayed &&
                       TitleDropdown.Displayed && DegreeInput.Displayed && GraduationYearDropdown.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsEducationAdded(string university, string degree)
        {
            try
            {
                // Wait until a row with matching university and degree is visible
                var row = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                    By.XPath($"//table[@class='ui fixed table']//tr[td[contains(text(), '{university}')] and td[contains(text(), '{degree}')]]")));

                return row != null;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"Education record '{university} - {degree}' not found within timeout.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while verifying education record: {ex.Message}");
                return false;
            }
        }

        public bool IsEducationListed(EducationModel data)
        {
            try
            {
                // Wait for either the record to appear or timeout
                var rows = _wait.Until(driver =>
                    driver.FindElements(By.XPath(
                        $"//table[@class='ui fixed table']//tr[td[text()='{data.Country}'] and " +
                        $"td[text()='{data.University}'] and " +
                        $"td[text()='{data.Title}'] and " +
                        $"td[text()='{data.Degree}'] and " +
                        $"td[text()='{data.GraduationYear}']]"))
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
                Console.WriteLine($"Error checking if education record is listed: {ex.Message}");
                return false;
            }
        }

        public void EditEducation(EducationModel oldData, EducationModel newData)
        {
            try
            {
                // Wait for the correct row to be present before clicking edit
                var editIcon = _wait.Until(driver =>
                    driver.FindElement(By.XPath(
                        $"//table[@class='ui fixed table']//tr[td[text()='{oldData.Country}'] and " +
                        $"td[text()='{oldData.University}'] and " +
                        $"td[text()='{oldData.Title}'] and " +
                        $"td[text()='{oldData.Degree}'] and " +
                        $"td[text()='{oldData.GraduationYear}']]//i[contains(@class,'write')]")));

                editIcon.Click();

                if (IsAddEducationFormVisible())
                {
                    AddEducation(newData, isCleanupNeeded: true);
                    ClickUpdate();
                }  
            }
            catch (NoSuchElementException)
            {
                throw new Exception($"Edit icon not found for record: {oldData.Country}, {oldData.University}, {oldData.Title}, {oldData.Degree}, {oldData.GraduationYear}");
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception("Timed out waiting for the education edit form to appear.");
            }
        }

        public void DeleteEducation(EducationModel data)
        {
            try
            {
                var deleteIcon = _driver.FindElement(By.XPath(
                    $"//table[@class='ui fixed table']//tr[td[text()='{data.Country}'] and " +
                    $"td[text()='{data.University}'] and " +
                    $"td[text()='{data.Title}'] and " +
                    $"td[text()='{data.Degree}'] and " +
                    $"td[text()='{data.GraduationYear}']]//i[contains(@class,'remove')]"));

                deleteIcon.Click();

                // Wait until the specific row is no longer present
                _wait.Until(driver =>
                    driver.FindElements(By.XPath(
                        $"//table[@class='ui fixed table']//tr[td[text()='{data.Country}'] and " +
                        $"td[text()='{data.University}'] and " +
                        $"td[text()='{data.Title}'] and " +
                        $"td[text()='{data.Degree}'] and " +
                        $"td[text()='{data.GraduationYear}']]")).Count == 0);
            }
            catch (NoSuchElementException)
            {
                throw new Exception($"Delete icon not found for record: {data.Country}, {data.University}, {data.Title}, {data.Degree}, {data.GraduationYear}");
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception($"Record was not removed from the table after clicking delete: {data.University} - {data.Degree}");
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
