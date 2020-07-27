using System;
using System.Collections.Generic;

namespace Deptorygen
{
	public class DelegateComparer<T, TKey> : IEqualityComparer<T>
		where TKey : notnull
	{
		private readonly Func<T, TKey> _selector;

		public DelegateComparer(Func<T, TKey> keySelector)
		{
			// キーを指定する関数を受け取って…
			_selector = keySelector;
		}

		// 比較の際に関数を通してからEqualsやGetHashCodeをする
		public bool Equals(T x, T y)
		{
			return _selector(x).Equals(_selector(y));
		}

		public int GetHashCode(T obj)
		{
			return _selector(obj).GetHashCode();
		}
	}
}
