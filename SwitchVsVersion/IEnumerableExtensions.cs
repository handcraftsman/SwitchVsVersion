using System.Collections.Generic;

namespace SwitchVsVersion
{
	public static class IEnumerableExtensions
	{
		public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items)
		{
			return new HashSet<T>(items);
		}
	}
}