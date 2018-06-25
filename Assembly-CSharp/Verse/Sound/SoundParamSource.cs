using System;

namespace Verse.Sound
{
	// Token: 0x02000B86 RID: 2950
	[EditorShowClassName]
	[EditorReplaceable]
	public abstract class SoundParamSource
	{
		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x0600402C RID: 16428
		public abstract string Label { get; }

		// Token: 0x0600402D RID: 16429
		public abstract float ValueFor(Sample samp);
	}
}
