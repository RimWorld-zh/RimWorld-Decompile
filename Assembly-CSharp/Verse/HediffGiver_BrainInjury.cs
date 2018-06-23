using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D31 RID: 3377
	public class HediffGiver_BrainInjury : HediffGiver
	{
		// Token: 0x0400324E RID: 12878
		public float chancePerDamagePct;

		// Token: 0x0400324F RID: 12879
		public string letterLabel;

		// Token: 0x04003250 RID: 12880
		public string letter;

		// Token: 0x06004A82 RID: 19074 RVA: 0x0026DC38 File Offset: 0x0026C038
		public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
		{
			bool result;
			if (!(hediff is Hediff_Injury))
			{
				result = false;
			}
			else if (hediff.Part != pawn.health.hediffSet.GetBrain())
			{
				result = false;
			}
			else
			{
				float num = hediff.Severity / hediff.Part.def.GetMaxHealth(pawn);
				if (Rand.Value < num * this.chancePerDamagePct)
				{
					bool flag = base.TryApply(pawn, null);
					if (flag)
					{
						if ((pawn.Faction == Faction.OfPlayer || pawn.IsPrisonerOfColony) && !this.letter.NullOrEmpty())
						{
							Find.LetterStack.ReceiveLetter(this.letterLabel, this.letter.AdjustedFor(pawn, "PAWN"), LetterDefOf.NegativeEvent, pawn, null, null);
						}
						return true;
					}
				}
				result = false;
			}
			return result;
		}
	}
}
