using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_IdleJoy : JobGiver_GetJoy
	{
		private const int GameStartNoIdleJoyTicks = 60000;

		protected override Job TryGiveJob(Pawn pawn)
		{
			return (pawn.needs.joy != null) ? ((Find.TickManager.TicksGame >= 60000) ? ((!JoyUtility.LordPreventsGettingJoy(pawn) && !JoyUtility.TimetablePreventsGettingJoy(pawn)) ? base.TryGiveJob(pawn) : null) : null) : null;
		}
	}
}
