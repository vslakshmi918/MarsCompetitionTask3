# 🪐 Mars QA Competition Task 2025 — Automation Framework

![C#](https://img.shields.io/badge/language-C%23-178600?style=flat-square)
![.NET 8](https://img.shields.io/badge/.NET-8.0-blue?style=flat-square)
![Reqnroll](https://img.shields.io/badge/Framework-Reqnroll%20(BDD)-green?style=flat-square)
![Selenium](https://img.shields.io/badge/Automation-Selenium%20WebDriver-orange?style=flat-square)
![ExtentReports](https://img.shields.io/badge/Reporting-ExtentReports-purple?style=flat-square)
![License](https://img.shields.io/badge/license-MIT-lightgrey?style=flat-square)

---

## 🧠 Overview

This project is part of the **IndustryConnect Mars QA Competition 2025**, built to demonstrate modern **BDD test automation** with **.NET 8**, **Reqnroll (Cucumber for .NET)**, **Selenium WebDriver**, and **ExtentReports**.  

It automates the **Education** and **Certification** modules on the [Mars SkillSwap website](https://skillswap.marsindustryconnect.io), validating Add, Edit, Delete, and Negative test scenarios using **JSON-driven data**.  

---

## ⚙️ Tech Stack

| Category | Tools / Frameworks |
|-----------|--------------------|
| **Language** | C# (.NET 8) |
| **BDD Framework** | Reqnroll (Cucumber for .NET) |
| **Automation Library** | Selenium WebDriver |
| **Reporting** | ExtentReports (HTML Reports with Screenshots) |
| **Test Data Source** | JSON |
| **Design Pattern** | Page Object Model (POM) |
| **Build & Run** | `dotnet CLI` |
| **IDE** | Visual Studio 2022 |
| **Version Control** | Git & GitHub |

---

## 📦 Project Modules

| Module | Description |
|--------|--------------|
| 🧾 **Education** | Automates Add, Edit, Delete, and validation of education records. |
| 🎓 **Certification** | Automates Add, Edit, Delete, and validation of certifications. |
| 🧰 **Hooks** | Initializes and finalizes ExtentReports, browser, and data cleanup. |
| ⚙️ **Utilities** | Handles config reading, JSON parsing, screenshots, and login. |

---

## 🧩 Key Features

✅ **Reqnroll (BDD)** — Gherkin syntax for readable scenarios  
✅ **JSON-driven test data** — Fully data-driven  
✅ **Selenium WebDriver** — Stable browser automation  
✅ **ExtentReports integration** — Visual HTML reports + screenshots  
✅ **Automatic cleanup** — Deletes records after each test  
✅ **Configurable setup** — Environment & browser via `appsettings.json`  
✅ **Page Object Model (POM)** — Clean and maintainable structure  
✅ **Parallel-ready** — Can scale in CI/CD  
✅ **Comprehensive manual + automated coverage**  

---

## 🧱 Folder Structure

```
MarsCompetitionTask/
│
├── docs/
├── Drivers/
├── Features/
├── Hooks/
├── Models/
├── Pages/
├── Reports/
├── Steps/
├── TestData/
└── Utilities/
└── README.md/

```

---

## 🧮 How to Run

### ▶️ From Visual Studio
1. Open `MarsCompetitionTask.sln`
2. Select **Test → Run All Tests**
3. View reports under `/src/MarsQA/Reports/`

### ▶️ From Terminal
```bash
dotnet restore
dotnet build
dotnet test
```

---

## 🧪 Example Feature (Education)

```gherkin
Scenario Outline: Add multiple educations using JSON data
  When I click on the Add New button
  And I add a new education record from JSON index <index>
  And I click on the Add button
  Then I should see the education record from JSON index <index> in my profile
  Examples:
    | index |
    | 0 |
    | 1 |
```

---

## 📘 Data-Driven JSON Example

```json
[
  {
    "Country": "India",
    "University": "JNTU Hyderabad",
    "Title": "B.Tech",
    "Degree": "Computer Science",
    "GraduationYear": "2019"
  },
  {
    "Country": "India",
    "University": "IIIT Hyderabad",
    "Title": "M.Tech",
    "Degree": "Data Science",
    "GraduationYear": "2022"
  }
]
```

---

## 📊 ExtentReports (HTML Reporting)

Reports are generated automatically in:
```
src/MarsQA/Reports/ExtentReport_<timestamp>.html
```

Each report includes:
- Scenario-wise pass/fail status  
- Logs and step details  
- Screenshots for failures  
- Auto cleanup logs  

Example log:
```
Scenario: Add Education record
Status: PASSED
Info: Cleanup: Deleted record 'JNTU Hyderabad - Computer Science'
```

---

## 🧰 Utilities Used

| Utility | Purpose |
|----------|----------|
| `ConfigHelper.cs` | Reads config from `appsettings.json` |
| `JsonHelper.cs` | Reads test data from JSON files |
| `ScreenshotHelper.cs` | Captures screenshots on failure |
| `LoginHelper.cs` | Reusable login logic for all tests |
| `Hooks.cs` | Initializes ExtentReports, manages browser, and performs cleanup |

---

## 🧾 Manual Test Cases & Test Plan

Located under:
```
docs/TestCases.xlsx
docs/TestPlan.md
```

Includes:
- Positive, Negative, and Destructive test cases  
- Acceptance criteria and scope  
- Manual test results for User Story 1  

---

## 🧱 Dependencies

Install required packages:
```bash
Install-Package Reqnroll
Install-Package Selenium.WebDriver
Install-Package Selenium.Support
Install-Package AventStack.ExtentReports
Install-Package Newtonsoft.Json
Install-Package Microsoft.Extensions.Configuration.Json
```

---

## 👩‍💻 Author

**👤 Name:** *Subha Vangalapudi*  
📧 **Email:** *your.email@example.com*  
🔗 **LinkedIn:** [https://www.linkedin.com/in/yourprofile](#)  
🏆 **Program:** *IndustryConnect Mars QA Competition 2025*  

---

⭐ *If you found this project helpful, please star the repository!*  
