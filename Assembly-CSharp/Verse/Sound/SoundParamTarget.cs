using System;

namespace Verse.Sound
{
	// Token: 0x02000B93 RID: 2963
	[EditorShowClassName]
	public abstract class SoundParamTarget
	{
		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x06004044 RID: 16452
		public abstract string Label { get; }

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x06004045 RID: 16453 RVA: 0x0021C870 File Offset: 0x0021AC70
		public virtual Type NeededFilterType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06004046 RID: 16454
		public abstract void SetOn(Sample sample, float value);
	}
}
