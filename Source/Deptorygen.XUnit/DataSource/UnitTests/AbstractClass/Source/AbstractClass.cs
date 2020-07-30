using System;
using Deptorygen.Annotations;

namespace UseDeptorygen.Samples.AbstractClass
{
	abstract class ServiceBase
	{
		public abstract void Tell();
	}

	class ServiceSilver : ServiceBase
	{
		public override void Tell()
		{
			Console.WriteLine("This is silver.");
		}
	}

	class ServiceGold : ServiceBase
	{
		public override void Tell()
		{
			Console.WriteLine("This is gold.");
		}
	}

	[Factory]
	interface IFactory
	{
		[Resolution(typeof(ServiceSilver))]
		ServiceBase ResolveService();
	}
}
