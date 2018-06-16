using System;

namespace Verse
{
	// Token: 0x02000E48 RID: 3656
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class LoadAliasAttribute : Attribute
	{
		// Token: 0x06005627 RID: 22055 RVA: 0x002C5E4D File Offset: 0x002C424D
		public LoadAliasAttribute(string alias)
		{
			this.alias = alias;
		}

		// Token: 0x040038E7 RID: 14567
		public string alias;
	}
}
