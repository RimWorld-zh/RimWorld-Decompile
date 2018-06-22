using System;

namespace Verse
{
	// Token: 0x02000E30 RID: 3632
	[AttributeUsage(AttributeTargets.Method)]
	public class CategoryAttribute : Attribute
	{
		// Token: 0x0600560F RID: 22031 RVA: 0x002C5ED4 File Offset: 0x002C42D4
		public CategoryAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x040038D7 RID: 14551
		public string name;
	}
}
