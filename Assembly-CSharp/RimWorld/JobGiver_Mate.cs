using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_Mate : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.gender == Gender.Male && pawn.ageTracker.CurLifeStage.reproductive)
			{
				Predicate<Thing> validator = delegate(Thing t)
				{
					Pawn pawn3 = t as Pawn;
					if (pawn3.Downed)
					{
						return false;
					}
					if (pawn3.CanCasuallyInteractNow(false) && !pawn3.IsForbidden(pawn))
					{
						if (pawn3.Faction != pawn.Faction)
						{
							return false;
						}
						if (!PawnUtility.FertileMateTarget(pawn, pawn3))
						{
							return false;
						}
						return true;
					}
					return false;
				};
				Pawn pawn2 = (Pawn)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(pawn.def), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 30f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
				if (pawn2 == null)
				{
					return null;
				}
				return new Job(JobDefOf.Mate, pawn2);
			}
			return null;
		}
	}
}
