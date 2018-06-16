using System;

namespace Verse
{
	// Token: 0x02000DA0 RID: 3488
	public enum LookMode : byte
	{
		// Token: 0x040033EE RID: 13294
		Undefined,
		// Token: 0x040033EF RID: 13295
		Value,
		// Token: 0x040033F0 RID: 13296
		Deep,
		// Token: 0x040033F1 RID: 13297
		Reference,
		// Token: 0x040033F2 RID: 13298
		Def,
		// Token: 0x040033F3 RID: 13299
		LocalTargetInfo,
		// Token: 0x040033F4 RID: 13300
		TargetInfo,
		// Token: 0x040033F5 RID: 13301
		GlobalTargetInfo,
		// Token: 0x040033F6 RID: 13302
		BodyPart
	}
}
