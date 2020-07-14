namespace Deprovgen.Generator.Domains
{
	public class VariableDefinition
	{
		public string TypeName { get; }
		public string VarName { get; }
		public string TypeNamespace { get; }
		public string Code { get; }

		public VariableDefinition(string typeName, string varName, string typeNamespace)
		{
			TypeName = typeName;
			VarName = varName;
			TypeNamespace = typeNamespace;
			Code = $"{typeName} {varName}";
		}
	}
}
