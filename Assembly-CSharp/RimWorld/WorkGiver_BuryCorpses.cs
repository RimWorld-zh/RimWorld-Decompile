using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_BuryCorpses : WorkGiver_Scanner
	{
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Corpse);
			}
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		public override bool ShouldSkip(Pawn pawn)
		{
			return pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse).Count == 0;
		}

		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Corpse corpse = t as Corpse;
			Job result;
			if (corpse == null)
			{
				result = null;
			}
			else if (!HaulAIUtility.PawnCanAutomaticallyHaul(pawn, t, forced))
			{
				result = null;
			}
			else
			{
				Building_Grave building_Grave = this.FindBestGrave(pawn, corpse);
				if (building_Grave == null)
				{
					JobFailReason.Is("NoEmptyGraveLower".Translate());
					result = null;
				}
				else
				{
					Job job = new Job(JobDefOf.BuryCorpse, t, (Thing)building_Grave);
					job.count = corpse.stackCount;
					result = job;
				}
			}
			return result;
		}

		private Building_Grave FindBestGrave(Pawn p, Corpse corpse)
		{
			Predicate<Thing> predicate = (Predicate<Thing>)((Thing m) => (byte)((!m.IsForbidden(p) && p.CanReserveNew(m)) ? (((Building_Grave)m).Accepts(corpse) ? 1 : 0) : 0) != 0);
			Building_Grave result;
			if (corpse.InnerPawn.ownership != null && corpse.InnerPawn.ownership.AssignedGrave != null)
			{
				Building_Grave assignedGrave = corpse.InnerPawn.ownership.AssignedGrave;
				if (predicate(assignedGrave) && p.Map.reachability.CanReach(corpse.Position, (Thing)assignedGrave, PathEndMode.ClosestTouch, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false)))
				{
					result = assignedGrave;
					goto IL_0142;
				}
			}
			Func<Thing, float> priorityGetter = (Func<Thing, float>)((Thing t) => (float)(int)((IStoreSettingsParent)t).GetStoreSettings().Priority);
			IntVec3 position = corpse.Position;
			Map map = corpse.Map;
			List<Thing> searchSet = corpse.Map.listerThings.ThingsInGroup(ThingRequestGroup.Grave);
			PathEndMode peMode = PathEndMode.ClosestTouch;
			TraverseParms traverseParams = TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false);
			Predicate<Thing> validator = predicate;
			result = (Building_Grave)GenClosest.ClosestThing_Global_Reachable(position, map, searchSet, peMode, traverseParams, 9999f, validator, priorityGetter);
			goto IL_0142;
			IL_0142:
			return result;
		}
	}
}
