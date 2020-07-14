using System;
using System.Collections.Generic;
using System.Text;

namespace Deprovgen.Annotations
{
	[AttributeUsage(AttributeTargets.Interface)]
	public class FactoryProviderAttribute : Attribute
	{
		public Type ChildType { get; }

		public FactoryProviderAttribute(Type childType)
		{
			ChildType = childType;
		}
	}
}
