using Deptorygen.Annotations;

namespace Deptorygen.XUnit.DataSource.UnitTests.DependencyDelegation.Source
{
	class Service
	{
	}

	class Client
	{
		public Client(Service service)
		{
		}
	}

	[Factory]
	interface IMixinFactory : IBaseFactory
	{
		IBaseFactory BaseFactory { get; }
	}

	[Factory]
	interface IBaseFactory
	{
		Client ResolveClient();
		[Resolution(typeof(MixinFactory))]
		IMixinFactory ResolveMixinFactory();
	}
}
