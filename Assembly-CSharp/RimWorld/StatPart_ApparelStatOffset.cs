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
			string result;
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
					result = stringBuilder.ToString();
					goto IL_00be;
				}
			}
			result = (string)null;
			goto IL_00be;
			IL_00be:
			return result;
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
						goto IL_003e;
				}
			}
			bool result = false;
			goto IL_0068;
			IL_003e:
			result = true;
			goto IL_0068;
			IL_0068:
			return result;
		}
	}
}
