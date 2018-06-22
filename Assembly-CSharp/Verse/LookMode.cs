using System;

namespace Verse
{
	// Token: 0x02000D9C RID: 3484
	public enum LookMode : byte
	{
		// Token: 0x040033F7 RID: 13303
		Undefined,
		// Token: 0x040033F8 RID: 13304
		Value,
		// Token: 0x040033F9 RID: 13305
		Deep,
		// Token: 0x040033FA RID: 13306
		Reference,
		// Token: 0x040033FB RID: 13307
		Def,
		// Token: 0x040033FC RID: 13308
		LocalTargetInfo,
		// Token: 0x040033FD RID: 13309
		TargetInfo,
		// Token: 0x040033FE RID: 13310
		GlobalTargetInfo,
		// Token: 0x040033FF RID: 13311
		BodyPart
	}
}
