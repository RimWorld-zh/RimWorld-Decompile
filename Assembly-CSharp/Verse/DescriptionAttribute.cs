using System;

namespace Verse
{
	// Token: 0x02000E47 RID: 3655
	[AttributeUsage(AttributeTargets.Field)]
	public class DescriptionAttribute : Attribute
	{
		// Token: 0x040038F5 RID: 14581
		public string description;

		// Token: 0x06005646 RID: 22086 RVA: 0x002C7A4D File Offset: 0x002C5E4D
		public DescriptionAttribute(string description)
		{
			this.description = description;
		}
	}
}
