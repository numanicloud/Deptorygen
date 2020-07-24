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

	public class ServiceA : IService, IDisposable
	{
		public void Say()
		{
			Console.WriteLine("I am ServiceA.");
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}

	public class ServiceB : IService, IDisposable
	{
		public void Say()
		{
			Console.WriteLine("I am ServiceB.");
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}

	class ServiceC : IService, IDisposable
	{
		public void Say()
		{
			Console.WriteLine("I am ServiceC.");
		}

		public void Dispose()
		{
			throw new NotImplementedException();
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
		ServiceA ResolveServiceAAsTransient();
		ServiceB ResolveServiceB();
		ServiceC ResolveServiceC();
		[Resolution(typeof(ServiceA))]
		[Resolution(typeof(ServiceB))]
		[Resolution(typeof(ServiceC))]
		IEnumerable<IService> ResolveServices(ServiceB b);
		Client ResolveClient();
	}

	public class Client2
	{
		private readonly IEnumerable<IService> _services;

		public Client2(IEnumerable<IService> services)
		{
			_services = services;
		}
	}

	[Factory]
	interface ICapturingFactory : IFactory
	{
		IFactory Factory { get; }
		Client2 ResolveClient2();
	}

	public class Client3
	{
		private readonly ServiceA _a;
		private readonly ServiceB _b;

		public Client3(ServiceA a, ServiceB b)
		{
			_a = a;
			_b = b;
		}
	}

	class Iron : IServiceIron
	{
		private readonly ServiceA _a;

		public Iron(ServiceA a)
		{
			_a = a;
		}

		public void Tell()
		{
			throw new NotImplementedException();
		}
	}

	[Factory]
	interface ISuperFactory : ICapturingFactory
	{
		ICapturingFactory Capturing { get; }
		Client3 ResolveClient3();
		[Resolution(typeof(Iron))]
		IServiceIron ResolveServiceIron();
	}

	class Program
	{
		static void Main(string[] args)
		{
		}
	}
}
