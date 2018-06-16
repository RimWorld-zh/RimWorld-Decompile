using System;

namespace Verse.Sound
{
	// Token: 0x02000B91 RID: 2961
	public class SoundParamSource_Random : SoundParamSource
	{
		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x0600403E RID: 16446 RVA: 0x0021C79C File Offset: 0x0021AB9C
		public override string Label
		{
			get
			{
				return "Random";
			}
		}

		// Token: 0x0600403F RID: 16447 RVA: 0x0021C7B8 File Offset: 0x0021ABB8
		public override float ValueFor(Sample samp)
		{
			return Rand.Value;
		}
	}
}
