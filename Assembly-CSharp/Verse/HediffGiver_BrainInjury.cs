using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D34 RID: 3380
	public class HediffGiver_BrainInjury : HediffGiver
	{
		// Token: 0x04003255 RID: 12885
		public float chancePerDamagePct;

		// Token: 0x04003256 RID: 12886
		public string letterLabel;

		// Token: 0x04003257 RID: 12887
		public string letter;

		// Token: 0x06004A86 RID: 19078 RVA: 0x0026E044 File Offset: 0x0026C444
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
