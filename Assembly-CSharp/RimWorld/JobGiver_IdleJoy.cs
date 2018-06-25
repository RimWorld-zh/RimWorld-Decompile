using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_IdleJoy : JobGiver_GetJoy
	{
		private const int GameStartNoIdleJoyTicks = 60000;

		public JobGiver_IdleJoy()
		{
		}

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
