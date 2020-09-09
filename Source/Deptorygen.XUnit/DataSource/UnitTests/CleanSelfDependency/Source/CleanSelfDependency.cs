using System;
using Deptorygen.Annotations;

namespace Deptorygen.XUnit.DataSource.UnitTests.CleanSelfDependency.Source
{
	class Service1
	{
	}

	class Service2
	{
	}

	[Factory]
	interface IFactory
	{
		Service1 ResolveService1AsTransient();
		[Resolution(typeof(Factory))]
		IFactory ResolveFactoryAsTransient();
	}
}
