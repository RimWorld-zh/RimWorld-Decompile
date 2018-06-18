using System;

namespace Verse
{
	// Token: 0x02000E48 RID: 3656
	[AttributeUsage(AttributeTargets.Field)]
	public class DescriptionAttribute : Attribute
	{
		// Token: 0x06005626 RID: 22054 RVA: 0x002C5E5D File Offset: 0x002C425D
		public DescriptionAttribute(string description)
		{
			this.description = description;
		}

		// Token: 0x040038E6 RID: 14566
		public string description;
	}
}
