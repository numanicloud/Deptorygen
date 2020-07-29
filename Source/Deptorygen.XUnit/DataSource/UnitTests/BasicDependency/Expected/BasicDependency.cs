using System;
using Deptorygen.Annotations;

namespace UseDeptorygen.Samples.BasicDependency
{
	class Service
	{
		public void Show()
		{
			Console.WriteLine("This is Service!");
		}
	}

	class Client
	{
		private readonly Service _service;

		public Client(Service service)
		{
			_service = service;
		}

		public void Execute()
		{
			Console.WriteLine("# Client");
			_service.Show();
		}
	}

	[Factory]
	interface IFactory
	{
		// Serviceクラスを解決してもらいたい
		Service ResolveService();
		// ClientクラスはServiceクラスに依存している。
		// これも解決してもらいたい
		Client ResolveClient();
	}
}