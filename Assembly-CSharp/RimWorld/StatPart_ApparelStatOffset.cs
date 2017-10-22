using System.Text;
using Verse;

namespace RimWorld
{
	public class StatPart_ApparelStatOffset : StatPart
	{
		private StatDef apparelStat;

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && req.Thing != null)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.apparel != null)
				{
					for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
					{
						val += pawn.apparel.WornApparel[i].GetStatValue(this.apparelStat, true);
					}
				}
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && req.Thing != null)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.PawnWearingRelevantGear(pawn))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatsReport_RelevantGear".Translate());
					if (pawn.apparel != null)
					{
						for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
						{
							Apparel gear = pawn.apparel.WornApparel[i];
							stringBuilder.AppendLine(this.InfoTextLineFrom(gear));
						}
					}
					return stringBuilder.ToString();
				}
			}
			return (string)null;
		}

		private string InfoTextLineFrom(Thing gear)
		{
			return "    " + gear.LabelCap + ": " + gear.GetStatValue(this.apparelStat, true).ToStringByStyle(base.parentStat.toStringStyle, ToStringNumberSense.Offset);
		}

		private bool PawnWearingRelevantGear(Pawn pawn)
		{
			if (pawn.apparel != null)
			{
				for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
				{
					Apparel thing = pawn.apparel.WornApparel[i];
					if (thing.GetStatValue(this.apparelStat, true) != 0.0)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
