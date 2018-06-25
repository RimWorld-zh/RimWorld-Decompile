using System;

namespace Verse
{
	// Token: 0x02000E48 RID: 3656
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class LoadAliasAttribute : Attribute
	{
		// Token: 0x040038F4 RID: 14580
		public string alias;

		// Token: 0x06005649 RID: 22089 RVA: 0x002C7B69 File Offset: 0x002C5F69
		public LoadAliasAttribute(string alias)
		{
			this.alias = alias;
		}
	}
}
