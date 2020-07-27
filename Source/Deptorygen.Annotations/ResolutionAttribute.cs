using System;

namespace Deptorygen.Annotations
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class ResolutionAttribute : Attribute
	{
		public Type Type { get; }

		public ResolutionAttribute(Type type)
		{
			Type = type;
		}
	}
}
