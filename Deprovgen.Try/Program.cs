using System;
using System.Collections.Generic;
using Deprovgen.Annotations;
using Deprovgen.GenericHost;
using Deprovgen.Try.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Deprovgen.Try.Interface
{
	public interface IServiceIron
	{
		void Tell();
	}
}

namespace Deprovgen.Try
{
	public interface IService
	{
		void Say();
	}

	public class ServiceA : IService
	{
		public void Say()
		{
			Console.WriteLine("I am ServiceA.");
		}
	}

	public class ServiceB : IService
	{
		public void Say()
		{
			Console.WriteLine("I am ServiceB.");
		}
	}

	public class ServiceC : IService
	{
		public void Say()
		{
			Console.WriteLine("I am ServiceC.");
		}
	}

	public class Client
	{
		private readonly IEnumerable<IService> _services;

		public Client(IEnumerable<IService> services)
		{
			_services = services;
		}

		public void Execute()
		{
			Console.WriteLine("# Client");
			foreach (var service in _services)
			{
				service.Say();
			}
		}
	}

	[Factory]
	interface IFactory
	{
		ServiceA ResolveServiceA();
		ServiceB ResolveServiceB();
		ServiceC ResolveServiceC();
		IEnumerable<IService> ResolveServices();
		Client ResolveClient();
	}

	class Program
	{
		static void Main(string[] args)
		{
		}
	}
}
