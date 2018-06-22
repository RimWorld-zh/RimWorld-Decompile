using System;

namespace Verse.Sound
{
	// Token: 0x02000B8F RID: 2959
	[EditorShowClassName]
	public abstract class SoundParamTarget
	{
		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x06004048 RID: 16456
		public abstract string Label { get; }

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x06004049 RID: 16457 RVA: 0x0021CFE0 File Offset: 0x0021B3E0
		public virtual Type NeededFilterType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600404A RID: 16458
		public abstract void SetOn(Sample sample, float value);
	}
}
