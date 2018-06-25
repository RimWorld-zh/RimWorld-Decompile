using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E45 RID: 3653
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultEmptyListAttribute : DefaultValueAttribute
	{
		// Token: 0x06005644 RID: 22084 RVA: 0x002C7C96 File Offset: 0x002C6096
		public DefaultEmptyListAttribute(Type type) : base(type)
		{
		}

		// Token: 0x06005645 RID: 22085 RVA: 0x002C7CA0 File Offset: 0x002C60A0
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
