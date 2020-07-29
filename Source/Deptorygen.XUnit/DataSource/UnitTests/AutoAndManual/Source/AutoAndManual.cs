using System;
using Deptorygen.Annotations;

namespace UseDeptorygen.Samples.AutoAndManual
{
	// こちらはファクトリーで生成してもらう予定
	class ServiceSilver
	{
		public void Tell()
		{
			Console.WriteLine("I'm Silver.");
		}
	}

	// こちらはファクトリー外で自前で生成する予定
	class ServiceGold
	{
		public void Say()
		{
			Console.WriteLine("I'm Gold.");
		}
	}

	class Client
	{
		private readonly ServiceSilver _silver;
		private readonly ServiceGold _gold;

		public Client(ServiceSilver silver, ServiceGold gold)
		{
			_silver = silver;
			_gold = gold;
		}

		public void Invoke()
		{
			Console.WriteLine("# Client");
			_silver.Tell();
			_gold.Say();
		}
	}

	[Factory]
	interface IFactory
	{
		ServiceSilver ResolveServiceSilver();
		Client ResolveClient();
	}
}