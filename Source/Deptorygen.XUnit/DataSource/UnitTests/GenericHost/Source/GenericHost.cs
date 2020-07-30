using System;
using Deptorygen.Annotations;
using Deptorygen.GenericHost;
using Microsoft.Extensions.DependencyInjection;

namespace UseDeptorygen.Samples.GenericHost
{
	class Service
	{
		public void Tell()
		{
			Console.WriteLine("Wow its Service!");
		}
	}

	class Service2
	{
		public void Describe()
		{
			Console.WriteLine("It is Service2.");
		}
	}

	class Client
	{
		private readonly Service _service;
		private readonly Service2 _service2;

		public Client(Service service, Service2 service2)
		{
			_service = service;
			_service2 = service2;
		}

		public void Work()
		{
			Console.WriteLine("# Client");
			_service.Tell();
			_service2.Describe();
		}
	}

	// ConfigureGenericHost 属性をつけると、GenericHostで使えるようになる
	[Factory]
	[ConfigureGenericHost]
	interface IFactory
	{
		Service ResolveService();
		Service2 ResolveService2();
		Client ResolveClient();
	}
}