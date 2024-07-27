# Source Builder measurements

## How to create complex line with multiple segments

Testing different scenarios for increasing readibility, while avoiding performance and memory hits.

### StartLineWithAddSegments

StartLine starts a sub builder. On this the segments added one-by-one, and close with endline. StartLine uses struct.

```csharp
builder.AddBlock(static (sb, args) =>
{
    var line = sb.StartLine();
    line.AddSegment("var ");
	...
	line.EndLine();
}, (_argName, _spanArgumentType));
```

### AppendLineWithStringInterpolation

Using string interpolation for better readibility.

```csharp
builder.AddBlock(static (sb, args) =>
{
    sb.AppendLineV1($"var {args._argName}Span = new global::System.ReadOnlySpan<{args._spanArgumentType}>({args._argName});");
}, (_argName, _spanArgumentType));
```

Where the interpolated string converted to string.

### PassStringInterpolationToBuilder

Using string interpolation for better readibility.

```csharp
builder.AddBlock(static (sb, args) =>
{
	sb.AppendV2($"var {args._argName}Span = new global::System.ReadOnlySpan<{args._spanArgumentType} >( {args._argName});");
}, (_argName, _spanArgumentType));
```

Where the interpolated string passed without converting to string.

### InterpolatedStringHandler

Using InterpolatedStringHandler so interpolated string can be used but the lowered code work like StartLineWithAddSegments.

```csharp
builder.AddBlock(static (sb, args) =>
{
	sb.AppendV3($"var {args._argName}Span = new global::System.ReadOnlySpan<{args._spanArgumentType} >( {args._argName});");
}, (_argName, _spanArgumentType));
```

## Measurement results

| Method                            | Mean     | Error   | StdDev  | Gen0   | Allocated |
|---------------------------------- |---------:|--------:|--------:|-------:|----------:|
| StartLineWithAddSegments          | 202.6 ns | 3.63 ns | 3.22 ns | 0.1931 |     808 B |
| AppendLineWithStringInterpolation | 226.4 ns | 2.84 ns | 2.66 ns | 0.2255 |     944 B |
| PassStringInterpolationToBuilder  | 269.4 ns | 5.12 ns | 5.03 ns | 0.2103 |     880 B |
| InterpolatedStringHandler         | 113.0 ns | 0.70 ns | 0.66 ns | 0.1396 |     584 B |
