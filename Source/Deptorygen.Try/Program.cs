using System;
using System.Collections.Generic;
using Deptorygen.Annotations;
using Deptorygen.GenericHost;

namespace Deptorygen.Try
{
	class Service
	{
		public void Run()
		{
			Console.WriteLine("Hey GenericHost.");
		}
	}

	class Client
	{
		private readonly Service _service;

		public Client(Service service)
		{
			_service = service;
		}
	}

	[Factory]
	[ConfigureGenericHost]
	interface IFactory
	{
		Service ResolveService();
	}

	[Factory]
	interface ICaptureFactory
	{
		IFactory BaseFactory { get; }
		Client ResolveClient();
	}

	class Program
	{
		static void Main(string[] args)
		{
		}
	}
}