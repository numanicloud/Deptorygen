using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Deptorygen.Generator.Interfaces
{
	public interface IAccessibilityClaimer
	{
		public IEnumerable<Accessibility> Accessibilities { get; }
	}
}
