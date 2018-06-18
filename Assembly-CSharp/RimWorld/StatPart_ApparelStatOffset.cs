using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A5 RID: 2469
	public class StatPart_ApparelStatOffset : StatPart
	{
		// Token: 0x06003758 RID: 14168 RVA: 0x001D8D0C File Offset: 0x001D710C
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

		// Token: 0x06003759 RID: 14169 RVA: 0x001D8DB4 File Offset: 0x001D71B4
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

		// Token: 0x0600375A RID: 14170 RVA: 0x001D8E84 File Offset: 0x001D7284
		private string InfoTextLineFrom(Thing gear)
		{
			float num = gear.GetStatValue(this.apparelStat, true);
			if (this.subtract)
			{
				num = -num;
			}
			return "    " + gear.LabelCap + ": " + num.ToStringByStyle(this.parentStat.toStringStyle, ToStringNumberSense.Offset);
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x001D8EDC File Offset: 0x001D72DC
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

		// Token: 0x04002398 RID: 9112
		private StatDef apparelStat;

		// Token: 0x04002399 RID: 9113
		private bool subtract;
	}
}
