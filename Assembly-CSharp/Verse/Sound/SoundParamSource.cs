using System;

namespace Verse.Sound
{
	// Token: 0x02000B87 RID: 2951
	[EditorShowClassName]
	[EditorReplaceable]
	public abstract class SoundParamSource
	{
		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06004025 RID: 16421
		public abstract string Label { get; }

		// Token: 0x06004026 RID: 16422
		public abstract float ValueFor(Sample samp);
	}
}
