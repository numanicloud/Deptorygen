using System;
using Deptorygen.Annotations;

namespace UseDeptorygen.Samples.ResolveByCapture
{	
	[Factory]
	interface IFactory
	{
		ISuperFactory Super { get; }
		[Resolution(typeof(SubFactory))]
		ISubFactory ResolveSubFactory();
	}
	
	[Factory]
	interface ISubFactory : IFactory
	{
		IFactory Factory { get; }
	}

	[Factory]
	interface ISuperFactory
	{
	}

	class SubFactory : ISubFactory
	{
		public IFactory Factory { get; }
		public ISuperFactory Super { get; }

		public SubFactory(IFactory factory, ISuperFactory super)
		{
			Factory = factory;
			Super = super;
		}
	}
}
