using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_Mate : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.gender != Gender.Male || !pawn.ageTracker.CurLifeStage.reproductive)
			{
				result = null;
			}
			else
			{
				Predicate<Thing> validator = (Predicate<Thing>)delegate(Thing t)
				{
					Pawn pawn3 = t as Pawn;
					return (byte)((!pawn3.Downed) ? ((pawn3.CanCasuallyInteractNow(false) && !pawn3.IsForbidden(pawn)) ? ((pawn3.Faction == pawn.Faction) ? (PawnUtility.FertileMateTarget(pawn, pawn3) ? 1 : 0) : 0) : 0) : 0) != 0;
				};
				Pawn pawn2 = (Pawn)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(pawn.def), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 30f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
				result = ((pawn2 != null) ? new Job(JobDefOf.Mate, (Thing)pawn2) : null);
			}
			return result;
		}
	}
}
