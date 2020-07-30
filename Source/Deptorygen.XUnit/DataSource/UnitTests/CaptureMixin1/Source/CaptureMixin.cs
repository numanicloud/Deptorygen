using Deptorygen.Annotations;
using System;

namespace UseDeptorygen.Samples.CaptureMixin
{
	class Service
	{
		public void Write()
		{
			Console.WriteLine("It's Service!");
		}
	}

	class Client
	{
		private readonly Service _service;

		public Client(Service service)
		{
			_service = service;
		}

		public void Invoke()
		{
			Console.WriteLine("# Client");
			_service.Write();
		}
	}

	[Factory]
	interface ICaptureFactory : IBaseFactory
	{
		IBaseFactory BaseFactory { get; }
		Client ResolveClient();
	}

	[Factory]
	interface IBaseFactory
	{
		Service ResolveService();
		[Resolution(typeof(CaptureFactory))]
		ICaptureFactory ResolveCaptureFactory();
	}
}