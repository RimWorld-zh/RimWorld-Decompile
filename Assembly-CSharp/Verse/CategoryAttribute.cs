using System;

namespace Verse
{
	// Token: 0x02000E32 RID: 3634
	[AttributeUsage(AttributeTargets.Method)]
	public class CategoryAttribute : Attribute
	{
		// Token: 0x040038D7 RID: 14551
		public string name;

		// Token: 0x06005613 RID: 22035 RVA: 0x002C6000 File Offset: 0x002C4400
		public CategoryAttribute(string name)
		{
			this.name = name;
		}
	}
}
