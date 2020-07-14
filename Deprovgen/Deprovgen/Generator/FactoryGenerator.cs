using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Deprovgen.Generator.Domains;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Deprovgen.Generator
{
	class FactoryGenerator
	{
		public async Task<FactoryDefinition> GetDefinition(Document document,
			InterfaceDeclarationSyntax target,
			CancellationToken ct)
		{
			try
			{
				var analyzer = new Analyzers.FactoryAnalyzer(target, document);
				return await analyzer.GetFactoryDefinitionAsync(ct);
			}
			catch (Exception e)
			{
				Logger.WriteLine(e.ToString()).Wait();
				throw;
			}
		}

		public Solution GetFactoryAppliedSolution(Document document, FactoryDefinition definition)
		{
			var template = new FactoryTemplate() { Factory = definition };
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

		private static async Task Log(Document document, CancellationToken cancellationToken, IEnumerable<Document> docs)
		{
			foreach (var reference in document.Project.MetadataReferences)
			{
				await Logger.WriteLine(reference.Display);
				await Logger.WriteLine(reference.Properties.ToString());
			}

			foreach (var doc in docs)
			{
				await Logger.WriteLine(doc.Name);
				var sm = await doc.GetSemanticModelAsync(cancellationToken);
				await Logger.WriteLine(sm.SyntaxTree.ToString());
			}
		}
	}
}
