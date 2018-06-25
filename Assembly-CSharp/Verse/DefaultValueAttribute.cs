using System;

namespace Verse
{
	// Token: 0x02000E42 RID: 3650
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultValueAttribute : Attribute
	{
		// Token: 0x040038F3 RID: 14579
		public object value;

		// Token: 0x06005641 RID: 22081 RVA: 0x002C7A3C File Offset: 0x002C5E3C
		public DefaultValueAttribute(object value)
		{
			this.value = value;
		}

		// Token: 0x06005642 RID: 22082 RVA: 0x002C7A4C File Offset: 0x002C5E4C
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
