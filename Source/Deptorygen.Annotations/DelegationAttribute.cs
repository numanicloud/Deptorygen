using System;

namespace Deptorygen.Annotations
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class DelegationAttribute : Attribute
	{
		public DelegationAttribute(string methodName)
		{
			MethodName = methodName;
		}

		public string MethodName { get; }
	}
}
