using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D28 RID: 3368
	public class Hediff_HeartAttack : HediffWithComps
	{
		// Token: 0x0400323A RID: 12858
		private float intervalFactor;

		// Token: 0x0400323B RID: 12859
		private const int SeverityChangeInterval = 5000;

		// Token: 0x0400323C RID: 12860
		private const float TendSuccessChanceFactor = 0.65f;

		// Token: 0x0400323D RID: 12861
		private const float TendSeverityReduction = 0.3f;

		// Token: 0x06004A3F RID: 19007 RVA: 0x0026C0C6 File Offset: 0x0026A4C6
		public override void PostMake()
		{
			base.PostMake();
			this.intervalFactor = Rand.Range(0.1f, 2f);
		}

		// Token: 0x06004A40 RID: 19008 RVA: 0x0026C0E4 File Offset: 0x0026A4E4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.intervalFactor, "intervalFactor", 0f, false);
		}

		// Token: 0x06004A41 RID: 19009 RVA: 0x0026C104 File Offset: 0x0026A504
		public override void Tick()
		{
			base.Tick();
			if (this.pawn.IsHashIntervalTick((int)(5000f * this.intervalFactor)))
			{
				this.Severity += Rand.Range(-0.4f, 0.6f);
			}
		}

		// Token: 0x06004A42 RID: 19010 RVA: 0x0026C154 File Offset: 0x0026A554
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
