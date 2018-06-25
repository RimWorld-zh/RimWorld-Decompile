using System;

namespace Verse.AI
{
	// Token: 0x02000A6D RID: 2669
	public class MentalStateWorker_Slaughterer : MentalStateWorker
	{
		// Token: 0x06003B4C RID: 15180 RVA: 0x001F70A4 File Offset: 0x001F54A4
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && SlaughtererMentalStateUtility.FindAnimal(pawn) != null;
		}
	}
}
