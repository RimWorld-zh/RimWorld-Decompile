using System;

namespace Verse
{
	// Token: 0x02000D95 RID: 3477
	public enum LoadSaveMode : byte
	{
		// Token: 0x040033DE RID: 13278
		Inactive,
		// Token: 0x040033DF RID: 13279
		Saving,
		// Token: 0x040033E0 RID: 13280
		LoadingVars,
		// Token: 0x040033E1 RID: 13281
		ResolvingCrossRefs,
		// Token: 0x040033E2 RID: 13282
		PostLoadInit
	}
}
