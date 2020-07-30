using System;
using Deptorygen.Annotations;

namespace UseDeptorygen.Samples.Parameterize
{
	class Service
	{
		public void Say()
		{
			Console.WriteLine("It's Service!");
		}
	}

	class Client
	{
		private readonly Service _service;
		private readonly int _repeat;

		public Client(Service service, int repeat)
		{
			_service = service;
			_repeat = repeat;
		}

		public void Run()
		{
			Console.WriteLine("# Client");
			for (int i = 0; i < _repeat; i++)
			{
				_service.Say();
			}
		}
	}

	[Factory]
	interface IFactory
	{
		Service ResolveService();
		Client ResolveClient(int repeat);
	}
}