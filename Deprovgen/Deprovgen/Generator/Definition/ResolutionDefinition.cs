using Deprovgen.Utilities;

namespace Deprovgen.Generator.Definition
{
	public class ResolutionDefinition
	{
		public TypeName TypeName { get; }
		public TypeName[] Dependencies { get; }

		public ResolutionDefinition(TypeName typeName, TypeName[] dependencies)
		{
			TypeName = typeName;
			Dependencies = dependencies;
		}
	}
}
