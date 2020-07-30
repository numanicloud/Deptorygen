using System;
using Deptorygen.Annotations;

namespace UseDeptorygen.Samples.UseCache
{
	// このクラスが依存関係解決時にキャッシュされているか確かめたい
	class Service
	{
		private static int NextId = 0;

		private int Id { get; }

		public Service()
		{
			Id = NextId++;
		}

		public void Invoke()
		{
			Console.WriteLine($"This is Service #{Id}.");
		}
	}

	class Client
	{
		private readonly Service _serviceA;
		private readonly Service _serviceB;

		public Client(Service serviceA, Service serviceB)
		{
			_serviceA = serviceA;
			_serviceB = serviceB;
		}

		public void Say()
		{
			// キャッシュされていれば、2つのオブジェクトは同じIDを表示するはず
			_serviceA.Invoke();
			_serviceB.Invoke();
		}
	}

	// ファクトリー定義
	[Factory]
	interface IFactory
	{
		// Serviceクラスをファクトリーに定義しておかないとコンストラクタで要求されてしまう
		// その場合キャッシュと同じ効果はあるが、ここでは定義した場合のキャッシュを確かめたい
		Service ResolveService();
		Client ResolveClient();
	}
}