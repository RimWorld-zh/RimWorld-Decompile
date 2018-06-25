using System;
using System.Reflection;

namespace Verse
{
	public static class GenAttribute
	{
		public static bool HasAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T t;
			return memberInfo.TryGetAttribute(out t);
		}

		public static bool TryGetAttribute<T>(this MemberInfo memberInfo, out T customAttribute) where T : Attribute
		{
			object[] customAttributes = memberInfo.GetCustomAttributes(typeof(T), true);
			bool result;
			if (customAttributes.Length == 0)
			{
				customAttribute = (T)((object)null);
				result = false;
			}
			else
			{
				for (int i = 0; i < customAttributes.Length; i++)
				{
					if (customAttributes[i] is T)
					{
						customAttribute = (T)((object)customAttributes[i]);
						return true;
					}
				}
				customAttribute = (T)((object)null);
				result = false;
			}
			return result;
		}

		public static T TryGetAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T result = (T)((object)null);
			memberInfo.TryGetAttribute(out result);
			return result;
		}
	}
}
