using System;
using System.Collections.Generic;
using System.Text;

namespace Deprovgen.Annotations
{
	[AttributeUsage(AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
	public class FactoryProviderAttribute : Attribute
	{
		public Type ChildType { get; }

		public FactoryProviderAttribute(Type childType)
		{
			ChildType = childType;
		}
	}
}
