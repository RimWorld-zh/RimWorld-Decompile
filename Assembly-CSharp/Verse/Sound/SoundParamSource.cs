using System;

namespace Verse.Sound
{
	// Token: 0x02000B87 RID: 2951
	[EditorShowClassName]
	[EditorReplaceable]
	public abstract class SoundParamSource
	{
		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06004027 RID: 16423
		public abstract string Label { get; }

		// Token: 0x06004028 RID: 16424
		public abstract float ValueFor(Sample samp);
	}
}
