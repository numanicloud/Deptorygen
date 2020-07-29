using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using TestHelper;
using Xunit;

namespace Deptorygen.XUnit
{
    public class UnitTests : ConventionCodeFixVerifier
    {
        [Fact]
        public void Basic() => VerifyCSharpByConvention();

        [Fact]
        public void AutoAndManual() => VerifyCSharpByConvention();

        [Fact]
        public void BasicDependency() => VerifyCSharpByConvention();

        [Fact]
        public void Capture1() => VerifyCSharpByConvention();

        [Fact]
        public void Capture2() => VerifyCSharpByConvention();

        protected override CodeFixProvider GetCSharpCodeFixProvider() => new DeptorygenCodeFixProvider();

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new DeptorygenAnalyzer();
    }
}
