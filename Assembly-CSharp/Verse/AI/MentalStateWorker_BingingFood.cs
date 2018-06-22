using System;

namespace Verse.AI
{
	// Token: 0x02000A60 RID: 2656
	public class MentalStateWorker_BingingFood : MentalStateWorker
	{
		// Token: 0x06003B2D RID: 15149 RVA: 0x001F67D0 File Offset: 0x001F4BD0
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && (!pawn.Spawned || pawn.Map.resourceCounter.TotalHumanEdibleNutrition > 10f);
		}
	}
}
