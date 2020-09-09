using System.Collections.Generic;

namespace Deptorygen.Generator.Interfaces
{
	interface IDefinitionRequiringNamespace
	{
		IEnumerable<string> GetRequiredNamespaces();
	}
}
