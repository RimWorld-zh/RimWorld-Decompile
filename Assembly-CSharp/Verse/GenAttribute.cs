using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Verse
{
	public static class GenAttribute
	{
		public static bool HasAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T val = default(T);
			return memberInfo.TryGetAttribute<T>(out val);
		}

		public static bool TryGetAttribute<T>(this MemberInfo memberInfo, out T customAttribute) where T : Attribute
		{
			object obj = ((IEnumerable<object>)memberInfo.GetCustomAttributes(typeof(T), true)).FirstOrDefault<object>();
			bool result;
			if (obj == null)
			{
				customAttribute = (T)null;
				result = false;
			}
			else
			{
				customAttribute = (T)obj;
				result = true;
			}
			return result;
		}
	}
}
