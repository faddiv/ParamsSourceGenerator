﻿// ReSharper disable All
namespace PerformanceTest.TestFiles
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Foxy.Params.SourceGenerator", "1.0.0.0")]
    [global::System.AttributeUsage(global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    sealed class ParamsAttribute : global::System.Attribute
    {
        public int MaxOverrides { get; set; } = 3;
        public bool HasParams { get; set; } = true;
    }
}
