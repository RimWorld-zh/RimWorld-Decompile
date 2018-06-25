using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A5B RID: 2651
	public class MentalBreakWorker_Catatonic : MentalBreakWorker
	{
		// Token: 0x06003AFF RID: 15103 RVA: 0x001F50C8 File Offset: 0x001F34C8
		public override bool BreakCanOccur(Pawn pawn)
		{
			return pawn.IsColonist && pawn.Spawned && base.BreakCanOccur(pawn);
		}

		// Token: 0x06003B00 RID: 15104 RVA: 0x001F5100 File Offset: 0x001F3500
		public override bool TryStart(Pawn pawn, Thought reason, bool causedByMood)
		{
			pawn.health.AddHediff(HediffDefOf.CatatonicBreakdown, null, null, null);
			base.TrySendLetter(pawn, "LetterCatatonicMentalBreak", reason);
			return true;
		}
	}
}
