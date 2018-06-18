using System;

namespace Verse
{
	// Token: 0x02000E41 RID: 3649
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultValueAttribute : Attribute
	{
		// Token: 0x0600561D RID: 22045 RVA: 0x002C5D1F File Offset: 0x002C411F
		public DefaultValueAttribute(object value)
		{
			this.value = value;
		}

		// Token: 0x0600561E RID: 22046 RVA: 0x002C5D30 File Offset: 0x002C4130
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

		// Token: 0x040038E4 RID: 14564
		public object value;
	}
}
