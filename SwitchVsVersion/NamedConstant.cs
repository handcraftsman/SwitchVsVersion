using System.Collections.Generic;
using System.Reflection;

namespace SwitchVsVersion
{
	public abstract class NamedConstant<T>
		where T : NamedConstant<T>
	{
		private static readonly Dictionary<string, T> Mappings = new Dictionary<string, T>();

		protected void Add(string key, T value)
		{
			Mappings.Add(key.ToLower(), value);
		}

		private static void EnsureValues()
		{
			if (Mappings.Count != 0)
			{
				return;
			}
			var fieldInfos = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public);
			// ensure its static members get created by triggering the type initializer
			fieldInfos[0].GetValue(null);
		}

		public static IEnumerable<T> GetAll()
		{
			EnsureValues();
			return Mappings.Values;
		}

		protected T GetFor(string key)
		{
			T item;
			return !Mappings.TryGetValue(key.ToLower(), out item) ? null : item;
		}
	}
}