using System;

namespace Verse
{
	// Token: 0x02000E3D RID: 3645
	[AttributeUsage(AttributeTargets.Field)]
	public class TranslationHandleAttribute : Attribute
	{
		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x06005639 RID: 22073 RVA: 0x002C78B8 File Offset: 0x002C5CB8
		// (set) Token: 0x0600563A RID: 22074 RVA: 0x002C78D2 File Offset: 0x002C5CD2
		public int Priority { get; set; }
	}
}
