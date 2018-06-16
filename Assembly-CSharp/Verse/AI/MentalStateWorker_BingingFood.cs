using System;

namespace Verse.AI
{
	// Token: 0x02000A64 RID: 2660
	public class MentalStateWorker_BingingFood : MentalStateWorker
	{
		// Token: 0x06003B30 RID: 15152 RVA: 0x001F6400 File Offset: 0x001F4800
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && (!pawn.Spawned || pawn.Map.resourceCounter.TotalHumanEdibleNutrition > 10f);
		}
	}
}
