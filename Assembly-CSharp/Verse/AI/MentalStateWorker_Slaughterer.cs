using System;

namespace Verse.AI
{
	// Token: 0x02000A6E RID: 2670
	public class MentalStateWorker_Slaughterer : MentalStateWorker
	{
		// Token: 0x06003B4A RID: 15178 RVA: 0x001F687C File Offset: 0x001F4C7C
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && SlaughtererMentalStateUtility.FindAnimal(pawn) != null;
		}
	}
}
