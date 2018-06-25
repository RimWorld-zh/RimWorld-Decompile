using System;

namespace Verse.Sound
{
	// Token: 0x02000DC6 RID: 3526
	public class SustainerScopeFader
	{
		// Token: 0x0400346E RID: 13422
		public bool inScope = true;

		// Token: 0x0400346F RID: 13423
		public float inScopePercent = 1f;

		// Token: 0x04003470 RID: 13424
		private const float ScopeMatchFallRate = 0.03f;

		// Token: 0x04003471 RID: 13425
		private const float ScopeMatchRiseRate = 0.05f;

		// Token: 0x06004EBC RID: 20156 RVA: 0x002924C0 File Offset: 0x002908C0
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
	}
}
