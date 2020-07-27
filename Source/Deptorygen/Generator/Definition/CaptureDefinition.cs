using Deptorygen.Utilities;

namespace Deptorygen.Generator.Definition
{
	public class CaptureDefinition
	{
		public TypeName InterfaceNameInfo { get; }
		public ResolverDefinition[] Resolvers { get; }
		public CollectionResolverDefinition[] CollectionResolvers { get; }
		public InjectionContext Injection { get; }
		public string PropertyName { get; }

		public string InterfaceName => InterfaceNameInfo.Name;
		public string ParameterName => PropertyName.ToLowerCamelCase();

		public CaptureDefinition(TypeName interfaceNameInfo,
			string propertyName,
			ResolverDefinition[] resolvers,
			CollectionResolverDefinition[] collectionResolvers)
		{
			InterfaceNameInfo = interfaceNameInfo;
			PropertyName = propertyName;
			Resolvers = resolvers;
			CollectionResolvers = collectionResolvers;

			Injection = new InjectionContext();
			foreach (var resolver in Resolvers)
			{
				Injection[resolver.ReturnType] = $"{propertyName}.{resolver.MethodName}({resolver.GetArgsListForSelf(Injection)})";
			}

			foreach (var resolver in collectionResolvers)
			{
				Injection[resolver.ReturnType] = $"{propertyName}.{resolver.MethodName}({resolver.GetArgListForSelf(Injection)})";
			}
		}
	}
}
