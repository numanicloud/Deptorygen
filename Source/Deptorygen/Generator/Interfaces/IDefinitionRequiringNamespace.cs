using System;
using System.Collections.Generic;
using System.Text;

namespace Deptorygen.Generator.Interfaces
{
	interface IDefinitionRequiringNamespace
	{
		IEnumerable<string> GetRequiredNamespaces();
	}
}
