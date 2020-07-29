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

        protected override CodeFixProvider GetCSharpCodeFixProvider() => new DeptorygenCodeFixProvider();

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new DeptorygenAnalyzer();
    }
}
