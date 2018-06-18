using System;

namespace Verse
{
	// Token: 0x02000E47 RID: 3655
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class LoadAliasAttribute : Attribute
	{
		// Token: 0x06005625 RID: 22053 RVA: 0x002C5E4D File Offset: 0x002C424D
		public LoadAliasAttribute(string alias)
		{
			this.alias = alias;
		}

		// Token: 0x040038E5 RID: 14565
		public string alias;
	}
}
