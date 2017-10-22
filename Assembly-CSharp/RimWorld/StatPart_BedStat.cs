using Verse;

namespace RimWorld
{
	public class StatPart_BedStat : StatPart
	{
		private StatDef stat = null;

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
			string result;
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.ageTracker != null)
				{
					result = "StatsReport_InBed".Translate() + ": x" + this.BedMultiplier(pawn).ToStringPercent();
					goto IL_005b;
				}
			}
			result = (string)null;
			goto IL_005b;
			IL_005b:
			return result;
		}

		private float BedMultiplier(Pawn pawn)
		{
			return (float)((!pawn.InBed()) ? 1.0 : pawn.CurrentBed().GetStatValue(this.stat, true));
		}
	}
}
