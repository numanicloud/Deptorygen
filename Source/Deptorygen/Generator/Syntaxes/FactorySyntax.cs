using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Deptorygen.Generator.Definition;
using Deptorygen.Generator.Interfaces;
using Deptorygen.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using IServiceProvider = Deptorygen.Generator.Interfaces.IServiceProvider;

namespace Deptorygen.Generator.Syntaxes
{
	class FactorySyntax : IServiceProvider, IInjectionProvider
	{
		public INamedTypeSymbol InterfaceSymbol { get; }
		public ResolverSyntax[] Resolvers { get; }
		public CollectionResolverSyntax[] CollectionResolvers { get; }
		public CaptureSyntax[] Captures { get; }

		public FactorySyntax(INamedTypeSymbol interfaceSymbol,
			ResolverSyntax[] resolvers,
			CollectionResolverSyntax[] collectionResolvers,
			CaptureSyntax[] captures)
		{
			InterfaceSymbol = interfaceSymbol;
			Resolvers = resolvers;
			CollectionResolvers = collectionResolvers;
			Captures = captures;
		}

		public static async Task<FactorySyntax> FromDeclarationAsync(InterfaceDeclarationSyntax syntax, Document document, CancellationToken ct)
		{
			async Task<INamedTypeSymbol> GetSymbol()
			{
				var ns = syntax.Parent is NamespaceDeclarationSyntax nsds
					? (nsds.Name is QualifiedNameSyntax qns ? qns.ToString() : throw new Exception())
					: throw new Exception();

				var symbols = await SymbolFinder.FindSourceDeclarationsAsync(
					document.Project,
					syntax.Identifier.ValueText,
					false,
					ct);

				var namedTypeSymbol = symbols.OfType<INamedTypeSymbol>()
					.First(x => x.GetFullNameSpace() == ns);
				return namedTypeSymbol;
			}

			var symbol = await GetSymbol();
			var resolvers = ResolverSyntax.FromParent(symbol);
			var captures = CaptureSyntax.FromFactory(symbol);

			return new FactorySyntax(symbol, resolvers.Item1, resolvers.Item2, captures);
		}

		public IEnumerable<TypeName> GetCapableServiceTypes()
		{
			yield return TypeName.FromSymbol(InterfaceSymbol);
			foreach (var capture in Captures)
			{
				yield return capture.TypeName;
			}
		}

		public IEnumerable<InjectionExpression> GetExpressions(ExpressionRouter? context)
		{
			var dependencies = GetDependencies()
				.Select(x => new InjectionExpression(x,
					InjectionMethod.Field,
					$"_{x.LowerCamelCase}"));

			var resolverExps = Resolvers.Where(x => !x.Parameters.Any())
				.Select(x => new InjectionExpression(x.ReturnTypeName, InjectionMethod.Resolver, $"{x.MethodName}()"));
			var collectionExps = CollectionResolvers.Where(x => !x.Parameters.Any())
				.Select(x => new InjectionExpression(x.CollectionType, InjectionMethod.Resolver, $"{x.MethodName}()"));
			var captureExps = Captures.SelectMany(x => x.Resolvers.Select(y => (capture: x, resolver: y)))
				.Where(x => !x.resolver.Parameters.Any())
				.Select(x => new InjectionExpression(
					x.resolver.ReturnTypeName,
					InjectionMethod.CapturedResolver,
					$"{x.capture.PropertyName}.{x.resolver.MethodName}()"));
			var captures = Captures.Select(x =>
				new InjectionExpression(x.TypeName, InjectionMethod.CapturedFactory, $"{x.PropertyName}"));
			var self = new InjectionExpression(TypeName.FromSymbol(InterfaceSymbol), InjectionMethod.This, "this");
			
			// TODO: パラメータ有りのものはどうする？
			// 解決のためにパラメータ有りリゾルバーを呼び出さないといけない場合が厄介そう

			var factoryContext = new ExpressionRouter(dependencies);

			return resolverExps.Concat(collectionExps)
				.Concat(captureExps)
				.Concat(captures)
				.Append(self);
		}

		private TypeName[] GetDependencies()
		{
			var consumed = Resolvers
				.Cast<IServiceConsumer>()
				.Concat(CollectionResolvers)
				.SelectMany(x => x.GetRequiredServiceTypes());

			var provided = Resolvers
				.Cast<IServiceProvider>()
				.Concat(CollectionResolvers)
				.Concat(Captures)
				.Append(this)
				.SelectMany(x => x.GetCapableServiceTypes());

			return consumed.Except(provided).ToArray();
		}
	}
}
