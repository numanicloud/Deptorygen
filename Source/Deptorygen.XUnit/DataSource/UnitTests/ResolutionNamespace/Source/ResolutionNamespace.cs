using System;
using Deptorygen.Annotations;

namespace UseDeptorygen.Samples.UsingNamespace.Another
{
	class Service : IService
	{
	}
}

namespace UseDeptorygen.Samples.UsingNamespace
{
	interface IService
	{
	}

	[Factory]
	interface IFactory
	{
		[Resolution(typeof(Another.Service))]
		IService ResolveService();
	}
}
