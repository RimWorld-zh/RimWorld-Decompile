using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A5C RID: 2652
	public class MentalBreakWorker_Catatonic : MentalBreakWorker
	{
		// Token: 0x06003B00 RID: 15104 RVA: 0x001F53F4 File Offset: 0x001F37F4
		public override bool BreakCanOccur(Pawn pawn)
		{
			return pawn.IsColonist && pawn.Spawned && base.BreakCanOccur(pawn);
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x001F542C File Offset: 0x001F382C
		public override bool TryStart(Pawn pawn, Thought reason, bool causedByMood)
		{
			pawn.health.AddHediff(HediffDefOf.CatatonicBreakdown, null, null, null);
			base.TrySendLetter(pawn, "LetterCatatonicMentalBreak", reason);
			return true;
		}
	}
}
