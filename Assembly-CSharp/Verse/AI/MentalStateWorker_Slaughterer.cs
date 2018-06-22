using System;

namespace Verse.AI
{
	// Token: 0x02000A6A RID: 2666
	public class MentalStateWorker_Slaughterer : MentalStateWorker
	{
		// Token: 0x06003B47 RID: 15175 RVA: 0x001F6C4C File Offset: 0x001F504C
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && SlaughtererMentalStateUtility.FindAnimal(pawn) != null;
		}
	}
}
