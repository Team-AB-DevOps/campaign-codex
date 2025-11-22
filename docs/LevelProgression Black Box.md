# Black Box Testing Analysis: Character Progression

## Specifications

A character starts at level 1 and is able to level up their character up to level 10 by gaining experience points. Once a threshold is passed, the character advances to the next level. The system uses predefined XP thresholds to determine the character's current level based on their total accumulated experience points. The XP thresholds are defined as:

| Level | Experience Points |
| ----- | ----------------- |
| 1     | 0                 |
| 2     | 300               |
| 3     | 900               |
| 4     | 2.700             |
| 5     | 6.500             |
| 6     | 14.000            |
| 7     | 23.000            |
| 8     | 34.000            |
| 9     | 48.000            |
| 10    | 64.000            |

## Total XP Test Analysis

### Equivalence Partitioning

| Partition ID | Partitions            | Type    | Description                          | Test case values |
| ------------ | --------------------- | ------- | ------------------------------------ | ---------------- |
| P1           | 0 - 64000             | Valid   | Between lvl 1 and 10                 | 32000            |
| P2           | NEGATIVE MAX INT - -1 | Invalid | Can not be under lvl 1               | -1000            |
| P3           | 64001 - MAX INT       | Valid   | Can not be over lvl 10, but no error | 100000           |

### 3-value Boundary Value Analysis

| Partition ID | Partition types | Partitions            | Boundary values | Test case values         |
| ------------ | --------------- | --------------------- | --------------- | ------------------------ |
| P1           | Valid           | 0 - 64000             | 0 64000         | -1 0 1 63999 64000 64001 |
| P2           | Invalid         | NEGATIVE MAX INT - -1 | -1              | -2 -1 0                  |
| P3           | Valid           | 64001 - MAX INT       | 64001           | 64000 64001 64002        |

### Final Test case values

`32000`, `-1000`, `100000`, `-1`, `0`, `1`, `63999`, `64000`, `64001`, `-2`, `-1`, `0`, `64000`, `64001`, `64002`

## Different Levels Test Analysis

### Equivalence Partitioning

| Partition ID | Partitions      | Type  | Description | Test case values |
| ------------ | --------------- | ----- | ----------- | ---------------- |
| P1           | 0 - 299         | Valid | Level 1     | 150              |
| P2           | 300 - 899       | Valid | Level 2     | 600              |
| P3           | 900 - 2699      | Valid | Level 3     | 1700             |
| P4           | 2700 - 6499     | Valid | Level 4     | 4500             |
| P5           | 6500 - 13999    | Valid | Level 5     | 9000             |
| P6           | 14000 - 22999   | Valid | Level 6     | 17000            |
| P7           | 23000 - 33999   | Valid | Level 7     | 28000            |
| P8           | 34000 - 47999   | Valid | Level 8     | 41000            |
| P9           | 48000 - 63999   | Valid | Level 9     | 55000            |
| P10          | 64000 - MAX INT | Valid | Level 10    | 72000            |

### 3-value Boundary Value Analysis

| Partition ID | Partitions types | Partitions      | Boundary values | Test case values                    |
| ------------ | ---------------- | --------------- | --------------- | ----------------------------------- |
| P1           | Valid            | 0 - 299         | 0 299           | -1 0 1 298 299 300                  |
| P2           | Valid            | 300 - 899       | 300 899         | 299 300 301 898 899 900             |
| P3           | Valid            | 900 - 2699      | 900 2699        | 899 900 901 2698 2699 2700          |
| P4           | Valid            | 2700 - 6499     | 2700 6499       | 2699 2700 2701 6498 6499 6500       |
| P5           | Valid            | 6500 - 13999    | 6500 13999      | 6499 6500 6501 13998 13999 14000    |
| P6           | Valid            | 14000 - 22999   | 14000 22999     | 13999 14000 14001 22998 22999 23000 |
| P7           | Valid            | 23000 - 33999   | 23000 33999     | 22999 23000 23001 33998 33999 34000 |
| P8           | Valid            | 34000 - 47999   | 34000 47999     | 33999 34000 34001 47998 47999 48000 |
| P9           | Valid            | 48000 - 63999   | 48000 63999     | 47999 48000 48001 63998 63999 64000 |
| P10          | Valid            | 64000 - MAX INT | 64000           | 63999 64000 64001                   |

### Final Test case values

`0`, `1`, `150`, `298`, `299`, `300`, `301`, `600`, `898`, `899`, `900`, `901`, `1700`, `2698`, `2699`, `2700`, `2701`, `4500`, `6498`, `6499`, `6500`, `6501`, `9000`, `13998`, `13999`, `14000`, `14001`, `17000`, `22998`, `22999`, `23000`, `23001`, `28000`, `33998`, `33999`, `34000`, `34001`, `41000`, `47998`, `47999`, `48000`, `48001`, `55000`, `63998`, `63999`, `64000`, `64001`, `72000`
