using Deptorygen.Generator.Definition;

namespace Deptorygen.Generator
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
