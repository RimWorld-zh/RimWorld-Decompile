using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A59 RID: 2649
	public class MentalBreakWorker_Catatonic : MentalBreakWorker
	{
		// Token: 0x06003AFB RID: 15099 RVA: 0x001F4F9C File Offset: 0x001F339C
		public override bool BreakCanOccur(Pawn pawn)
		{
			return pawn.IsColonist && pawn.Spawned && base.BreakCanOccur(pawn);
		}

		// Token: 0x06003AFC RID: 15100 RVA: 0x001F4FD4 File Offset: 0x001F33D4
		public override bool TryStart(Pawn pawn, Thought reason, bool causedByMood)
		{
			pawn.health.AddHediff(HediffDefOf.CatatonicBreakdown, null, null, null);
			base.TrySendLetter(pawn, "LetterCatatonicMentalBreak", reason);
			return true;
		}
	}
}
