SyntaxFactory:

| Method       | Mean     | Error    | StdDev   | Gen0     | Gen1     | Allocated |
|------------- |---------:|---------:|---------:|---------:|---------:|----------:|
| RunGenerator | 16.07 ms | 0.320 ms | 0 .479 ms | 500.0000 | 166.6667 |   3.62 MB |

StringBuilder:

| Method       | Mean     | Error   | StdDev  | Gen0    | Gen1   | Allocated |
|------------- |---------:|--------:|--------:|--------:|-------:|----------:|
| RunGenerator | 594.6 us | 6.08 us | 8.12 us | 46.8750 | 7.8125 | 191.77 KB |

StringBuilder v2:
| Method       | Mean     | Error   | StdDev  | Gen0    | Gen1    | Allocated |
|------------- |---------:|--------:|--------:|--------:|--------:|----------:|
| RunGenerator | 564.7 us | 6.02 us | 6.69 us | 42.9688 | 11.7188 |  190.9 KB |

Testing if separating file grouping during pipeline make optimizations.
Calculating all output together:
| Method             | Mean     | Error   | StdDev  | Gen0       | Gen1      | Allocated |
|------------------- |---------:|--------:|--------:|-----------:|----------:|----------:|
| OnlyOneFileChanges | 247.4 ms | 3.88 ms | 3.63 ms | 17000.0000 | 6000.0000 | 104.55 MB |

Grouping outputs recalculate only the changed groups:
| Method             | Mean     | Error   | StdDev  | Gen0      | Gen1      | Allocated |
|------------------- |---------:|--------:|--------:|----------:|----------:|----------:|
| OnlyOneFileChanges | 198.8 ms | 3.74 ms | 7.65 ms | 3000.0000 | 1000.0000 |  21.89 MB |