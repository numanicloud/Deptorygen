using System;
using System.Collections.Generic;
using System.Text;

namespace Deprovgen.Annotations
{
	[AttributeUsage(AttributeTargets.Interface)]
	internal class InheritAttribute : Attribute
	{
		public Type Type { get; }

		public InheritAttribute(Type type)
		{
			Type = type;
		}
	}
}
