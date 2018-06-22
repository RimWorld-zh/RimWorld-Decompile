using System;

namespace Verse
{
	// Token: 0x02000E46 RID: 3654
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class LoadAliasAttribute : Attribute
	{
		// Token: 0x06005645 RID: 22085 RVA: 0x002C7A3D File Offset: 0x002C5E3D
		public LoadAliasAttribute(string alias)
		{
			this.alias = alias;
		}

		// Token: 0x040038F4 RID: 14580
		public string alias;
	}
}
