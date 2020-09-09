using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Deptorygen.Generator.Interfaces
{
	public interface IAccessibilityClaimer
	{
		public IEnumerable<Accessibility> Accessibilities { get; }
	}
}
