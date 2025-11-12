using Io.Cucumber.Messages.Types;
using MarsCompetitionTask.Utilities;
using MarsCompetitionTask3.Drivers;
using MarsCompetitionTask3.Hooks;
using MarsCompetitionTask3.Models;
using MarsCompetitionTask3.Pages;
using MarsCompetitionTask3.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsCompetitionTask.Steps
{
    [Binding, Scope(Feature = "Manage Education")]
    public class EducationSteps
    {
        private readonly IWebDriver _driver;
        private readonly EducationPage _educationPage;
        private readonly LoginHelper _loginHelper;
        private readonly List<EducationModel> _educationData;

        public EducationSteps()
        {
            _driver = WebDriverMngr.Instance.GetDriver();
            _educationPage = new EducationPage(_driver);
            _loginHelper = new LoginHelper(_driver);

            string currentDir = Environment.CurrentDirectory;
            // Move up three levels: bin → Debug → net8.0
            string projectRoot = Directory.GetParent(currentDir).Parent.Parent.FullName;
            // Append the TestData folder
            string testDataFile = Path.Combine(projectRoot, "TestData", "EducationData.json");



            _educationData = JsonHelper.ReadJsonList<EducationModel>(testDataFile);

        }

        #region Login steps
        [Given(@"I navigate to the login page")]
        public void GivenINavigateToTheLoginPage() =>
            _loginHelper.GoToLoginPage("http://localhost:5003");

        [Given(@"I login with email ""(.*)"" and password ""(.*)""")]
        public void GivenILogin(string email, string password) =>
            _loginHelper.Login(email, password);

        [Given(@"I am on the Home page")]
        public void GivenIAmOnHomePage()
        {
            new WebDriverWait(_driver, TimeSpan.FromSeconds(5)).Until(d => d.Url.Contains("/Account/Profile"));
            Assert.That(_driver.Url, Does.Contain("/Account/Profile"), "Not on Home page!");
        }

        [Given(@"I navigate to the Education tab")]
        public void GivenINavigateToEducationTab()
        {
            _loginHelper.NavigateToEducationTab();
        }
        #endregion

        #region Add Education
        [When(@"I click on the Add New button")]
        public void WhenIClickOnAddNewButton() => _educationPage.ClickAddNew(); // ensure active tab

        [When(@"I add a new education record from JSON index (.*)")]
        public void WhenIAddEducationRecord(int index)
        {
            _educationPage.AddEducation(_educationData[index], isCleanupNeeded: true);
        } 

        [When(@"I click on the Add button")]
        public void WhenIClickAddButton() => _educationPage.ClickAdd();

        [Then(@"I should see the education record from JSON index (.*) in my profile")]
        public void ThenIShouldSeeEducationRecord(int index)
        {
            var data = _educationData[index];
            Assert.IsTrue(_educationPage.IsEducationAdded(data.University!, data.Degree!),
                $"Record '{data.University} - {data.Degree}' not visible!");
        }
        #endregion

        #region Edit Multiple
        [Given(@"I have added education record for edit from JSON index (.*)")]
        public void GivenIHaveAddedEducationRecordForEdit(int index)
        {
            _educationPage.ClickAddNew();
            _educationPage.AddEducation(_educationData[index], isCleanupNeeded : false);
            _educationPage.ClickAdd();
        }

        [When(@"I edit the education record from JSON index (.*) to updated JSON index (.*)")]
        public void WhenIEditEducationRecord(int oldIndex, int newIndex) 
        {
            _educationPage.EditEducation(_educationData[oldIndex], _educationData[newIndex]);
        }

        [Then(@"I should see the updated education record from JSON index (.*) in my profile")]
        public void ThenIShouldSeeUpdatedEducation(int index)
        {
            var data = _educationData[index];
            Assert.IsTrue(_educationPage.IsEducationAdded(data.University!, data.Degree!));
        }
        #endregion

        #region Delete Education 
        [Given(@"I have added education record for delete from JSON index (.*)")]
        public void GivenIHaveAddedEducationRecordForDelete(int index)
        {
            _educationPage.ClickAddNew();
            _educationPage.AddEducation(_educationData[index], isCleanupNeeded: false);
            _educationPage.ClickAdd();
        }
        [When(@"I delete the education record from JSON index (.*)")]
        public void WhenIDeleteEducationRecord(int index) => _educationPage.DeleteEducation(_educationData[index]);

        [Then(@"the education record from JSON index (.*) should not be listed in my educations")]
        public void ThenEducationShouldNotBeListed(int index)
        {
            var data = _educationData[index];
            Assert.IsFalse(_educationPage.IsEducationListed(data), $"Record '{data.University} - {data.Degree}' should be deleted but is still listed.");
        }
        #endregion

        #region duplicate and Invalid data
        [Given(@"I have added education record for duplication from JSON index (.*)")]
        public void GivenIHaveAddedEducationRecordForDuplication(int index)
        {
            _educationPage.ClickAddNew();
            _educationPage.AddEducation(_educationData[index], isCleanupNeeded: true);
            _educationPage.ClickAdd();
        }

        [When(@"I add a new education record with duplicate from JSON index (.*)")]
        public void WhenIAddEducationRecordWithDuplicate(int index)
        {
            _educationPage.AddEducation(_educationData[index], isCleanupNeeded: false);
        }

        [Then(@"I should see an error message indicating duplicate education not allowed")]
        public void ThenDuplicateError() =>
            _educationPage.IsErrorMessageDisplayed("This education record already exists.");

        [When(@"I add a new education record without degree from JSON index (.*)")]
        public void WhenIAddEducationRecordWithoutDegree(int index)
        {
            var data = _educationData[index];
            var incompleteData = new EducationModel
            {
                Country = data.Country,
                University = data.University,
                Title = data.Title,
                Degree = "", // Missing degree
                GraduationYear = data.GraduationYear
            };
            _educationPage.AddEducation(incompleteData, isCleanupNeeded: false);
        }

        [Then(@"I should see an error message indicating degree field is required")]
        public void ThenDegreeRequiredError() =>
            _educationPage.IsErrorMessageDisplayed("Degree is required.");
        #endregion
    }
}
