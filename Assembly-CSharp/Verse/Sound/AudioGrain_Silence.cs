using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000B7A RID: 2938
	public class AudioGrain_Silence : AudioGrain
	{
		// Token: 0x06003FFF RID: 16383 RVA: 0x0021B29C File Offset: 0x0021969C
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			yield return new ResolvedGrain_Silence(this);
			yield break;
		}

		// Token: 0x06004000 RID: 16384 RVA: 0x0021B2C8 File Offset: 0x002196C8
		public override int GetHashCode()
		{
			return this.durationRange.GetHashCode();
		}

		// Token: 0x04002AE6 RID: 10982
		[EditSliderRange(0f, 5f)]
		public FloatRange durationRange = new FloatRange(1f, 2f);
	}
}
