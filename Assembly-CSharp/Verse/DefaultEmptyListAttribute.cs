using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E42 RID: 3650
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultEmptyListAttribute : DefaultValueAttribute
	{
		// Token: 0x06005640 RID: 22080 RVA: 0x002C797E File Offset: 0x002C5D7E
		public DefaultEmptyListAttribute(Type type) : base(type)
		{
		}

		// Token: 0x06005641 RID: 22081 RVA: 0x002C7988 File Offset: 0x002C5D88
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
