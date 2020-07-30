using System;
using Deptorygen.Annotations;

namespace UseDeptorygen.Samples.UsingNamespace.Another
{
	class Service
	{
	}
}

namespace UseDeptorygen.Samples.UsingNamespace
{
	[Factory]
	interface IFactory
	{
		Another.Service ResolveService();
	}
}
