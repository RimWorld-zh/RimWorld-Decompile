using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000B79 RID: 2937
	public class AudioGrain_Silence : AudioGrain
	{
		// Token: 0x04002AF2 RID: 10994
		[EditSliderRange(0f, 5f)]
		public FloatRange durationRange = new FloatRange(1f, 2f);

		// Token: 0x06004004 RID: 16388 RVA: 0x0021BCF4 File Offset: 0x0021A0F4
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			yield return new ResolvedGrain_Silence(this);
			yield break;
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x0021BD20 File Offset: 0x0021A120
		public override int GetHashCode()
		{
			return this.durationRange.GetHashCode();
		}
	}
}
