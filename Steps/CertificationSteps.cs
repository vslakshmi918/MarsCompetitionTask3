using MarsCompetitionTask3.Drivers;
using MarsCompetitionTask3.Models;
using MarsCompetitionTask3.Pages;
using MarsCompetitionTask3.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;

namespace MarsCompetitionTask3.Steps
{
    [Binding, Scope(Feature = "Manage Certification")]
    public class CertificationSteps
    {
        private readonly IWebDriver _driver;
        private readonly CertificationPage _certificationPage;
        private readonly LoginHelper _loginHelper;
        private readonly List<CertificationModel> _certificationData;

        public CertificationSteps()
        {
            _driver = WebDriverMngr.Instance.GetDriver();
            _certificationPage = new CertificationPage(_driver);
            _loginHelper = new LoginHelper(_driver);

            string currentDir = Environment.CurrentDirectory;
            // Move up three levels: bin → Debug → net8.0
            string projectRoot = Directory.GetParent(currentDir).Parent.Parent.FullName;
            // Append the TestData folder
            string testDataFile = Path.Combine(projectRoot, "TestData", "CertificationData.json");



            _certificationData = JsonHelper.ReadJsonList<CertificationModel>(testDataFile);

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

        [Given(@"I navigate to the certification tab")]
        public void GivenINavigateToCertificationTab()
        {
            _loginHelper.NavigateToCertificationTab();
        }
        #endregion

        #region Add Certification
        [When(@"I click on the Add New button")]
        public void WhenIClickOnAddNewButton() => _certificationPage.ClickAddNew(); // ensure active tab

        [When(@"I add a new certification record from JSON index (.*)")]
        public void WhenIAddCertificationRecord(int index)
        {
            _certificationPage.AddCertification(_certificationData[index], isCleanupNeeded: true);
        }

        [When(@"I click on the Add button")]
        public void WhenIClickAddButton() => _certificationPage.ClickAdd();

        [Then(@"I should see the certification record from JSON index (.*) in my profile")]
        public void ThenIShouldSeeCertificationRecord(int index)
        {
            var data = _certificationData[index];
            Assert.IsTrue(_certificationPage.IsCertificationAdded(data.Certificate!, data.From!),
                $"Record '{data.Certificate} - {data.From}' not visible!");
        }
        #endregion

        #region Edit Multiple
        [Given(@"I have added certification record for edit from JSON index (.*)")]
        public void GivenIHaveAddedCertificationRecordForEdit(int index)
        {
            _certificationPage.ClickAddNew();
            _certificationPage.AddCertification(_certificationData[index], isCleanupNeeded: false);
            _certificationPage.ClickAdd();
        }

        [When(@"I edit the certification record from JSON index (.*) to updated JSON index (.*)")]
        public void WhenIEditCertificationRecord(int oldIndex, int newIndex)
        {
            _certificationPage.EditCertification(_certificationData[oldIndex], _certificationData[newIndex]);
        }

        [Then(@"I should see the updated certification record from JSON index (.*) in my profile")]
        public void ThenIShouldSeeUpdatedCertification(int index)
        {
            var data = _certificationData[index];
            Assert.IsTrue(_certificationPage.IsCertificationAdded(data.Certificate!, data.From!));
        }
        #endregion

        #region Delete certification 
        [Given(@"I have added certification record for delete from JSON index (.*)")]
        public void GivenIHaveAddedCertificationRecordForDelete(int index)
        {
            _certificationPage.ClickAddNew();
            _certificationPage.AddCertification(_certificationData[index], isCleanupNeeded: false);
            _certificationPage.ClickAdd();
        }
        [When(@"I delete the certification record from JSON index (.*)")]
        public void WhenIDeleteCertificationRecord(int index) => _certificationPage.DeleteCertification(_certificationData[index]);

        [Then(@"the certification record from JSON index (.*) should not be listed in my certifications")]
        public void ThenCertificationShouldNotBeListed(int index)
        {
            var data = _certificationData[index];
            Assert.IsFalse(_certificationPage.IsCertificationListed(data), $"Record '{data.Certificate} - {data.From}' should be deleted but is still listed.");
        }
        #endregion

        #region duplicate and Invalid data
        [Given(@"I have added certification record for duplication from JSON index (.*)")]
        public void GivenIHaveAddedcertificationRecordForDuplication(int index)
        {
            _certificationPage.ClickAddNew();
            _certificationPage.AddCertification(_certificationData[index], isCleanupNeeded: true);
            _certificationPage.ClickAdd();
        }

        [When(@"I add a new certification record with duplicate from JSON index (.*)")]
        public void WhenIAddcertificationRecordWithDuplicate(int index)
        {
            try
            {
                _certificationPage.AddCertification(_certificationData[index], isCleanupNeeded: false);

            }
            catch
            {

            }
        }

        [Then(@"I should see an error message indicating duplicate certification not allowed")]
        public void ThenDuplicateError() =>
            _certificationPage.IsErrorMessageDisplayed("This certification record already exists.");

        [When(@"I add a new certification record with invalid data from JSON index (.*)")]
        public void WhenIAddCertificationRecordWithInvalidData(int index)
        {
            var data = _certificationData[index];
            var incompleteData = new CertificationModel
            {
                Certificate = data.Certificate,
                From = data.From,
                Year = data.Year
            };
            _certificationPage.AddCertification(incompleteData, isCleanupNeeded: false);
        }

        [Then(@"I should see an error message indicating field is required")]
        public void ThenFieldRequiredError() =>
            _certificationPage.IsErrorMessageDisplayed("Field is required.");
        #endregion
    }
}
