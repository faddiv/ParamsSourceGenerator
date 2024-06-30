using Foxy.Params;
using System;

namespace SourceGeneratorTests.IntegrationTests.ErrorReportingTestCases.Reports_NoPartialKeywordOnParentClass;

public class ParentClass
{
    public partial class Bar
    {
        [{|#0:Params|}]
        private static void Format(string format, ReadOnlySpan<object> args)
        {
        }
    }
}
