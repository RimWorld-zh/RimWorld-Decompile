using System;

namespace Verse
{
	// Token: 0x02000E49 RID: 3657
	[AttributeUsage(AttributeTargets.Field)]
	public class DescriptionAttribute : Attribute
	{
		// Token: 0x06005628 RID: 22056 RVA: 0x002C5E5D File Offset: 0x002C425D
		public DescriptionAttribute(string description)
		{
			this.description = description;
		}

		// Token: 0x040038E8 RID: 14568
		public string description;
	}
}
