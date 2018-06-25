using System;

namespace Verse
{
	// Token: 0x02000E40 RID: 3648
	[AttributeUsage(AttributeTargets.Field)]
	public class TranslationHandleAttribute : Attribute
	{
		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x0600563D RID: 22077 RVA: 0x002C7BD0 File Offset: 0x002C5FD0
		// (set) Token: 0x0600563E RID: 22078 RVA: 0x002C7BEA File Offset: 0x002C5FEA
		public int Priority { get; set; }
	}
}
