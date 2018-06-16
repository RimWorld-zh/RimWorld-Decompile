using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000B7A RID: 2938
	public class AudioGrain_Silence : AudioGrain
	{
		// Token: 0x06003FFD RID: 16381 RVA: 0x0021B1C8 File Offset: 0x002195C8
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			yield return new ResolvedGrain_Silence(this);
			yield break;
		}

		// Token: 0x06003FFE RID: 16382 RVA: 0x0021B1F4 File Offset: 0x002195F4
		public override int GetHashCode()
		{
			return this.durationRange.GetHashCode();
		}

		// Token: 0x04002AE6 RID: 10982
		[EditSliderRange(0f, 5f)]
		public FloatRange durationRange = new FloatRange(1f, 2f);
	}
}
