# Black Box Testing Analysis: Password Validator

## Specifications

The password validator enforces the following requirements:

- **Length**: Between 8 and 50 characters
- **Uppercase**: At least one uppercase letter
- **Number**: At least one number
- **Special Character**: At least one special character

## Equivalence Partitioning (Password length)

| Partition ID | Partitions   | Type    | Description                  | Test case values |
|--------------|--------------|---------|------------------------------|------------------|
| P1           | 0 - 7        | Invalid | Password length between 0-7  | 3                |
| P2           | 8 - 50       | Valid   | Password length between 8-50 | 21               |
| P3           | 51 - MAX INT | Invalid | Password length more than 50 | 60               |

## Boundary Value Analysis (Password length)

| Condition      | Boundary Values |
|----------------|-----------------|
| Minimum length | 7, 8, 9         |
| Maximum length | 49, 50, 51      |

## Decision Table

| Test Case | Length 8-50 | Has Uppercase | Has Number | Has Special Char | Expected Result |
|-----------|-------------|---------------|------------|------------------|-----------------|
| TC1       | ✓           | ✓             | ✓          | ✓                | Valid           |
| TC2       | ✗ (< 8)     | ✓             | ✓          | ✓                | Invalid         |
| TC3       | ✗ (> 50)    | ✓             | ✓          | ✓                | Invalid         |
| TC4       | ✓           | ✗             | ✓          | ✓                | Invalid         |
| TC5       | ✓           | ✓             | ✗          | ✓                | Invalid         |
| TC6       | ✓           | ✓             | ✓          | ✗                | Invalid         |
| TC7       | ✓           | ✗             | ✗          | ✗                | Invalid         |
| TC8       | ✗ (< 8)     | ✗             | ✗          | ✗                | Invalid         |

## Test Cases

| ID   | Description                           | Input                                                          | Expected Value |
|------|---------------------------------------|----------------------------------------------------------------|----------------|
| TC1  | Minimum-length valid mix              | `Abcdef1!`                                                     | Valid          |
| TC2  | Mid-length valid mix                  | `Val1dPassword!Sample1`                                        | Valid          |
| TC3  | Minimum +1 boundary valid             | `Abcdefg2!`                                                    | Valid          |
| TC4  | Maximum -1 boundary valid             | `Password1!Password1!Password1!Password1!Secure1!X`            | Valid          |
| TC5  | Maximum length valid                  | `Password1!Password1!Password1!Password1!Password1!`           | Valid          |
| TC6  | Multiple specials still valid         | `Abcdef1!!`                                                    | Valid          |
| TC7  | Internal space allowed                | `Abcd ef1!`                                                    | Valid          |
| TC8  | Unicode uppercase counted             | `ÆbleGrød1!`                                                   | Valid          |
| TC9  | Uppercase-only                        | `ABCD123!`                                                     | Invalid        |
| TC10 | Minimum boundary violation (7 chars)  | `Abc1!xY`                                                      | Invalid        |
| TC11 | Deep short-length violation (3 chars) | `Ab!`                                                          | Invalid        |
| TC12 | Just over max length (51 chars)       | `Password1!Password1!Password1!Password1!Password1!P`          | Invalid        |
| TC13 | Far over max length (60 chars)        | `Password1!Password1!Password1!Password1!Password1!Password1!` | Invalid        |
| TC14 | No uppercase                          | `password1!`                                                   | Invalid        |
| TC15 | No number                             | `Password!`                                                    | Invalid        |
| TC16 | No special character                  | `Password1`                                                    | Invalid        |
| TC17 | No uppercase, number, or special      | `password`                                                     | Invalid        |
| TC18 | Missing uppercase and special         | `password1`                                                    | Invalid        |
| TC19 | Missing number and special            | `Password`                                                     | Invalid        |
| TC20 | Leading space                         | `Abcdef1!`                                                     | Invalid        |
| TC21 | Trailing space                        | `Abcdef1!`                                                     | Invalid        |
| TC22 | Empty string                          | `""`                                                           | Invalid        |
| TC23 | Null input                            | `null`                                                         | Invalid        |
