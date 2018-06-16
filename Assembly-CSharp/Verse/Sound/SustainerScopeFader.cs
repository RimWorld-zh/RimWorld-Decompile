using System;

namespace Verse.Sound
{
	// Token: 0x02000DC7 RID: 3527
	public class SustainerScopeFader
	{
		// Token: 0x06004EA5 RID: 20133 RVA: 0x00290B24 File Offset: 0x0028EF24
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

		// Token: 0x0400345E RID: 13406
		public bool inScope = true;

		// Token: 0x0400345F RID: 13407
		public float inScopePercent = 1f;

		// Token: 0x04003460 RID: 13408
		private const float ScopeMatchFallRate = 0.03f;

		// Token: 0x04003461 RID: 13409
		private const float ScopeMatchRiseRate = 0.05f;
	}
}
