using System.Collections.Generic;
using System.Linq;
using Deptorygen.Utilities;

namespace Deptorygen.Generator.Definition
{
	public enum InjectionMethod
	{
		Parameter, This, CapturedFactory, CapturedResolver, Resolver, Field
	}

	public class InjectionExpression
	{
		public InjectionExpression(TypeName type, InjectionMethod method, string code)
		{
			Type = type;
			Method = method;
			Code = code;
		}

		public TypeName Type { get; }
		public InjectionMethod Method { get; }
		public string Code { get; }
	}

	public class ExpressionRouter
	{
		private readonly Dictionary<TypeName, List<InjectionExpression>> _expressions;

		public ExpressionRouter(IEnumerable<InjectionExpression> expressions)
		{
			_expressions = expressions.GroupBy(x => x.Type)
				.ToDictionary(x => x.Key, x => x.ToList());
		}

		public InjectionExpression GetExpression(TypeName typeName)
		{
			return _expressions[typeName].OrderBy(x => x.Method)
				.First();
		}
	}
}
