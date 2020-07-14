using System;
using Deprovgen.Annotations;

namespace Deprovgen.Try
{
	[Factory]
	[FactoryProvider(typeof(ServiceLocator))]
	interface IServiceLocatorFuga
	{
		Fuga ResolveFuga();
	}

	[Factory]
	interface IServiceLocator
	{
		Hoge ResolveHoge();
	}

	public class Service
	{
		public void Run() => Console.WriteLine("I'm Service!");
	}

	public class Service2
	{
		public void Execute() => Console.WriteLine("It's Service2!");
	}

	class Fuga
	{
		private readonly Service _service;

		public Fuga(Service service)
		{
			_service = service;
		}

		public void Invoke()
		{
			Console.WriteLine("# Fuga");
			_service.Run();
			_service.Run();
		}
	}

	public class Hoge
	{
		private readonly Service _service;
		private readonly Service2 _service2;

		public Hoge(Service service, Service2 service2)
		{
			_service = service;
			_service2 = service2;
		}

		public void Show()
		{
			Console.WriteLine("# Hoge");
			_service.Run();
			_service2.Execute();
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			var serviceLocator = new ServiceLocatorFuga(new Service());
			serviceLocator.ResolveFuga().Invoke();

			var serviceLocator2 = serviceLocator.ResolveServiceLocator(new Service2());
			serviceLocator2.ResolveHoge().Show();
		}
	}
}
