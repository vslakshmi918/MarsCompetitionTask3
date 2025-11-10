Feature: Manage Education
  Each test is independent and handles its own data setup

  Background:
    Given I navigate to the login page
    And I login with email "subhavangalapudi@gmail.com" and password "Subha1982"
    And I am on the Home page
    And I navigate to the Education tab

  @Add
  @Positive
  Scenario Outline: Add a single Education record
    When I click on the Add New button
    And I add a new education record from JSON index 0
    And I click on the Add button
    Then I should see the education record from JSON index 0 in my profile

  @Add
  @Positive
  Scenario Outline: Add multiple educations using JSON data
    When I click on the Add New button
    And I add a new education record from JSON index <index>
    And I click on the Add button
    Then I should see the education record from JSON index <index> in my profile
    Examples:
      | index |
      | 0 |
      | 1 |
      | 2 |
      | 3 |

  @Edit
  @Positive
  Scenario Outline: Edit multiple educations using JSON data
    # Setup: Add the education first
    Given I have added education record for edit from JSON index <index>
    # Step 2: Edit the record using updated JSON data
    When I edit the education record from JSON index <index> to updated JSON index <updateIndex>
    Then I should see the updated education record from JSON index <updateIndex> in my profile
    Examples:
      | index | updateIndex |
      | 0 | 2 |
      | 1 | 3 |

  @Delete
  @Positive
  Scenario Outline: Delete multiple educations using JSON data
    # Setup: Add the education first
    Given I have added education record for delete from JSON index <index>
    # Step 2: Delete the record using updated JSON data
    When I delete the education record from JSON index <index>
    Then the education record from JSON index <index> should not be listed in my educations
    Examples:
      | index |
      | 0 |
      | 1 |
      | 2 |
      | 3 |

  @Duplicate
  @Negative
  Scenario: Add duplicate education
    # Setup: Add the education first
    Given I have added education record for duplication from JSON index 0
    # Test: Try to add duplicate
    When I click on the Add New button
    And I add a new education record with duplicate from JSON index 0
    And I click on the Add button
    Then I should see an error message indicating duplicate education not allowed

  @Invalid
  @Negative
  Scenario: Add education invalid data
    When I click on the Add New button
    And I add a new education record without degree from JSON index 0
    And I click on the Add button
    Then I should see an error message indicating degree field is required
