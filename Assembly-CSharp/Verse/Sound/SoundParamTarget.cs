using System;

namespace Verse.Sound
{
	// Token: 0x02000B91 RID: 2961
	[EditorShowClassName]
	public abstract class SoundParamTarget
	{
		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x0600404B RID: 16459
		public abstract string Label { get; }

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x0600404C RID: 16460 RVA: 0x0021D0BC File Offset: 0x0021B4BC
		public virtual Type NeededFilterType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600404D RID: 16461
		public abstract void SetOn(Sample sample, float value);
	}
}
