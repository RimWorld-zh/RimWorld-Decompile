using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A6 RID: 2470
	public class StatPart_BedStat : StatPart
	{
		// Token: 0x0600375D RID: 14173 RVA: 0x001D8F64 File Offset: 0x001D7364
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

		// Token: 0x0600375E RID: 14174 RVA: 0x001D8FA4 File Offset: 0x001D73A4
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

		// Token: 0x0600375F RID: 14175 RVA: 0x001D9010 File Offset: 0x001D7410
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

		// Token: 0x0400239A RID: 9114
		private StatDef stat = null;
	}
}
