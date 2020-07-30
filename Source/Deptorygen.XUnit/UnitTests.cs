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

        [Fact]
        public void CaptureMixin1() => VerifyCSharpByConvention();

        [Fact]
        public void CaptureMixin2() => VerifyCSharpByConvention();

        [Fact]
        public void Collection() => VerifyCSharpByConvention();

        [Fact]
        public void Disposable() => VerifyCSharpByConvention();

        [Fact]
        public void GenericHost() => VerifyCSharpByConvention();

        [Fact]
        public void Mixin1() => VerifyCSharpByConvention();

        [Fact]
        public void Mixin2() => VerifyCSharpByConvention();

        [Fact]
        public void Parameterize() => VerifyCSharpByConvention();

        [Fact]
        public void Transient() => VerifyCSharpByConvention();

        [Fact]
        public void UseCache() => VerifyCSharpByConvention();

        protected override CodeFixProvider GetCSharpCodeFixProvider() => new DeptorygenCodeFixProvider();

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new DeptorygenAnalyzer();
    }
}
