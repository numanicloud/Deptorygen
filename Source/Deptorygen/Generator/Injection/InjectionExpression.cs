using Deptorygen.Utilities;

namespace Deptorygen.Generator.Definition
{
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
}
