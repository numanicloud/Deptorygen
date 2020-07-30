using System;
using Deptorygen.Annotations;

namespace UseDeptorygen.Samples.Mixin
{
	class Service
	{
		public void Tell()
		{
			Console.WriteLine("Its Service.");
		}
	}

	class Service2
	{
		public void Say()
		{
			Console.WriteLine("That's Service2!");
		}
	}

	class ClientA
	{
		private readonly Service _service;

		public ClientA(Service service)
		{
			_service = service;
		}

		public void Invoke()
		{
			Console.WriteLine("# ClientA");
			_service.Tell();
		}
	}

	class ClientB
	{
		private readonly Service2 _service2;

		public ClientB(Service2 service2)
		{
			_service2 = service2;
		}

		public void Execute()
		{
			Console.WriteLine("# ClientB");
			_service2.Say();
		}
	}

	// このインターフェースは、IBaseFactoryインターフェースの定義するメソッドも含む
	[Factory]
	interface IMixinFactory : IBaseFactory
	{
		Service2 ResolveService2();
		ClientB ResolveClientB();
	}

	[Factory]
	interface IBaseFactory
	{
		Service ResolveService();
		ClientA ResolveClientA();
	}
}