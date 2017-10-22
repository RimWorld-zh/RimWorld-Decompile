using Verse;

namespace RimWorld
{
	public class Hediff_HeartAttack : HediffWithComps
	{
		private float intervalFactor;

		private const int SeverityChangeInterval = 5000;

		private const float TendSuccessChanceFactor = 0.65f;

		private const float TendSeverityReduction = 0.3f;

		public override void PostMake()
		{
			base.PostMake();
			this.intervalFactor = Rand.Range(0.1f, 2f);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.intervalFactor, "intervalFactor", 0f, false);
		}

		public override void Tick()
		{
			base.Tick();
			if (base.pawn.IsHashIntervalTick((int)(5000.0 * this.intervalFactor)))
			{
				this.Severity += Rand.Range(-0.4f, 0.6f);
			}
		}

		public override void Tended(float quality, int batchPosition = 0)
		{
			base.Tended(quality, 0);
			float num = (float)(0.64999997615814209 * quality);
			if (Rand.Value < num)
			{
				if (batchPosition == 0 && base.pawn.Spawned)
				{
					MoteMaker.ThrowText(base.pawn.DrawPos, base.pawn.Map, "TextMote_TreatSuccess".Translate(num.ToStringPercent()), 6.5f);
				}
				this.Severity -= 0.3f;
			}
			else if (batchPosition == 0 && base.pawn.Spawned)
			{
				MoteMaker.ThrowText(base.pawn.DrawPos, base.pawn.Map, "TextMote_TreatFailed".Translate(num.ToStringPercent()), 6.5f);
			}
		}
	}
}
