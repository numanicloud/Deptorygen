using System;

namespace Deprovgen.Annotations
{
	[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
	public class FactoryAttribute : Attribute
	{
	}
}
