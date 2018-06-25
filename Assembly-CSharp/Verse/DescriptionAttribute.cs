using System;

namespace Verse
{
	// Token: 0x02000E49 RID: 3657
	[AttributeUsage(AttributeTargets.Field)]
	public class DescriptionAttribute : Attribute
	{
		// Token: 0x040038F5 RID: 14581
		public string description;

		// Token: 0x0600564A RID: 22090 RVA: 0x002C7B79 File Offset: 0x002C5F79
		public DescriptionAttribute(string description)
		{
			this.description = description;
		}
	}
}
