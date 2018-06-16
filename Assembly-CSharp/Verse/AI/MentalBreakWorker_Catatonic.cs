using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A5D RID: 2653
	public class MentalBreakWorker_Catatonic : MentalBreakWorker
	{
		// Token: 0x06003AFE RID: 15102 RVA: 0x001F4BCC File Offset: 0x001F2FCC
		public override bool BreakCanOccur(Pawn pawn)
		{
			return pawn.IsColonist && pawn.Spawned && base.BreakCanOccur(pawn);
		}

		// Token: 0x06003AFF RID: 15103 RVA: 0x001F4C04 File Offset: 0x001F3004
		public override bool TryStart(Pawn pawn, Thought reason, bool causedByMood)
		{
			pawn.health.AddHediff(HediffDefOf.CatatonicBreakdown, null, null, null);
			base.TrySendLetter(pawn, "LetterCatatonicMentalBreak", reason);
			return true;
		}
	}
}
