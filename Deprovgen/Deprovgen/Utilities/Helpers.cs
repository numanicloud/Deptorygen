using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Deprovgen
{
	public static class Helpers
	{
		public static string Join(this IEnumerable<string> source, string delimiter)
		{
			return string.Join(delimiter, source);
		}

		public static Task<TResult[]> WhenAll<TResult>(this IEnumerable<Task<TResult>> source)
		{
			return Task.WhenAll(source);
		}

		public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
		{
			return source.Distinct(new DelegateComparer<T, TKey>(selector));
		}

		public static string GetFullNameSpace(this ITypeSymbol typeSymbol)
		{
			IEnumerable<string> GetFullNameSpace(INamespaceSymbol nss)
			{
				if (nss.IsGlobalNamespace)
				{
					yield break;
				}

				var ns = nss.ContainingNamespace;
				foreach (var part in GetFullNameSpace(ns))
				{
					yield return part;
				}

				yield return nss.Name;
			}

			return GetFullNameSpace(typeSymbol.ContainingNamespace).Join(".");
		}

		public static async Task<TResult[]> ToArrayAsync<TResult>(this IAsyncEnumerable<TResult> source, CancellationToken ct = default)
		{
			var list = new List<TResult>();
			await foreach (var item in source.WithCancellation(ct))
			{
				list.Add(item);
			}

			return list.ToArray();
		}

		public static bool IsStructualEqual<T>(this IEnumerable<T> source, IEnumerable<T> second)
			where T : notnull
		{
			using var enumerator1 = source.GetEnumerator();
			using var enumerator2 = second.GetEnumerator();

			while (true)
			{
				var end1 = !enumerator1.MoveNext();
				var end2 = !enumerator2.MoveNext();

				if (end1 && end2)
				{
					return true;
				}

				if (end1 != end2 || !enumerator1.Current.Equals(enumerator2.Current))
				{
					return false;
				}
			}
		}

		public static bool HasAttribute(this INamedTypeSymbol symbol, string attributeName)
		{
			return symbol.GetAttributes()
				.Any(attr => attr.AttributeClass.Name == attributeName);
		}

		public static string ToLowerCamelCase(this string name)
		{
			return name[0].ToString().ToLower() + name.Substring(1);
		}
	}
}
