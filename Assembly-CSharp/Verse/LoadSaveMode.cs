using System;

namespace Verse
{
	// Token: 0x02000D97 RID: 3479
	public enum LoadSaveMode : byte
	{
		// Token: 0x040033D5 RID: 13269
		Inactive,
		// Token: 0x040033D6 RID: 13270
		Saving,
		// Token: 0x040033D7 RID: 13271
		LoadingVars,
		// Token: 0x040033D8 RID: 13272
		ResolvingCrossRefs,
		// Token: 0x040033D9 RID: 13273
		PostLoadInit
	}
}
