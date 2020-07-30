using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Deptorygen.XUnit.Helpers
{
	static class Helper
	{
		public static Diagnostic[] GetDiagnostics(Project project, DiagnosticAnalyzer analyzer)
		{
			var compilationWithAnalyzers = project.GetCompilationAsync().Result.WithAnalyzers(ImmutableArray.Create(analyzer));
			var diagnostics = compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync().Result;
			return diagnostics.OrderBy(d => d.Location.SourceSpan.Start).ToArray();
		}
	}
}
