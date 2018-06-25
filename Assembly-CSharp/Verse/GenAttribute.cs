using System;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000E3A RID: 3642
	public static class GenAttribute
	{
		// Token: 0x06005635 RID: 22069 RVA: 0x002C78E8 File Offset: 0x002C5CE8
		public static bool HasAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T t;
			return memberInfo.TryGetAttribute(out t);
		}

		// Token: 0x06005636 RID: 22070 RVA: 0x002C7908 File Offset: 0x002C5D08
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

		// Token: 0x06005637 RID: 22071 RVA: 0x002C7994 File Offset: 0x002C5D94
		public static T TryGetAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T result = (T)((object)null);
			memberInfo.TryGetAttribute(out result);
			return result;
		}
	}
}
