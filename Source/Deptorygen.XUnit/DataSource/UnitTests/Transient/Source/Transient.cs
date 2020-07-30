using System;
using Deptorygen.Annotations;

namespace UseDeptorygen.Samples.Transient
{
	// このクラスが依存関係解決時にキャッシュされないことを確かめたい
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
			// キャッシュされていなければ、2つのオブジェクトは互いに異なるIDを表示するはず
			_serviceA.Invoke();
			_serviceB.Invoke();
		}
	}

	// ファクトリー定義
	[Factory]
	interface IFactory
	{
		// 解決メソッドの名前の末尾を "AsTransient" にするとキャッシュしないようになる
		Service ResolveServiceAsTransient();
		Client ResolveClient();
	}
}