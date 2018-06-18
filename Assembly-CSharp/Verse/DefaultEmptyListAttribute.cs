using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E43 RID: 3651
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultEmptyListAttribute : DefaultValueAttribute
	{
		// Token: 0x06005620 RID: 22048 RVA: 0x002C5D8E File Offset: 0x002C418E
		public DefaultEmptyListAttribute(Type type) : base(type)
		{
		}

		// Token: 0x06005621 RID: 22049 RVA: 0x002C5D98 File Offset: 0x002C4198
		public override bool ObjIsDefault(object obj)
		{
			bool result;
			if (obj == null)
			{
				result = false;
			}
			else if (obj.GetType().GetGenericTypeDefinition() != typeof(List<>))
			{
				result = false;
			}
			else
			{
				Type[] genericArguments = obj.GetType().GetGenericArguments();
				if (genericArguments.Length != 1 || genericArguments[0] != (Type)this.value)
				{
					result = false;
				}
				else
				{
					int num = (int)obj.GetType().GetProperty("Count").GetValue(obj, null);
					result = (num == 0);
				}
			}
			return result;
		}
	}
}
