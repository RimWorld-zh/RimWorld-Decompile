using System;

namespace Verse
{
	// Token: 0x02000E43 RID: 3651
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultValueAttribute : Attribute
	{
		// Token: 0x040038FB RID: 14587
		public object value;

		// Token: 0x06005641 RID: 22081 RVA: 0x002C7C28 File Offset: 0x002C6028
		public DefaultValueAttribute(object value)
		{
			this.value = value;
		}

		// Token: 0x06005642 RID: 22082 RVA: 0x002C7C38 File Offset: 0x002C6038
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
