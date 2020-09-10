using Deptorygen.Annotations;

namespace Deptorygen.XUnit.DataSource.UnitTests.ResolverDelegation.Source
{
	interface IServiceApi
	{
	}

	class Service : IServiceApi
	{
	}

	[Factory]
	interface IFactory
	{
		Service ResolveService();
		[Delegation(nameof(ResolveService))]
		IServiceApi ResolveServiceApi();
	}
}
