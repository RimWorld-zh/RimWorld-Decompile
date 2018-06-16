using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006CF RID: 1743
	public class IntermittentSteamSprayer
	{
		// Token: 0x060025B1 RID: 9649 RVA: 0x001428FF File Offset: 0x00140CFF
		public IntermittentSteamSprayer(Thing parent)
		{
			this.parent = parent;
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x00142930 File Offset: 0x00140D30
		public void SteamSprayerTick()
		{
			if (this.sprayTicksLeft > 0)
			{
				this.sprayTicksLeft--;
				if (Rand.Value < 0.6f)
				{
					MoteMaker.ThrowAirPuffUp(this.parent.TrueCenter(), this.parent.Map);
				}
				if (Find.TickManager.TicksGame % 20 == 0)
				{
					GenTemperature.PushHeat(this.parent, 40f);
				}
				if (this.sprayTicksLeft <= 0)
				{
					if (this.endSprayCallback != null)
					{
						this.endSprayCallback();
					}
					this.ticksUntilSpray = Rand.RangeInclusive(500, 2000);
				}
			}
			else
			{
				this.ticksUntilSpray--;
				if (this.ticksUntilSpray <= 0)
				{
					if (this.startSprayCallback != null)
					{
						this.startSprayCallback();
					}
					this.sprayTicksLeft = Rand.RangeInclusive(200, 500);
				}
			}
		}

		// Token: 0x04001508 RID: 5384
		private Thing parent;

		// Token: 0x04001509 RID: 5385
		private int ticksUntilSpray = 500;

		// Token: 0x0400150A RID: 5386
		private int sprayTicksLeft = 0;

		// Token: 0x0400150B RID: 5387
		public Action startSprayCallback = null;

		// Token: 0x0400150C RID: 5388
		public Action endSprayCallback = null;

		// Token: 0x0400150D RID: 5389
		private const int MinTicksBetweenSprays = 500;

		// Token: 0x0400150E RID: 5390
		private const int MaxTicksBetweenSprays = 2000;

		// Token: 0x0400150F RID: 5391
		private const int MinSprayDuration = 200;

		// Token: 0x04001510 RID: 5392
		private const int MaxSprayDuration = 500;

		// Token: 0x04001511 RID: 5393
		private const float SprayThickness = 0.6f;
	}
}
