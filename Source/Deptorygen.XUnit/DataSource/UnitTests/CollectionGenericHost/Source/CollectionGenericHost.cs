using System;
using System.Collections.Generic;
using Deptorygen.Annotations;
using Deptorygen.GenericHost;

namespace UseDeptorygen.Samples.CollectionGenericHost
{
	abstract class Service
	{
		public abstract void Do();
	}

	class ServiceA : Service
	{
		public override void Do()
		{
			Console.WriteLine("This is ServiceA");
		}
	}

	class ServiceB : Service
	{
		public override void Do()
		{
			Console.WriteLine("This is ServiceB");
		}
	}

	class ServiceC : Service
	{
		public override void Do()
		{
			Console.WriteLine("This is ServiceC");
		}
	}

	class Client
	{
		private readonly IEnumerable<Service> _services;

		public Client(IEnumerable<Service> services)
		{
			_services = services;
		}

		public void Invoke()
		{
			Console.WriteLine("# Client");
			foreach (var service in _services)
			{
				service.Do();
			}
		}
	}

	[Factory]
	[ConfigureGenericHost]
	interface IFactory
	{
		ServiceA ResolveServiceA();
		ServiceB ResolveServiceB();
		ServiceC ResolveServiceC();
		[Resolution(typeof(ServiceA))]
		[Resolution(typeof(ServiceB))]
		[Resolution(typeof(ServiceC))]
		IEnumerable<Service> ResolveServices();
		Client ResolveClient();
	}
}