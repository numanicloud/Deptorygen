using System;
using System.Collections.Generic;
using System.Text;
using Deptorygen.Generator.Definition;
using Deptorygen.Utilities;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Deptorygen.XUnit.RegularTests.Definitions
{
	public class FactoryDefinitionTest
	{
		private TypeName ServiceAInfo => new TypeName("Sample", "ServiceA", Accessibility.Public);
		private TypeName ServiceBInfo => new TypeName("Sample", "ServiceB", Accessibility.Public);
		private TypeName ServiceCInfo => new TypeName("Sample", "ServiceC", Accessibility.Public);
		private TypeName FactoryInterfaceInfo => new TypeName("Sample", "ISubject", Accessibility.Public);
		private TypeName FactoryInterfaceInfo2 => new TypeName("Sample", "IFactory", Accessibility.Public);

		private ResolverDefinition ResolverDefinition(
			TypeName typename,
			VariableDefinition[]? variables = null,
			ResolutionDefinition? resolution = null)
		{
			return new ResolverDefinition(
				$"Resolve{typename.Name}",
				typename,
				resolution ?? new ResolutionDefinition(typename, new TypeName[0], false),
				variables ?? new VariableDefinition[0],
				false,
				$"_Resolver{typename.Name}Cache");
		}

		private TypeName ToEnumerableType(TypeName source)
		{
			return new TypeName(
				"System.Collections.Generic",
				"IEnumerable",
				Accessibility.Public,
				new []{source});
		}

		private CollectionResolverDefinition GetCollectionResolverDefinition(
			TypeName typeName,
			VariableDefinition[]? variables = null)
		{
			return new CollectionResolverDefinition(
				ToEnumerableType(typeName),
				$"Resolve{typeName.Name}s",
				new TypeName[0],
				variables ?? new VariableDefinition[0],
				Accessibility.Public);
		}

		private FactoryDefinition GetFixtureFactoryDefinition(bool doCapture = false)
		{
			var resolveFactory = ResolverDefinition(FactoryInterfaceInfo2);
			var resolveServiceB = ResolverDefinition(ServiceBInfo);
			var resolveServiceC = ResolverDefinition(
				ServiceCInfo,
				variables: new[] { new VariableDefinition(FactoryInterfaceInfo, "subject") });
			var collectionResolverA = GetCollectionResolverDefinition(ServiceAInfo);
			var collectionResolverC = GetCollectionResolverDefinition(ServiceCInfo,
				new VariableDefinition[]
				{
					new VariableDefinition(FactoryInterfaceInfo, "self"), 
				});

			var captures = doCapture ? new CaptureDefinition[]
			{
				new CaptureDefinition(
					FactoryInterfaceInfo2,
					"Captured",
					new[] { resolveServiceB },
					new []{ collectionResolverA }),
				new CaptureDefinition(
					FactoryInterfaceInfo,
					"Custom",
					new []{ resolveServiceB, resolveFactory },
					new CollectionResolverDefinition[0]),
			} : new CaptureDefinition[0];

			return new FactoryDefinition(
				"Subject",
				FactoryInterfaceInfo,
				new TypeName[0],
				new[] { resolveServiceB, resolveFactory, resolveServiceC },
				new[] { collectionResolverA, collectionResolverC },
				captures,
				false);
		}

		[Fact]
		public void コレクション解決メソッドでthisよりパラメータのほうが優先()
		{
			var definition = GetFixtureFactoryDefinition();

			var generated = definition.Injection.GetExpression(ToEnumerableType(ServiceCInfo));

			Assert.Equal("ResolveServiceCs(self)", generated);
		}

		private FactoryDefinition FactoryDefinition(string name, TypeName interfaceInfo,
			ResolverDefinition[]? resolvers = null,
			CollectionResolverDefinition[]? collectionResolvers = null,
			CaptureDefinition[]? captures = null)
		{
			return new FactoryDefinition(
				name,
				interfaceInfo,
				new TypeName[0],
				resolvers ?? new ResolverDefinition[0],
				collectionResolvers ?? new CollectionResolverDefinition[0], 
				captures ?? new CaptureDefinition[0], 
				false);
		}

		[Fact]
		public void 解決メソッドでthisよりパラメータのほうが優先()
		{
			var resolveServiceC = ResolverDefinition(ServiceCInfo,
				variables: new[] { new VariableDefinition(FactoryInterfaceInfo, "subject") });
			var factory = FactoryDefinition("Subject", FactoryInterfaceInfo,
				resolvers: new[] {resolveServiceC});

			var generated = factory.Injection.GetExpression(ServiceCInfo);

			Assert.Equal("ResolveServiceC(subject)", generated);
		}

		[Fact]
		public void キャプチャしたファクトリーよりthisのほうが優先()
		{
			var definition = GetFixtureFactoryDefinition(true);

			var generated = definition.Injection.GetExpression(FactoryInterfaceInfo);

			Assert.Equal("this", generated);
		}

		[Fact]
		public void 解決メソッドよりもキャプチャしたファクトリープロパティのほうが優先()
		{
			var definition = GetFixtureFactoryDefinition(true);

			var generated = definition.Injection.GetExpression(FactoryInterfaceInfo2);

			Assert.Equal("Captured", generated);
		}
		[Fact]
		public void コレクション解決メソッドよりもキャプチャしたコレクション解決メソッドのほうが優先()
		{
			var definition = GetFixtureFactoryDefinition(true);

			var generated = definition.Injection.GetExpression(ToEnumerableType(ServiceAInfo));

			Assert.Equal("Captured.ResolveServiceAs()", generated);
		}

		[Fact]
		public void 解決メソッドよりもキャプチャした解決メソッドのほうが優先()
		{
			var definition = GetFixtureFactoryDefinition(true);

			var generated = definition.Injection.GetExpression(ServiceBInfo);

			Assert.Equal("Captured.ResolveServiceB()", generated);
		}

		[Fact]
		public void 式として解決メソッドが使用される()
		{
			var definition = GetFixtureFactoryDefinition();

			var generated = definition.Injection.GetExpression(ServiceBInfo);

			Assert.Equal("ResolveServiceB()", generated);
		}
	}
}
