using Deprovgen.Utilities;

namespace Deprovgen.Generator.Definition
{
	public class ResolutionDefinition
	{
		public TypeName TypeName { get; }
		public TypeName[] Dependencies { get; }
		public bool IsDisposable { get; }

		public ResolutionDefinition(TypeName typeName, TypeName[] dependencies, bool isDisposable)
		{
			TypeName = typeName;
			Dependencies = dependencies;
			IsDisposable = isDisposable;
		}
	}
}
