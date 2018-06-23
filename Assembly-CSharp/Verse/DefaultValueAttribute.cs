using System;

namespace Verse
{
	// Token: 0x02000E40 RID: 3648
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultValueAttribute : Attribute
	{
		// Token: 0x040038F3 RID: 14579
		public object value;

		// Token: 0x0600563D RID: 22077 RVA: 0x002C7910 File Offset: 0x002C5D10
		public DefaultValueAttribute(object value)
		{
			this.value = value;
		}

		// Token: 0x0600563E RID: 22078 RVA: 0x002C7920 File Offset: 0x002C5D20
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
	}
}
