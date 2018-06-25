using System;

namespace Verse.AI
{
	// Token: 0x02000A6C RID: 2668
	public class MentalStateWorker_Slaughterer : MentalStateWorker
	{
		// Token: 0x06003B4B RID: 15179 RVA: 0x001F6D78 File Offset: 0x001F5178
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && SlaughtererMentalStateUtility.FindAnimal(pawn) != null;
		}
	}
}
