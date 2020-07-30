using System;
using System.Collections.Generic;
using System.Text;
using Deptorygen.Generator.Definition;
using Deptorygen.Utilities;

namespace Deptorygen.Generator.Interfaces
{
	public interface IInjectionGenerator
	{
		string? GetInjectionExpression(TypeName typeName, InjectionContext context);
	}
}
