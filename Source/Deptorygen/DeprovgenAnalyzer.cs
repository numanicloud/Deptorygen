using System.Collections.Immutable;
using System.Linq;
using Deptorygen.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Deptorygen
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class DeprovgenAnalyzer : DiagnosticAnalyzer
	{
		public const string DiagnosticId = "Deprovgen";

		// You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
		// See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
		private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
		private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
		private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
		private const string Category = "Naming";

		private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

		public override void Initialize(AnalysisContext context)
		{
			context.EnableConcurrentExecution();
			context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
			// TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
			// See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
			context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
		}

		private static void AnalyzeSymbol(SymbolAnalysisContext context)
		{
			// TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
			var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

			if (namedTypeSymbol.TypeKind != TypeKind.Interface)
			{
				return;
			}

			if (namedTypeSymbol.GetAttributes().Any(a => a.AttributeClass.Name == nameof(FactoryAttribute)))
			{
				// For all such symbols, produce a diagnostic.
				var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
				context.ReportDiagnostic(diagnostic);
			}
		}
	}
}
