using System;
using System.Collections.Generic;
using Deptorygen.Annotations;

namespace Deptorygen.Try
{
	class Service
	{
		public void Run()
		{
			Console.WriteLine("Hey GenericHost.");
		}
	}

	[Factory]
	[ConfigureGenericHost]
	interface IFactory
	{
		Service ResolveService();
	}

	class Program
	{
		static void Main(string[] args)
		{
		}
	}
}