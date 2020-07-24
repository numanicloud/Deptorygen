using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Deprovgen.Utilities
{
	public class TypeName : IEquatable<TypeName>
	{
		public TypeName(string fullNamespace, string name, Accessibility accessibility)
		{
			FullNamespace = fullNamespace;
			Name = name;
			Accessibility = accessibility;
		}

		public string FullNamespace { get; }
		public string Name { get; }
		public Accessibility Accessibility { get; }
		public string FullName => $"{FullNamespace}.{Name}";
		public string LowerCamelCase => Name.ToLowerCamelCase();

		public bool Equals(TypeName other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return FullNamespace == other.FullNamespace && Name == other.Name;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((TypeName) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((FullNamespace != null ? FullNamespace.GetHashCode() : 0) * 397) ^ (Name != null ? Name.GetHashCode() : 0);
			}
		}

		public TypeName ToNonInterfaceName()
		{
			var name = Name[0].ToString().Replace("I", "") + Name.Substring(1);
			return new TypeName(FullNamespace, name, Accessibility);
		}

		public static TypeName FromSymbol(ITypeSymbol symbol)
		{
			return symbol is INamedTypeSymbol nts ? FromSymbol(nts) : throw new ArgumentException();
		}

		public static TypeName FromSymbol(INamedTypeSymbol symbol)
		{
			return new TypeName(symbol.GetFullNameSpace(), symbol.Name, symbol.DeclaredAccessibility);
		}

		public static bool operator ==(TypeName lop, TypeName rop) => lop?.Equals(rop) ?? false;

		public static bool operator !=(TypeName lop, TypeName rop) => !(lop?.Equals(rop) ?? true);
	}
}
