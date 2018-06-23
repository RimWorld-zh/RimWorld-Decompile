using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000FA RID: 250
	public class JobGiver_IdleJoy : JobGiver_GetJoy
	{
		// Token: 0x040002D1 RID: 721
		private const int GameStartNoIdleJoyTicks = 60000;

		// Token: 0x06000543 RID: 1347 RVA: 0x00039A04 File Offset: 0x00037E04
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.needs.joy == null)
			{
				result = null;
			}
			else if (Find.TickManager.TicksGame < 60000)
			{
				result = null;
			}
			else if (JoyUtility.LordPreventsGettingJoy(pawn) || JoyUtility.TimetablePreventsGettingJoy(pawn))
			{
				result = null;
			}
			else
			{
				result = base.TryGiveJob(pawn);
			}
			return result;
		}
	}
}
