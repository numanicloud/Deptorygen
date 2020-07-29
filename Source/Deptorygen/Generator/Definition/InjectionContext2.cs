using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deptorygen.Generator.Interfaces;
using Deptorygen.Utilities;

namespace Deptorygen.Generator.Definition
{
	public class InjectionContext2
	{
		private readonly IEnumerable<IInjectionGenerator> _generators;
		private readonly Dictionary<TypeName, IInjectionGenerator> _generatorTable;

		public InjectionContext2(IEnumerable<IInjectionGenerator> generators)
		{
			_generators = generators;
			_generatorTable = new Dictionary<TypeName, IInjectionGenerator>();
		}

		public string GetExpression(TypeName typeName)
		{
			if (_generatorTable.TryGetValue(typeName, out var match))
			{
				if (match.GetInjectionExpression(typeName, this) is {} exp)
				{
					return exp;
				}
				_generatorTable.Remove(typeName);
			}

			foreach (var generator in _generators)
			{
				if (generator.GetInjectionExpression(typeName, this) is {} exp)
				{
					return exp;
				}
			}

			throw new InvalidOperationException();
		}

		public InjectionContext2 Merge(InjectionContext2 other)
		{
			var generators = _generators.Concat(other._generators);
			var result = new InjectionContext2(generators);

			foreach (var g in _generatorTable)
			{
				result._generatorTable[g.Key] = g.Value;
			}

			foreach (var g in other._generatorTable)
			{
				result._generatorTable[g.Key] = g.Value;
			}

			return result;
		}
	}
}
