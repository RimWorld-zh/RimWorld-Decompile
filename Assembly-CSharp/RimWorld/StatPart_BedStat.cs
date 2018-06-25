using System;
using Verse;

namespace RimWorld
{
	public class StatPart_BedStat : StatPart
	{
		private StatDef stat = null;

		public StatPart_BedStat()
		{
		}

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
