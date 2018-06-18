using System;

namespace Verse
{
	// Token: 0x02000D96 RID: 3478
	public enum LoadSaveMode : byte
	{
		// Token: 0x040033D3 RID: 13267
		Inactive,
		// Token: 0x040033D4 RID: 13268
		Saving,
		// Token: 0x040033D5 RID: 13269
		LoadingVars,
		// Token: 0x040033D6 RID: 13270
		ResolvingCrossRefs,
		// Token: 0x040033D7 RID: 13271
		PostLoadInit
	}
}
