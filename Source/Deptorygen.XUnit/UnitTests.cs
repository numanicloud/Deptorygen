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

		[Fact]
		public void Resolution() => VerifyCSharpByConvention();

		[Fact]
		public void AbstractClass() => VerifyCSharpByConvention();

		[Fact]
		public void AllPublic() => VerifyCSharpByConvention();

		[Fact]
		public void InternalCapture() => VerifyCSharpByConvention();

		[Fact]
		public void InternalDependency() => VerifyCSharpByConvention();

		[Fact]
		public void InternalParameter() => VerifyCSharpByConvention();

		[Fact]
		public void InternalResolution() => VerifyCSharpByConvention();

		[Fact]
		public void InternalReturn() => VerifyCSharpByConvention();

		[Fact]
		public void InternalDefinition() => VerifyCSharpByConvention();

		[Fact]
		public void UsingNamespace() => VerifyCSharpByConvention();

		[Fact]
		public void ResolutionNamespace() => VerifyCSharpByConvention();

		[Fact]
		public void ResolveByCapture() => VerifyCSharpByConvention();

		[Fact]
		public void CollectionGenericHost() => VerifyCSharpByConvention();

		[Fact]
		public void CleanSelfDependency() => VerifyCSharpByConvention();

		protected override CodeFixProvider GetCSharpCodeFixProvider() => new DeptorygenCodeFixProvider();

		protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new DeptorygenAnalyzer();
	}
}
