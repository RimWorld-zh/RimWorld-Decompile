using System;

namespace Verse
{
	// Token: 0x02000E4A RID: 3658
	[AttributeUsage(AttributeTargets.Field)]
	public class DescriptionAttribute : Attribute
	{
		// Token: 0x040038FD RID: 14589
		public string description;

		// Token: 0x0600564A RID: 22090 RVA: 0x002C7D65 File Offset: 0x002C6165
		public DescriptionAttribute(string description)
		{
			this.description = description;
		}
	}
}
