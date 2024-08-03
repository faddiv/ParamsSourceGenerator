# GetTypeHierarchy

## GetTypeHierarchyV1
Creating list, reverse, select display string to array. This has Lots of operation and it is slow.

## GetTypeHierarchyV2

Recursive implementation, wich creates the final array in the deepest level. Looks stupid, but works.

## GetTypeHierarchyV3

Counting the level, then creating the array. No recursion, much cleaner.

## GetTypeHierarchyVR

Using the real Implementations seems much slower.

## GetTypeHierarchyX

Empty implementation, it only creates the array.

| Method             | Mean      | Error    | StdDev   | Gen0   | Allocated |
|------------------- |----------:|---------:|---------:|-------:|----------:|
| GetTypeHierarchyV1 | 130.95 ns | 0.890 ns | 0.789 ns | 0.0496 |     208 B |
| GetTypeHierarchyV2 |  21.66 ns | 0.471 ns | 0.596 ns | 0.0115 |      48 B |
| GetTypeHierarchyV3 |  17.92 ns | 0.393 ns | 0.348 ns | 0.0115 |      48 B |
| GetTypeHierarchyVR | 507.90 ns | 5.770 ns | 5.390 ns | 0.0114 |      48 B |
| GetTypeHierarchyX  |           |          |          | 0.0115 |      48 B |