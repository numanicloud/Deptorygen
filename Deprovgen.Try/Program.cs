using System;
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
	[Factory]
	[FactoryProvider(typeof(ServiceLocator))]
	interface IServiceLocatorFuga
	{
		Fuga ResolveFuga();
		[Implementation(typeof(Iron))]
		IServiceIron ResolveIron(Service3 service3);
	}

	interface IIronServiceLocator
	{
		IServiceIron ResolveIron(Service3 service3);
	}

	[Factory]
	interface ICapturedFactory
	{
		Service ResolveService();
	}

	[Factory]
	[ConfigureGenericHost]
	interface IServiceLocator
	{
		ICapturedFactory Captured { get; }

		Hoge ResolveHoge();
		Piyo ResolvePiyo();
	}

	internal partial class ServiceLocatorFuga : IIronServiceLocator
	{
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

	public class Iron : IServiceIron
	{
		private readonly Service3 _service3;

		public Iron(Service3 service3)
		{
			_service3 = service3;
		}

		public void Tell()
		{
			Console.WriteLine("# Iron");
			_service3.Say();
		}
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

			Console.WriteLine("## GenericHost");

			var services = new ServiceCollection();
			services.UseDeprovgenFactory(serviceLocator2);

			var provider = services.BuildServiceProvider();
			provider.GetService<Fuga>().Invoke();
			provider.GetService<Piyo>().Do();
		}
	}
}
