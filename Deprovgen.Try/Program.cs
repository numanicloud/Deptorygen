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
	interface IServiceLocator : IServiceLocatorFuga
	{
		Hoge ResolveHoge();
		Piyo ResolvePiyo();
	}

	public class Service
	{
		public void Run() => Console.WriteLine("I'm Service!");
	}

	public class Service2
	{
		public void Execute() => Console.WriteLine("It's Service2!");
	}

	public class Service3
	{
		public void Say() => Console.WriteLine("Yes its Service3.");
	}

	class Fuga
	{
		private readonly Service _service;
		private readonly Service3 _service3;

		public Fuga(Service service, Service3 service3)
		{
			_service = service;
			_service3 = service3;
		}

		public void Invoke()
		{
			Console.WriteLine("# Fuga");
			_service.Run();
			_service3.Say();
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

	public class Piyo
	{
		private readonly Hoge _hoge;
		private readonly Service _service;

		public Piyo(Hoge hoge, Service service)
		{
			_hoge = hoge;
			_service = service;
		}

		public void Do()
		{
			Console.WriteLine("# Piyo");
			_service.Run();
			_hoge.Show();
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			var serviceLocator = new ServiceLocatorFuga(new Service(), new Service3());
			serviceLocator.ResolveFuga().Invoke();

			var serviceLocator2 = serviceLocator.ResolveServiceLocator(new Service2());
			serviceLocator2.ResolveHoge().Show();
		}
	}
}
