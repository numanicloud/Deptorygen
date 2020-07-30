using System;
using Deptorygen.Annotations;

namespace UseDeptorygen.Samples.Disposable
{
	class Service : IDisposable
	{
		public void Work()
		{
			Console.WriteLine("Hi its Service.");
		}

		public void Dispose()
		{
			Console.WriteLine("Disposed.");
		}
	}

	[Factory]
	interface IFactory
	{
		Service ResolveService();
	}
}