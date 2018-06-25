using System;

namespace Verse.AI
{
	// Token: 0x02000A63 RID: 2659
	public class MentalStateWorker_BingingFood : MentalStateWorker
	{
		// Token: 0x06003B32 RID: 15154 RVA: 0x001F6C28 File Offset: 0x001F5028
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && (!pawn.Spawned || pawn.Map.resourceCounter.TotalHumanEdibleNutrition > 10f);
		}
	}
}
