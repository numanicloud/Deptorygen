using System;
using System.Collections.Generic;
using System.Text;

namespace Deprovgen.Annotations
{
	[AttributeUsage(AttributeTargets.Method)]
	public class ImplementationAttribute : Attribute
	{
		public Type Type { get; }

		public ImplementationAttribute(Type type)
		{
			Type = type;
		}
	}
}
