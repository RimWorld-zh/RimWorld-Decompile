using System;

namespace Verse
{
	// Token: 0x02000E34 RID: 3636
	[AttributeUsage(AttributeTargets.Method)]
	public class CategoryAttribute : Attribute
	{
		// Token: 0x060055F5 RID: 22005 RVA: 0x002C4318 File Offset: 0x002C2718
		public CategoryAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x040038CB RID: 14539
		public string name;
	}
}
