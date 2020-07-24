using System;
using System.Collections.Generic;
using System.Text;
using Deprovgen.Utilities;

namespace Deprovgen.Generator.DefinitionV2
{
	class ResolutionDefinition
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
