using System;

namespace Verse
{
	// Token: 0x02000D96 RID: 3478
	public enum LoadSaveMode : byte
	{
		// Token: 0x040033E5 RID: 13285
		Inactive,
		// Token: 0x040033E6 RID: 13286
		Saving,
		// Token: 0x040033E7 RID: 13287
		LoadingVars,
		// Token: 0x040033E8 RID: 13288
		ResolvingCrossRefs,
		// Token: 0x040033E9 RID: 13289
		PostLoadInit
	}
}
