#Source Generator general measurements

## SyntaxFactory

This test measures how the source generation performs on simple case when the SytaxFactory is used:

| Method       | Mean     | Error    | StdDev   | Gen0     | Gen1     | Allocated |
|------------- |---------:|---------:|---------:|---------:|---------:|----------:|
| RunGenerator | 16.07 ms | 0.320 ms | 0 .479 ms | 500.0000 | 166.6667 |   3.62 MB |

## StringBuilder

This test measures how the source generation performs on simple case when the StringBuilder is used:

| Method       | Mean     | Error   | StdDev  | Gen0    | Gen1   | Allocated |
|------------- |---------:|--------:|--------:|--------:|-------:|----------:|
| RunGenerator | 594.6 us | 6.08 us | 8.12 us | 46.8750 | 7.8125 | 191.77 KB |

### StringBuilder Improvements

Decreased memoory allocations by using static generator. (c7da5bf2699cbaaa231f73668fe2ae42b7e7620b)

| Method       | Mean     | Error   | StdDev  | Gen0    | Gen1    | Allocated |
|------------- |---------:|--------:|--------:|--------:|--------:|----------:|
| RunGenerator | 564.7 us | 6.02 us | 6.69 us | 42.9688 | 11.7188 |  190.9 KB |

## Incremental pipeline improvements

These test measures the improvements on the incremental pipeline changes.

Testing if separating file grouping during pipeline make optimizations.

For this 1000 file is used. All has one method that needs source generated. After generation, let's change only one file. Before, the .Collect() is used, which process all the file in one go, after change, only the changed file is sent for the source generator.

Calculating all output together:
| Method             | Mean     | Error   | StdDev  | Gen0       | Gen1      | Allocated |
|------------------- |---------:|--------:|--------:|-----------:|----------:|----------:|
| OnlyOneFileChanges | 247.4 ms | 3.88 ms | 3.63 ms | 17000.0000 | 6000.0000 | 104.55 MB |

Grouping outputs recalculate only the changed groups:
| Method             | Mean     | Error   | StdDev  | Gen0      | Gen1      | Allocated |
|------------------- |---------:|--------:|--------:|----------:|----------:|----------:|
| OnlyOneFileChanges | 198.8 ms | 3.74 ms | 7.65 ms | 3000.0000 | 1000.0000 |  21.89 MB |