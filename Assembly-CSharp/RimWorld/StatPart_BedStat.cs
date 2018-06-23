using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A2 RID: 2466
	public class StatPart_BedStat : StatPart
	{
		// Token: 0x04002398 RID: 9112
		private StatDef stat = null;

		// Token: 0x06003756 RID: 14166 RVA: 0x001D9160 File Offset: 0x001D7560
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

		// Token: 0x06003757 RID: 14167 RVA: 0x001D91A0 File Offset: 0x001D75A0
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

		// Token: 0x06003758 RID: 14168 RVA: 0x001D920C File Offset: 0x001D760C
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
