using Deptorygen.Generator.Definition;
using Deptorygen.Generator.Injection;

namespace Deptorygen.Generator
{
	partial class FactoryTemplate
	{
		public FactoryTemplate(FactoryDefinition factory)
		{
			Factory = factory;
		}

		public FactoryDefinition Factory { get; }

		public string GetResolution(ResolverDefinition resolver)
		{
			var aggregator = new InjectionAggregator(Factory, resolver);
			return aggregator.GetResolution(resolver.Resolution);
		}

		public string GetResolutionList(CollectionResolverDefinition resolver)
		{
			var aggregator = new InjectionAggregator(Factory, resolver);
			return aggregator.GetResolutionList(resolver);
		}
	}
}
