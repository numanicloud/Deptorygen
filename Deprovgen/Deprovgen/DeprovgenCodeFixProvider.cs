using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Deprovgen.Generator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Deprovgen
{
	[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DeprovgenCodeFixProvider)), Shared]
	public class DeprovgenCodeFixProvider : CodeFixProvider
	{
		private const string Title = "Create Factory";

		public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DeprovgenAnalyzer.DiagnosticId);

		public sealed override FixAllProvider GetFixAllProvider()
		{
			// See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
			return WellKnownFixAllProviders.BatchFixer;
		}

		public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
		{
			await Logger.WriteLine($"RegisterCodeFixesAsync");
			var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

			// TODO: Replace the following code with your own analysis, generating a CodeAction for each fix to suggest
			var diagnostic = context.Diagnostics.First();
			var diagnosticSpan = diagnostic.Location.SourceSpan;

			// Find the type declaration identified by the diagnostic.
			var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().First();

			// Register a code action that will invoke the fix.
			context.RegisterCodeFix(
				CodeAction.Create(
					Title,
					c => GenerateFactoryAsync(context.Document, declaration, c),
					Title),
				diagnostic);
		}

		private async Task<Solution> GenerateFactoryAsync(Document document, TypeDeclarationSyntax typeDecl, CancellationToken cancellationToken)
		{
			await Logger.WriteLine($"GenerateFactoryAsync, {typeDecl}");
			if (typeDecl is InterfaceDeclarationSyntax ids)
			{
				var handler = new FactoryGenerator();
				var definition = await handler.GetDefinition(document, ids, cancellationToken);
				return handler.GetFactoryAppliedSolution(document, definition);
			}

			return document.Project.Solution;
		}
	}
}
