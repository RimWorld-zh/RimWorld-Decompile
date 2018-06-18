using System;

namespace Verse.Sound
{
	// Token: 0x02000DC6 RID: 3526
	public class SustainerScopeFader
	{
		// Token: 0x06004EA3 RID: 20131 RVA: 0x00290B04 File Offset: 0x0028EF04
		public void SustainerScopeUpdate()
		{
			if (this.inScope)
			{
				float num = this.inScopePercent + 0.05f;
				this.inScopePercent = num;
				if (this.inScopePercent > 1f)
				{
					this.inScopePercent = 1f;
				}
			}
			else
			{
				this.inScopePercent -= 0.03f;
				if (this.inScopePercent <= 0.001f)
				{
					this.inScopePercent = 0f;
				}
			}
		}

		// Token: 0x0400345C RID: 13404
		public bool inScope = true;

		// Token: 0x0400345D RID: 13405
		public float inScopePercent = 1f;

		// Token: 0x0400345E RID: 13406
		private const float ScopeMatchFallRate = 0.03f;

		// Token: 0x0400345F RID: 13407
		private const float ScopeMatchRiseRate = 0.05f;
	}
}
