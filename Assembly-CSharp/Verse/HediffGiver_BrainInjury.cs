using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D35 RID: 3381
	public class HediffGiver_BrainInjury : HediffGiver
	{
		// Token: 0x06004A70 RID: 19056 RVA: 0x0026C6D4 File Offset: 0x0026AAD4
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

		// Token: 0x04003245 RID: 12869
		public float chancePerDamagePct;

		// Token: 0x04003246 RID: 12870
		public string letterLabel;

		// Token: 0x04003247 RID: 12871
		public string letter;
	}
}
