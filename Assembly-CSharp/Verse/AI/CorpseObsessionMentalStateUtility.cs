using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A59 RID: 2649
	public static class CorpseObsessionMentalStateUtility
	{
		// Token: 0x06003AF4 RID: 15092 RVA: 0x001F43FC File Offset: 0x001F27FC
		public static Corpse GetClosestCorpseToDigUp(Pawn pawn)
		{
			Corpse result;
			if (!pawn.Spawned)
			{
				result = null;
			}
			else
			{
				Building_Grave building_Grave = (Building_Grave)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Grave), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, delegate(Thing x)
				{
					Building_Grave building_Grave2 = (Building_Grave)x;
					return building_Grave2.HasCorpse && CorpseObsessionMentalStateUtility.IsCorpseValid(building_Grave2.Corpse, pawn, true);
				}, null, 0, -1, false, RegionType.Set_Passable, false);
				result = ((building_Grave == null) ? null : building_Grave.Corpse);
			}
			return result;
		}

		// Token: 0x06003AF5 RID: 15093 RVA: 0x001F4498 File Offset: 0x001F2898
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
				result = (pawn.CanReserve(corpse, 1, -1, null, false) && (ignoreReachability || pawn.CanReach(corpse, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn)));
			}
			else
			{
				Building_Grave building_Grave = corpse.ParentHolder as Building_Grave;
				result = (building_Grave != null && building_Grave.Spawned && pawn.CanReserve(building_Grave, 1, -1, null, false) && (ignoreReachability || pawn.CanReach(building_Grave, PathEndMode.InteractionCell, Danger.Deadly, false, TraverseMode.ByPawn)));
			}
			return result;
		}
	}
}
