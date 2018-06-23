using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000B76 RID: 2934
	public class AudioGrain_Silence : AudioGrain
	{
		// Token: 0x04002AEB RID: 10987
		[EditSliderRange(0f, 5f)]
		public FloatRange durationRange = new FloatRange(1f, 2f);

		// Token: 0x06004001 RID: 16385 RVA: 0x0021B938 File Offset: 0x00219D38
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			yield return new ResolvedGrain_Silence(this);
			yield break;
		}

		// Token: 0x06004002 RID: 16386 RVA: 0x0021B964 File Offset: 0x00219D64
		public override int GetHashCode()
		{
			return this.durationRange.GetHashCode();
		}
	}
}
