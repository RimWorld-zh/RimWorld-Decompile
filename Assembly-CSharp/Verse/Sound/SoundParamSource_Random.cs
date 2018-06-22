using System;

namespace Verse.Sound
{
	// Token: 0x02000B8D RID: 2957
	public class SoundParamSource_Random : SoundParamSource
	{
		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x06004042 RID: 16450 RVA: 0x0021CF0C File Offset: 0x0021B30C
		public override string Label
		{
			get
			{
				return "Random";
			}
		}

		// Token: 0x06004043 RID: 16451 RVA: 0x0021CF28 File Offset: 0x0021B328
		public override float ValueFor(Sample samp)
		{
			return Rand.Value;
		}
	}
}
