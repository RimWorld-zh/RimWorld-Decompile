using System;

namespace Verse
{
	// Token: 0x02000E42 RID: 3650
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultValueAttribute : Attribute
	{
		// Token: 0x0600561F RID: 22047 RVA: 0x002C5D1F File Offset: 0x002C411F
		public DefaultValueAttribute(object value)
		{
			this.value = value;
		}

		// Token: 0x06005620 RID: 22048 RVA: 0x002C5D30 File Offset: 0x002C4130
		public virtual bool ObjIsDefault(object obj)
		{
			bool result;
			if (obj == null)
			{
				result = (this.value == null);
			}
			else
			{
				result = (this.value != null && this.value.Equals(obj));
			}
			return result;
		}

		// Token: 0x040038E6 RID: 14566
		public object value;
	}
}
