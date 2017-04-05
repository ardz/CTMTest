Feature: EnergyJourney

# Example of simple smoke test?
@EnergySmoke
Scenario: Check results returned after post code entered
Given I am on the Energy Journey start page
When I search for the post code PE2 6YS
Then I should I see options for my current energy supplier

# Example of using a table in a scenario for a data driven test case
@EnergyRegression
Scenario: User has no bill, same gas and electric supplier on standard tariff, pays by dd and wishes to switch to another dd fixed tariff
Background: 
Given These user profiles where user has no bill and same provider for gas and electric:
| Profile | CurrentSupplier | CurrentElectricitySpend | CurrentGasSpend | Period      |
| 1       | British Gas     | 80                      | 80              | Monthly     |
| 2       | EDF Energy      | 2500                    | 2500            | Quarterly   |
| 3       | E.ON            | 450                     | 450             | Six Monthly |
| 4       | NPower          | 8000                    | 8000            | Annually    |
| 5       | Scottish Power  | 40                      | 40              | Monthly     |
| 6       | SSE             | 200                     | 200             | Monthly     |

Then The minimum expected saving should be:
| Profile | MinimumSaving |
| 1       | 200           |
| 2       | 3500          |
| 3       | 300           |
| 4       | 5000          |
| 5       | 50            |
| 6       | 75            |