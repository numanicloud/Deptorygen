using System;
using System.Collections.Generic;
using System.Text;
using Deptorygen.Utilities;

namespace Deptorygen.Generator.Interfaces
{
	interface IServiceProvider
	{
		IEnumerable<TypeName> GetCapableServiceTypes();
	}
}
