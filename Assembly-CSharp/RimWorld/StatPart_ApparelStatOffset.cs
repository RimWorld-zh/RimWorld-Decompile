using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A3 RID: 2467
	public class StatPart_ApparelStatOffset : StatPart
	{
		// Token: 0x04002397 RID: 9111
		private StatDef apparelStat;

		// Token: 0x04002398 RID: 9112
		private bool subtract;

		// Token: 0x06003755 RID: 14165 RVA: 0x001D9048 File Offset: 0x001D7448
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

		// Token: 0x06003756 RID: 14166 RVA: 0x001D90F0 File Offset: 0x001D74F0
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

		// Token: 0x06003757 RID: 14167 RVA: 0x001D91C0 File Offset: 0x001D75C0
		private string InfoTextLineFrom(Thing gear)
		{
			float num = gear.GetStatValue(this.apparelStat, true);
			if (this.subtract)
			{
				num = -num;
			}
			return "    " + gear.LabelCap + ": " + num.ToStringByStyle(this.parentStat.toStringStyle, ToStringNumberSense.Offset);
		}

		// Token: 0x06003758 RID: 14168 RVA: 0x001D9218 File Offset: 0x001D7618
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
	}
}
