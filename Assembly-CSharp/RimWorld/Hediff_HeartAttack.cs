using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D2B RID: 3371
	public class Hediff_HeartAttack : HediffWithComps
	{
		// Token: 0x06004A2D RID: 18989 RVA: 0x0026AC3A File Offset: 0x0026903A
		public override void PostMake()
		{
			base.PostMake();
			this.intervalFactor = Rand.Range(0.1f, 2f);
		}

		// Token: 0x06004A2E RID: 18990 RVA: 0x0026AC58 File Offset: 0x00269058
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.intervalFactor, "intervalFactor", 0f, false);
		}

		// Token: 0x06004A2F RID: 18991 RVA: 0x0026AC78 File Offset: 0x00269078
		public override void Tick()
		{
			base.Tick();
			if (this.pawn.IsHashIntervalTick((int)(5000f * this.intervalFactor)))
			{
				this.Severity += Rand.Range(-0.4f, 0.6f);
			}
		}

		// Token: 0x06004A30 RID: 18992 RVA: 0x0026ACC8 File Offset: 0x002690C8
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

		// Token: 0x0400322F RID: 12847
		private float intervalFactor;

		// Token: 0x04003230 RID: 12848
		private const int SeverityChangeInterval = 5000;

		// Token: 0x04003231 RID: 12849
		private const float TendSuccessChanceFactor = 0.65f;

		// Token: 0x04003232 RID: 12850
		private const float TendSeverityReduction = 0.3f;
	}
}
