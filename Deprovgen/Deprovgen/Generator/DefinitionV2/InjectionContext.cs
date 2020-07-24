using System;
using System.Collections.Generic;
using System.Text;
using Deprovgen.Utilities;

namespace Deprovgen.Generator.DefinitionV2
{
	public class InjectionContext
	{
		private Dictionary<TypeName, string> InjectionCodes { get; }

		public string? this[TypeName type]
		{
			get => InjectionCodes.TryGetValue(type, out var value) ? value : null;
			set
			{
				if (value is {})
				{
					InjectionCodes[type] = value;
				}
			}
		}

		public InjectionContext()
		{
			InjectionCodes = new Dictionary<TypeName, string>();
		}

		/// <summary>
		/// 2つの設定を合成し、それぞれに設定された依存関係の注入を全て使える新たなインスタンスを生成します。
		/// other 引数に指定された設定のほうが優先度が低くなります。
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public InjectionContext Merge(InjectionContext other)
		{
			var result = new InjectionContext();

			foreach (var injection in other.InjectionCodes)
			{
				result[injection.Key] = injection.Value;
			}

			foreach (var injection in InjectionCodes)
			{
				result[injection.Key] = injection.Value;
			}

			return result;
		}
	}
}
