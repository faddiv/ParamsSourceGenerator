#Source Generator general measurements

## SyntaxFactory

This test measures how the source generation performs on simple case when the SyntaxFactory is used:

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

### Using InterpolatedStringHandler for optimalization

This further reduce memory usage by avoiding creating strings when not needed by take advantage of InterpolatedStringHandlerAttribute. Some code seems become slower tough.

| Method       | Mean     | Error    | StdDev   | Gen0    | Gen1   | Allocated |
|------------- |---------:|---------:|---------:|--------:|-------:|----------:|
| RunGenerator | 658.9 us | 12.89 us | 14.33 us | 39.0625 | 7.8125 | 169.25 KB |

### Using object pool

Using object pool for source builder and ensure initial capacity to be enough.

| Method       | Mean     | Error   | StdDev  | Gen0    | Gen1   | Allocated |
|------------- |---------:|--------:|--------:|--------:|-------:|----------:|
| RunGenerator | 555.6 us | 9.51 us | 7.94 us | 31.2500 | 7.8125 | 137.13 KB |

### SourceLine readibility improvements

The SourceLine readibility improvements shouldn't change anything, but running the thests again and again reveals, the Allocated memory isn't measured consistently.

| Method       | Mean     | Error    | StdDev   | Gen0    | Gen1   | Allocated |
|------------- |---------:|---------:|---------:|--------:|-------:|----------:|
| RunGenerator | 550.5 us | 10.67 us | 14.95 us | 31.2500 | 7.8125 | 136.81 KB |

### Improvig Transform

Removed the AttributeSyntax calculation when it doesn't needed and GetSemanticModel since thecontext has it anyway.

| Method       | Mean     | Error   | StdDev   | Gen0    | Gen1   | Allocated |
|------------- |---------:|--------:|---------:|--------:|-------:|----------:|
| RunGenerator | 505.4 us | 9.17 us | 10.19 us | 31.2500 | 3.9063 |  133.2 KB |

## Incremental pipeline improvements

These test measures the improvements on the incremental pipeline changes.

Testing if separating file grouping during pipeline make optimizations.

For this 1000 file is used. All has one method that needs source generated. After generation, let's change only one file. Before, the .Collect() is used, which process all the file in one go, after change, only the changed file is sent for the source generator.

### Calculating all output together

| Method             | Mean     | Error   | StdDev  | Gen0       | Gen1      | Allocated |
|------------------- |---------:|--------:|--------:|-----------:|----------:|----------:|
| OnlyOneFileChanges | 247.4 ms | 3.88 ms | 3.63 ms | 17000.0000 | 6000.0000 | 104.55 MB |

### Grouping outputs

Grouping outputs recalculate only the changed groups:

| Method             | Mean     | Error   | StdDev  | Gen0      | Gen1      | Allocated |
|------------------- |---------:|--------:|--------:|----------:|----------:|----------:|
| OnlyOneFileChanges | 198.8 ms | 3.74 ms | 7.65 ms | 3000.0000 | 1000.0000 |  21.89 MB |

### Using InterpolatedStringHandler for optimalization

In case of changing one file in 1000 source, the gain is small.

| Method             | Mean     | Error   | StdDev  | Gen0      | Gen1      | Allocated |
|------------------- |---------:|--------:|--------:|----------:|----------:|----------:|
| OnlyOneFileChanges | 184.5 ms | 3.62 ms | 5.31 ms | 3000.0000 | 1000.0000 |   21.8 MB |

### Using object pool

| Method             | Mean     | Error   | StdDev  | Gen0      | Gen1      | Allocated |
|------------------- |---------:|--------:|--------:|----------:|----------:|----------:|
| OnlyOneFileChanges | 184.5 ms | 3.54 ms | 4.48 ms | 3000.0000 | 1000.0000 |  21.77 MB |

### SourceLine readibility improvements

(Inconsistent memory readings.)

| Method             | Mean     | Error   | StdDev  | Gen0      | Gen1      | Allocated |
|------------------- |---------:|--------:|--------:|----------:|----------:|----------:|
| OnlyOneFileChanges | 182.9 ms | 3.65 ms | 6.30 ms | 3000.0000 | 1000.0000 |  21.85 MB |

### Improvig Transform

As suspected, every transform optimalization has greater effect on caching.

| Method             | Mean     | Error   | StdDev  | Gen0      | Gen1      | Allocated |
|------------------- |---------:|--------:|--------:|----------:|----------:|----------:|
| OnlyOneFileChanges | 161.6 ms | 3.18 ms | 4.66 ms | 3000.0000 | 1000.0000 |  18.67 MB |