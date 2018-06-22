using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A1 RID: 2465
	public class StatPart_ApparelStatOffset : StatPart
	{
		// Token: 0x06003751 RID: 14161 RVA: 0x001D8F08 File Offset: 0x001D7308
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && req.Thing != null)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.apparel != null)
				{
					for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
					{
						float statValue = pawn.apparel.WornApparel[i].GetStatValue(this.apparelStat, true);
						if (this.subtract)
						{
							val -= statValue;
						}
						else
						{
							val += statValue;
						}
					}
				}
			}
		}

		// Token: 0x06003752 RID: 14162 RVA: 0x001D8FB0 File Offset: 0x001D73B0
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
			return null;
		}

		// Token: 0x06003753 RID: 14163 RVA: 0x001D9080 File Offset: 0x001D7480
		private string InfoTextLineFrom(Thing gear)
		{
			float num = gear.GetStatValue(this.apparelStat, true);
			if (this.subtract)
			{
				num = -num;
			}
			return "    " + gear.LabelCap + ": " + num.ToStringByStyle(this.parentStat.toStringStyle, ToStringNumberSense.Offset);
		}

		// Token: 0x06003754 RID: 14164 RVA: 0x001D90D8 File Offset: 0x001D74D8
		private bool PawnWearingRelevantGear(Pawn pawn)
		{
			if (pawn.apparel != null)
			{
				for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
				{
					Apparel thing = pawn.apparel.WornApparel[i];
					if (thing.GetStatValue(this.apparelStat, true) != 0f)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04002396 RID: 9110
		private StatDef apparelStat;

		// Token: 0x04002397 RID: 9111
		private bool subtract;
	}
}
