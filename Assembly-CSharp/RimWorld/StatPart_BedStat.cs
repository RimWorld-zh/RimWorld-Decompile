using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A4 RID: 2468
	public class StatPart_BedStat : StatPart
	{
		// Token: 0x04002399 RID: 9113
		private StatDef stat = null;

		// Token: 0x0600375A RID: 14170 RVA: 0x001D92A0 File Offset: 0x001D76A0
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					val *= this.BedMultiplier(pawn);
				}
			}
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x001D92E0 File Offset: 0x001D76E0
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.ageTracker != null)
				{
					return "StatsReport_InBed".Translate() + ": x" + this.BedMultiplier(pawn).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x0600375C RID: 14172 RVA: 0x001D934C File Offset: 0x001D774C
		private float BedMultiplier(Pawn pawn)
		{
			float result;
			if (pawn.InBed())
			{
				result = pawn.CurrentBed().GetStatValue(this.stat, true);
			}
			else
			{
				result = 1f;
			}
			return result;
		}
	}
}
