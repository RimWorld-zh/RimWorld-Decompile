using RimWorld;
using System;

namespace Verse.AI
{
	public static class CorpseObsessionMentalStateUtility
	{
		public static Corpse GetClosestCorpseToDigUp(Pawn pawn)
		{
			Corpse result;
			if (!pawn.Spawned)
			{
				result = null;
			}
			else
			{
				Building_Grave building_Grave = (Building_Grave)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Grave), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Predicate<Thing>)delegate(Thing x)
				{
					Building_Grave building_Grave2 = (Building_Grave)x;
					return building_Grave2.HasCorpse && CorpseObsessionMentalStateUtility.IsCorpseValid(building_Grave2.Corpse, pawn, true);
				}, null, 0, -1, false, RegionType.Set_Passable, false);
				result = ((building_Grave == null) ? null : building_Grave.Corpse);
			}
			return result;
		}

		public static bool IsCorpseValid(Corpse corpse, Pawn pawn, bool ignoreReachability = false)
		{
			bool result;
			if (corpse == null || corpse.Destroyed || !corpse.InnerPawn.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (pawn.carryTracker.CarriedThing == corpse)
			{
				result = true;
			}
			else if (corpse.Spawned)
			{
				result = (pawn.CanReserve((Thing)corpse, 1, -1, null, false) && (ignoreReachability || pawn.CanReach((Thing)corpse, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn)));
			}
			else
			{
				Building_Grave building_Grave = corpse.ParentHolder as Building_Grave;
				result = (building_Grave != null && building_Grave.Spawned && pawn.CanReserve((Thing)building_Grave, 1, -1, null, false) && (ignoreReachability || pawn.CanReach((Thing)building_Grave, PathEndMode.InteractionCell, Danger.Deadly, false, TraverseMode.ByPawn)));
			}
			return result;
		}
	}
}
