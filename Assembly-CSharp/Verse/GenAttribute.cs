using System;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000E3B RID: 3643
	public static class GenAttribute
	{
		// Token: 0x06005615 RID: 22037 RVA: 0x002C5C00 File Offset: 0x002C4000
		public static bool HasAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T t;
			return memberInfo.TryGetAttribute(out t);
		}

		// Token: 0x06005616 RID: 22038 RVA: 0x002C5C20 File Offset: 0x002C4020
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

		// Token: 0x06005617 RID: 22039 RVA: 0x002C5CAC File Offset: 0x002C40AC
		public static T TryGetAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T result = (T)((object)null);
			memberInfo.TryGetAttribute(out result);
			return result;
		}
	}
}
