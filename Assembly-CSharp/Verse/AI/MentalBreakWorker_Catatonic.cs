using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A5D RID: 2653
	public class MentalBreakWorker_Catatonic : MentalBreakWorker
	{
		// Token: 0x06003B00 RID: 15104 RVA: 0x001F4CA0 File Offset: 0x001F30A0
		public override bool BreakCanOccur(Pawn pawn)
		{
			return pawn.IsColonist && pawn.Spawned && base.BreakCanOccur(pawn);
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x001F4CD8 File Offset: 0x001F30D8
		public override bool TryStart(Pawn pawn, Thought reason, bool causedByMood)
		{
			pawn.health.AddHediff(HediffDefOf.CatatonicBreakdown, null, null, null);
			base.TrySendLetter(pawn, "LetterCatatonicMentalBreak", reason);
			return true;
		}
	}
}
