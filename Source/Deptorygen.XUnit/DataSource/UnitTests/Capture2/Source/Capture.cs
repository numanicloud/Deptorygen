using System;
using Deptorygen.Annotations;

namespace UseDeptorygen.Samples.Capture
{
	class Service
	{
		public void Call()
		{
			Console.WriteLine("Its Service.");
		}
	}

	class Service2
	{
		public void Say()
		{
			Console.WriteLine("Its Service2!");
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

		public void Run()
		{
			Console.WriteLine("# Client");
			_service.Call();
			_service2.Say();
		}
	}

	[Factory]
	interface IBaseFactory
	{
		Service ResolveService();
		[Resolution(typeof(CaptureFactory))]
		ICaptureFactory ResolveCaptureFactory(Service2 service2);
	}

	[Factory]
	interface ICaptureFactory
	{
		// ここでファクトリー定義の IBaseFactory を"キャプチャ"する
		IBaseFactory BaseFactory { get; }
		Client ResolveClient();
	}
}