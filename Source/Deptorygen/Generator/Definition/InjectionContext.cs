using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deptorygen.Exceptions;
using Deptorygen.Generator.Interfaces;
using Deptorygen.Utilities;

namespace Deptorygen.Generator.Definition
{
	public class InjectionContext
	{
		private readonly IInjectionGenerator[] _generators_refactor;

		private readonly IEnumerable<IInjectionGenerator> _generators;
		private readonly Dictionary<TypeName, IInjectionGenerator> _generatorTable;

		public InjectionContext(IEnumerable<IInjectionGenerator> generators)
		{
			_generatorTable = new Dictionary<TypeName, IInjectionGenerator>();
			_generators_refactor = generators.ToArray();
			_generators = _generators_refactor;
		}

		public string GetExpression_refactor(TypeName typeName, InjectionContext parent)
		{
			var expressions = new List<InjectionExpression>();
			foreach (var provider in _generators_refactor)
			{
				expressions.AddRange(provider.GetInjectionExpressions(typeName, parent));
			}

			return expressions.OrderBy(x => x.Type).First().Code;
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

			throw new CannotResolveException($"{typeName} のインスタンスを解決する手段がありませんでした。")
			{
				TargetType = typeName,
			};
		}
	}
}
