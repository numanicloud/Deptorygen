using Deprovgen.Utilities;

namespace Deprovgen.Generator.Definition
{
	public class VariableDefinition
	{
		public TypeName TypeNameInfo { get; set; }
		public string TypeName => TypeNameInfo.Name;
		public string VarName { get; }
		public string TypeNamespace => TypeNameInfo.FullNamespace;
		public string Code => $"{TypeName} {VarName}";

		public VariableDefinition(TypeName typeNameInfo, string varName)
		{
			TypeNameInfo = typeNameInfo;
			VarName = varName;
		}
	}
}
