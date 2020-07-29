using System;
using Deptorygen.Annotations;

namespace UseDeptorygen.Samples.Basic
{
	// Clientクラスにこのクラスを注入したい
	class Service
	{
		public void Show()
		{
			Console.WriteLine("This is Service!");
		}
	}

	// Serviceクラスをこのクラスへ注入したい
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

	// ファクトリーの定義。これをタネにDeptorygenの生成を走らせます
	[Factory]
	interface IFactory
	{
		Client ResolveClient();
	}
}