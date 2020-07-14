using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Deprovgen.Annotations;
using Deprovgen.Generator.Domains;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;

namespace Deprovgen.Generator.Analyzers
{
	class FactoryAnalyzer
	{
		private readonly Document _document;
		private readonly Func<CancellationToken, Task<INamedTypeSymbol>> _getSymbolFunc;

		public FactoryAnalyzer(InterfaceDeclarationSyntax syntax, Document document)
		{
			_document = document;
			_getSymbolFunc = ct =>
			{
				var ns = syntax.Parent is NamespaceDeclarationSyntax nsds
					? (nsds.Name is QualifiedNameSyntax qns ? qns.ToString() : throw new Exception())
					: throw new Exception();
				return AnalyzeInterfaceAsync(ns, syntax.Identifier.ValueText, ct);
			};
		}

		public FactoryAnalyzer(Type type, Document document)
		{
			_document = document;

			_getSymbolFunc = ct => AnalyzeInterfaceAsync(type.Namespace, type.Name, ct);
		}

		private async Task<INamedTypeSymbol> AnalyzeInterfaceAsync(string @namespace, string typeName, CancellationToken ct)
		{
			var symbols = await SymbolFinder.FindSourceDeclarationsAsync(
				_document.Project,
				typeName,
				false,
				ct);
			return symbols.OfType<INamedTypeSymbol>()
				.First(x => x.GetFullNameSpace() == @namespace);
		}

		private async Task<FactoryDefinition> AnalyzeInterfaceAsync(INamedTypeSymbol symbol, CancellationToken ct)
		{
			var methods = ResolverAnalyzer.GetResolverDefinitions(symbol);
			var dependencies = GetDependencies(methods);
			var children = await GetChildrenAsync(symbol, ct).ToArrayAsync(ct);

			var typeName = symbol.Name[0].ToString().Replace("I", "") + symbol.Name.Substring(1);

			return new FactoryDefinition(symbol.GetFullNameSpace(), typeName, dependencies, methods, children, symbol.Name);
		}

		private static ServiceDefinition[] GetDependencies(ResolverDefinition[] methods)
		{
			return methods.Select(x => x.ServiceType)
				.SelectMany(x => x.Dependencies)
				.Distinct(x => $"{x.Namespace}.{x.TypeName}")
				.ToArray();
		}

		private async IAsyncEnumerable<FactoryDefinition> GetChildrenAsync(INamedTypeSymbol symbol,
			[EnumeratorCancellation] CancellationToken ct = default)
		{
			foreach (var attributeData in symbol.GetAttributes())
			{
				if (attributeData.AttributeClass.Name != "FactoryProviderAttribute")
				{
					continue;
				}

				if (attributeData.ConstructorArguments[0].Value is Type type
				    && type.IsInterface
				    && type.GetCustomAttributesData().Any(x => x.AttributeType.Name == nameof(FactoryAttribute)))
				{
					var analyzer = new FactoryAnalyzer(type, _document);
					yield return await analyzer.GetFactoryDefinitionAsync(ct);
				}
			}
		}

		public async Task<FactoryDefinition> GetFactoryDefinitionAsync(CancellationToken ct)
		{
			var symbol = await _getSymbolFunc(ct);
			return await AnalyzeInterfaceAsync(symbol, ct);
		}
	}
}
