using System;
using System.Collections.Generic;
using System.Text;
using Deptorygen.Generator.Definition;

namespace Deptorygen.Generator.Interfaces
{
	public interface IInjectionProvider
	{
		IEnumerable<InjectionExpression> GetExpressions(ExpressionRouter? context);
	}
}
