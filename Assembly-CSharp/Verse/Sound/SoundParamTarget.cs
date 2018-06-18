using System;

namespace Verse.Sound
{
	// Token: 0x02000B93 RID: 2963
	[EditorShowClassName]
	public abstract class SoundParamTarget
	{
		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x06004046 RID: 16454
		public abstract string Label { get; }

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x06004047 RID: 16455 RVA: 0x0021C944 File Offset: 0x0021AD44
		public virtual Type NeededFilterType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06004048 RID: 16456
		public abstract void SetOn(Sample sample, float value);
	}
}
