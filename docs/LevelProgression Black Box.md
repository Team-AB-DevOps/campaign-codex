# Black Box Testing Analysis: Character Progression

## Specifications

```c#
private static readonly int[] XpThresholds =
[
    0, // Level 1
    300, // Level 2
    900, // Level 3
    2700, // Level 4
    6500, // Level 5
    14000, // Level 6
    23000, // Level 7
    34000, // Level 8
    48000, // Level 9
    64000, // Level 10
];
```

## Total XP Test Analysis

### Equivalence Partitioning

| Partition ID | Partitions            | Type    | Description                          | Test case values |
| ------------ | --------------------- | ------- | ------------------------------------ | ---------------- |
| VP1          | 0 - 64000             | Valid   | Between lvl 1 and 10                 | 32000            |
| VP2          | NEGATIVE MAX INT - -1 | Invalid | Can not be under lvl 1               | -1000            |
| VP3          | 64001 - MAX INT       | Valid   | Can not be over lvl 10, but no error | 100000           |

### 3-value Boundary Value Analysis

| Partition ID | Partition types | Partitions            | Boundary values | Test case values         |
| ------------ | --------------- | --------------------- | --------------- | ------------------------ |
| VP1          | Valid           | 0 - 64000             | 0 64000         | -1 0 1 63999 64000 64001 |
| VP2          | Invalid         | NEGATIVE MAX INT - -1 | -1              | -2 -1 0                  |
| VP3          | Valid           | 64001 - MAX INT       | 64001           | 64000 64001 64002        |

### Final Test case values

`32000`, `-1000`, `100000`, `-1`, `0`, `1`, `63999`, `64000`, `64001`, `-2`, `-1`, `0`, `64000`, `64001`, `64002`

