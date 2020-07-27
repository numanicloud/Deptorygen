using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Deptorygen.Generator.Analyzer;
using Deptorygen.Generator.Definition;
using Deptorygen.Generator.Syntaxes;
using Deptorygen.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Deptorygen.Generator
{
	class FactoryGenerator
	{
		public async Task<FactoryDefinition> GetDefinition(Document document,
			InterfaceDeclarationSyntax target,
			CancellationToken ct)
		{
			try
			{
				var syntax = await FactorySyntax.FromDeclarationAsync(target, document, ct);
				var analyzer = new FactoryAnalyzer(syntax);
				return analyzer.GetDefinition();
			}
			catch (Exception e)
			{
				Logger.WriteLine(e.ToString()).Wait();
				throw;
			}
		}

		public Solution GetFactoryAppliedSolution(Document document, FactoryDefinition definition)
		{
			var template = new FactoryTemplate(definition);
			var code = template.TransformText();

			Logger.WriteLine(code).Wait();

			var fileName = $"{definition.TypeName}.g.cs";
			var existing = document.Project.Documents
				.Where(d => d.Folders.IsStructualEqual(document.Folders))
				.FirstOrDefault(d => d.Name == fileName);
			if (existing is { })
			{
				var newDoc = existing.WithText(SourceText.From(code, Encoding.UTF8));
				return newDoc.Project.Solution;
			}

			var newDocument = document.Project.AddDocument(
				fileName,
				SourceText.From(code, Encoding.UTF8),
				document.Folders);

			return newDocument.Project.Solution;
		}
	}
}
