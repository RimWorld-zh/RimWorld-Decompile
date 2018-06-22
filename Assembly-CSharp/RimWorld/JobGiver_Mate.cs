using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000A6 RID: 166
	public class JobGiver_Mate : ThinkNode_JobGiver
	{
		// Token: 0x06000414 RID: 1044 RVA: 0x00030C3C File Offset: 0x0002F03C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.gender != Gender.Male || !pawn.ageTracker.CurLifeStage.reproductive)
			{
				result = null;
			}
			else
			{
				Predicate<Thing> validator = delegate(Thing t)
				{
					Pawn pawn3 = t as Pawn;
					return !pawn3.Downed && pawn3.CanCasuallyInteractNow(false) && !pawn3.IsForbidden(pawn) && pawn3.Faction == pawn.Faction && PawnUtility.FertileMateTarget(pawn, pawn3);
				};
				Pawn pawn2 = (Pawn)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(pawn.def), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 30f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
				if (pawn2 == null)
				{
					result = null;
				}
				else
				{
					result = new Job(JobDefOf.Mate, pawn2);
				}
			}
			return result;
		}
	}
}
