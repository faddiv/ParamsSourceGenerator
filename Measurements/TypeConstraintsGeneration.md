# TypeConstraint improvements

Turns out the previous version wasn't allocation free. The reason probably because the string array was casted to IEnumerable<string>.

| Method           | Mean      | Error    | StdDev   | Gen0   | Allocated |
|----------------- |----------:|---------:|---------:|-------:|----------:|
| TypeConstraintV1 | 199.80 ns | 2.129 ns | 1.887 ns | 0.0191 |      80 B |
| TypeConstraintV2 |  93.40 ns | 1.238 ns | 1.158 ns |      - |         - |
