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
		private TypeName FactoryInterfaceInfo => new TypeName("Sample", "ISubject", Accessibility.Public);
		private TypeName FactoryInfo => new TypeName("Sample", "Subject", Accessibility.Public);
		private TypeName FactoryInterfaceInfo2 => new TypeName("Sample", "IFactory", Accessibility.Public);

		private FactoryDefinition GetFixtureFactoryDefinition(bool doCapture = false)
		{
			var dependencies = new[] { ServiceAInfo, };

			var resolution = new ResolutionDefinition(ServiceBInfo, dependencies, false);

			var resolveServiceB = new ResolverDefinition(
				"ResolveServiceB",
				ServiceBInfo,
				resolution,
				new VariableDefinition[0],
				false,
				"_ResolveServiceB");
			var resolveFactory = new ResolverDefinition(
				"ResolveFactory",
				FactoryInterfaceInfo2,
				new ResolutionDefinition(
					new TypeName("Sample", "Factory", Accessibility.Public),
					new TypeName[0],
					false),
				new VariableDefinition[0],
				false,
				"_ResolverFactoryCache");

			var resolvers = new[] { resolveServiceB };

			var collectionResolvers = new CollectionResolverDefinition[] { };

			var captures = doCapture ? new CaptureDefinition[]
			{
				new CaptureDefinition(
					FactoryInterfaceInfo2,
					"Captured",
					resolvers,
					new CollectionResolverDefinition[0]),
			} : new CaptureDefinition[0];

			var definition = new FactoryDefinition(
				"Subject",
				FactoryInterfaceInfo,
				dependencies,
				new []{ resolveServiceB, resolveFactory },
				collectionResolvers,
				captures,
				false);

			return definition;
		}

		[Fact]
		public void 解決メソッドよりもキャプチャしたファクトリープロパティのほうが優先()
		{
			var definition = GetFixtureFactoryDefinition(true);

			var generated = definition.Injection.GetExpression(FactoryInterfaceInfo2);

			Assert.Equal("Captured", generated);
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
