using System;

namespace Verse.Sound
{
	// Token: 0x02000B83 RID: 2947
	[EditorShowClassName]
	[EditorReplaceable]
	public abstract class SoundParamSource
	{
		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x06004029 RID: 16425
		public abstract string Label { get; }

		// Token: 0x0600402A RID: 16426
		public abstract float ValueFor(Sample samp);
	}
}
