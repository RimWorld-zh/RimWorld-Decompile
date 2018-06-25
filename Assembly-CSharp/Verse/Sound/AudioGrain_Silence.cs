using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000B78 RID: 2936
	public class AudioGrain_Silence : AudioGrain
	{
		// Token: 0x04002AEB RID: 10987
		[EditSliderRange(0f, 5f)]
		public FloatRange durationRange = new FloatRange(1f, 2f);

		// Token: 0x06004004 RID: 16388 RVA: 0x0021BA14 File Offset: 0x00219E14
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			yield return new ResolvedGrain_Silence(this);
			yield break;
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x0021BA40 File Offset: 0x00219E40
		public override int GetHashCode()
		{
			return this.durationRange.GetHashCode();
		}
	}
}
