using System;

namespace Deprovgen.Try
{
	[AttributeUsage(AttributeTargets.Interface)]
	class MyFactoryAttribute : Attribute
	{
	}

	[MyFactory]
	interface IServiceLocator
	{
		Hoge ResolveHoge();
	}

	class Service
	{
		
	}

	class Hoge
	{
		public Hoge(Service service)
		{
			
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
		}
	}
}
