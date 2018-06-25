using System;

namespace Verse.AI
{
	// Token: 0x02000A62 RID: 2658
	public class MentalStateWorker_BingingFood : MentalStateWorker
	{
		// Token: 0x06003B31 RID: 15153 RVA: 0x001F68FC File Offset: 0x001F4CFC
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && (!pawn.Spawned || pawn.Map.resourceCounter.TotalHumanEdibleNutrition > 10f);
		}
	}
}
