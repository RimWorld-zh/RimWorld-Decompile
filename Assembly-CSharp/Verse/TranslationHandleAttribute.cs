using System;

namespace Verse
{
	// Token: 0x02000E3F RID: 3647
	[AttributeUsage(AttributeTargets.Field)]
	public class TranslationHandleAttribute : Attribute
	{
		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x0600563D RID: 22077 RVA: 0x002C79E4 File Offset: 0x002C5DE4
		// (set) Token: 0x0600563E RID: 22078 RVA: 0x002C79FE File Offset: 0x002C5DFE
		public int Priority { get; set; }
	}
}
