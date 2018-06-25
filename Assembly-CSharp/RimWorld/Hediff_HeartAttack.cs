using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D2B RID: 3371
	public class Hediff_HeartAttack : HediffWithComps
	{
		// Token: 0x04003241 RID: 12865
		private float intervalFactor;

		// Token: 0x04003242 RID: 12866
		private const int SeverityChangeInterval = 5000;

		// Token: 0x04003243 RID: 12867
		private const float TendSuccessChanceFactor = 0.65f;

		// Token: 0x04003244 RID: 12868
		private const float TendSeverityReduction = 0.3f;

		// Token: 0x06004A43 RID: 19011 RVA: 0x0026C4D2 File Offset: 0x0026A8D2
		public override void PostMake()
		{
			base.PostMake();
			this.intervalFactor = Rand.Range(0.1f, 2f);
		}

		// Token: 0x06004A44 RID: 19012 RVA: 0x0026C4F0 File Offset: 0x0026A8F0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.intervalFactor, "intervalFactor", 0f, false);
		}

		// Token: 0x06004A45 RID: 19013 RVA: 0x0026C510 File Offset: 0x0026A910
		public override void Tick()
		{
			base.Tick();
			if (this.pawn.IsHashIntervalTick((int)(5000f * this.intervalFactor)))
			{
				this.Severity += Rand.Range(-0.4f, 0.6f);
			}
		}

		// Token: 0x06004A46 RID: 19014 RVA: 0x0026C560 File Offset: 0x0026A960
		public override void Tended(float quality, int batchPosition = 0)
		{
			base.Tended(quality, 0);
			float num = 0.65f * quality;
			if (Rand.Value < num)
			{
				if (batchPosition == 0 && this.pawn.Spawned)
				{
					MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, "TextMote_TreatSuccess".Translate(new object[]
					{
						num.ToStringPercent()
					}), 6.5f);
				}
				this.Severity -= 0.3f;
			}
			else if (batchPosition == 0 && this.pawn.Spawned)
			{
				MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, "TextMote_TreatFailed".Translate(new object[]
				{
					num.ToStringPercent()
				}), 6.5f);
			}
		}
	}
}
