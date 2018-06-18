using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D34 RID: 3380
	public class HediffGiver_BrainInjury : HediffGiver
	{
		// Token: 0x06004A6E RID: 19054 RVA: 0x0026C6AC File Offset: 0x0026AAAC
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
							Find.LetterStack.ReceiveLetter(this.letterLabel, this.letter.AdjustedFor(pawn), LetterDefOf.NegativeEvent, pawn, null, null);
						}
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x04003243 RID: 12867
		public float chancePerDamagePct;

		// Token: 0x04003244 RID: 12868
		public string letterLabel;

		// Token: 0x04003245 RID: 12869
		public string letter;
	}
}
