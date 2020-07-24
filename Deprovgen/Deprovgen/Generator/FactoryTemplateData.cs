using Deprovgen.Generator.Definition;

namespace Deprovgen.Generator
{
	partial class FactoryTemplate
	{
		public FactoryTemplate(FactoryDefinition factory)
		{
			Factory = factory;
		}

		public FactoryDefinition Factory { get; }
	}
}
