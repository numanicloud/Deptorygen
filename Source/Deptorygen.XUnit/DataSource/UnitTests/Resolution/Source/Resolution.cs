using System;
using Deptorygen.Annotations;

namespace UseDeptorygen.Samples.Resolution
{
	class ServiceSilver : IService
	{
		public void Say()
		{
			Console.WriteLine("This is silver.");
		}
	}

	class ServiceGold : IService
	{
		public void Say()
		{
			Console.WriteLine("This is gold.");
		}
	}

	interface IService
	{
		void Say();
	}

	[Factory]
	interface IFactory
	{
		[Resolution(typeof(ServiceGold))]
		IService ResolveService();
	}
}
