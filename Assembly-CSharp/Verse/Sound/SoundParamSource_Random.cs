using System;

namespace Verse.Sound
{
	// Token: 0x02000B8F RID: 2959
	public class SoundParamSource_Random : SoundParamSource
	{
		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x06004045 RID: 16453 RVA: 0x0021CFE8 File Offset: 0x0021B3E8
		public override string Label
		{
			get
			{
				return "Random";
			}
		}

		// Token: 0x06004046 RID: 16454 RVA: 0x0021D004 File Offset: 0x0021B404
		public override float ValueFor(Sample samp)
		{
			return Rand.Value;
		}
	}
}
