using System;

namespace Verse
{
	// Token: 0x02000D9F RID: 3487
	public enum LookMode : byte
	{
		// Token: 0x040033FE RID: 13310
		Undefined,
		// Token: 0x040033FF RID: 13311
		Value,
		// Token: 0x04003400 RID: 13312
		Deep,
		// Token: 0x04003401 RID: 13313
		Reference,
		// Token: 0x04003402 RID: 13314
		Def,
		// Token: 0x04003403 RID: 13315
		LocalTargetInfo,
		// Token: 0x04003404 RID: 13316
		TargetInfo,
		// Token: 0x04003405 RID: 13317
		GlobalTargetInfo,
		// Token: 0x04003406 RID: 13318
		BodyPart
	}
}
