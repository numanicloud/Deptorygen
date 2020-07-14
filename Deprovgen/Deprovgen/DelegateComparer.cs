using System;
using System.Collections.Generic;

namespace Deprovgen
{
	public class DelegateComparer<T, TKey> : IEqualityComparer<T>
	{
		private readonly Func<T, TKey> _selector;

		public DelegateComparer(Func<T, TKey> keySelector)
		{
			// キーを指定する関数を受け取って…
			_selector = keySelector;
		}

		// 比較の際に関数を通してからEqualsやGetHashCodeをする
		public bool Equals(T x, T y) => _selector(x).Equals(_selector(y));

		public int GetHashCode(T obj) => _selector(obj).GetHashCode();
	}
}
