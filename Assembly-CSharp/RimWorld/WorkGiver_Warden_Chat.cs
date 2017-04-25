using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Warden_Chat : WorkGiver_Warden
	{
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if ((pawn2.guest.interactionMode == PrisonerInteractionMode.Chat || pawn2.guest.interactionMode == PrisonerInteractionMode.AttemptRecruit) && pawn2.guest.ScheduledForInteraction && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && pawn.CanReserve(t, 1, -1, ReservationLayer.Default, false) && pawn2.Awake())
			{
				if (pawn2.guest.interactionMode == PrisonerInteractionMode.Chat)
				{
					return new Job(JobDefOf.PrisonerFriendlyChat, t);
				}
				if (pawn2.guest.interactionMode == PrisonerInteractionMode.AttemptRecruit)
				{
					return new Job(JobDefOf.PrisonerAttemptRecruit, t);
				}
			}
			return null;
		}
	}
}