using System;

namespace Verse
{
	// Token: 0x02000E49 RID: 3657
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class LoadAliasAttribute : Attribute
	{
		// Token: 0x040038FC RID: 14588
		public string alias;

		// Token: 0x06005649 RID: 22089 RVA: 0x002C7D55 File Offset: 0x002C6155
		public LoadAliasAttribute(string alias)
		{
			this.alias = alias;
		}
	}
}
