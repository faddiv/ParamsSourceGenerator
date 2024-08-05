# SuccessfulParamsCandidate Creation

## Original

| Method                          | Mean     | Error     | StdDev    | Gen0   | Allocated |
|-------------------------------- |---------:|----------:|----------:|-------:|----------:|
| CreateSuccessfulParamsCandidate | 5.116 us | 0.0827 us | 0.0773 us | 0.4044 |   1.67 KB |
| CreateMethodInfo                | 2.393 us | 0.0182 us | 0.0161 us | 0.3128 |   1.29 KB |

## Merge TypeArguments and TypeConstraints

This change instead of two separate Property uses one. Also the constraint list wasn't created, instead info about creation is stored.

| Method                          | Mean     | Error     | StdDev    | Gen0   | Allocated |
|-------------------------------- |---------:|----------:|----------:|-------:|----------:|
| CreateSuccessfulParamsCandidate | 4.842 us | 0.0507 us | 0.0449 us | 0.2823 |    1200 B |
| CreateMethodInfo                | 1.979 us | 0.0134 us | 0.0126 us | 0.1907 |     808 B |
