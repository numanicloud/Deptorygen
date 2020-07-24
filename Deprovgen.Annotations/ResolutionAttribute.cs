using System;
using System.Collections.Generic;
using System.Text;

namespace Deprovgen.Annotations
{
	[AttributeUsage(AttributeTargets.Method)]
	public class ResolutionAttribute : Attribute
	{
		public Type Type { get; }

		public ResolutionAttribute(Type type)
		{
			Type = type;
		}
	}
}
