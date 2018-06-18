using System;

namespace Verse
{
	// Token: 0x02000E33 RID: 3635
	[AttributeUsage(AttributeTargets.Method)]
	public class CategoryAttribute : Attribute
	{
		// Token: 0x060055F3 RID: 22003 RVA: 0x002C4318 File Offset: 0x002C2718
		public CategoryAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x040038C9 RID: 14537
		public string name;
	}
}
