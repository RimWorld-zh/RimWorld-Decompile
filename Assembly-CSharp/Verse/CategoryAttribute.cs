using System;

namespace Verse
{
	// Token: 0x02000E33 RID: 3635
	[AttributeUsage(AttributeTargets.Method)]
	public class CategoryAttribute : Attribute
	{
		// Token: 0x040038DF RID: 14559
		public string name;

		// Token: 0x06005613 RID: 22035 RVA: 0x002C61EC File Offset: 0x002C45EC
		public CategoryAttribute(string name)
		{
			this.name = name;
		}
	}
}
