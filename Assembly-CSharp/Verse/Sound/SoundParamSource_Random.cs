using System;

namespace Verse.Sound
{
	// Token: 0x02000B91 RID: 2961
	public class SoundParamSource_Random : SoundParamSource
	{
		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x06004040 RID: 16448 RVA: 0x0021C870 File Offset: 0x0021AC70
		public override string Label
		{
			get
			{
				return "Random";
			}
		}

		// Token: 0x06004041 RID: 16449 RVA: 0x0021C88C File Offset: 0x0021AC8C
		public override float ValueFor(Sample samp)
		{
			return Rand.Value;
		}
	}
}
