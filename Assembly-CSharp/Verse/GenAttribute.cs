using System;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000E38 RID: 3640
	public static class GenAttribute
	{
		// Token: 0x06005631 RID: 22065 RVA: 0x002C77BC File Offset: 0x002C5BBC
		public static bool HasAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T t;
			return memberInfo.TryGetAttribute(out t);
		}

		// Token: 0x06005632 RID: 22066 RVA: 0x002C77DC File Offset: 0x002C5BDC
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

		// Token: 0x06005633 RID: 22067 RVA: 0x002C7868 File Offset: 0x002C5C68
		public static T TryGetAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T result = (T)((object)null);
			memberInfo.TryGetAttribute(out result);
			return result;
		}
	}
}
