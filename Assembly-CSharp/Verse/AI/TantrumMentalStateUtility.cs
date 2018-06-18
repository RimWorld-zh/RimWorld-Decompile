using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A8C RID: 2700
	public static class TantrumMentalStateUtility
	{
		// Token: 0x06003BDD RID: 15325 RVA: 0x001F8960 File Offset: 0x001F6D60
		public static bool CanSmash(Pawn pawn, Thing thing, bool skipReachabilityCheck = false, Predicate<Thing> customValidator = null, int extraMinBuildingOrItemMarketValue = 0)
		{
			if (customValidator != null)
			{
				if (!customValidator(thing))
				{
					return false;
				}
			}
			else if (!thing.def.IsBuildingArtificial && thing.def.category != ThingCategory.Item)
			{
				return false;
			}
			return !thing.Destroyed && thing.Spawned && thing != pawn && (thing.def.category == ThingCategory.Pawn || thing.def.useHitPoints) && (thing.def.category == ThingCategory.Pawn || !thing.def.CanHaveFaction || thing.Faction == pawn.Faction) && (thing.def.category != ThingCategory.Item || thing.MarketValue * (float)thing.stackCount >= 75f) && (thing.def.category != ThingCategory.Pawn || !((Pawn)thing).Downed) && ((thing.def.category != ThingCategory.Item && thing.def.category != ThingCategory.Building) || thing.MarketValue * (float)thing.stackCount >= (float)extraMinBuildingOrItemMarketValue) && (skipReachabilityCheck || pawn.CanReach(thing, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn));
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x001F8ACC File Offset: 0x001F6ECC
		public static void GetSmashableThingsNear(Pawn pawn, IntVec3 near, List<Thing> outCandidates, Predicate<Thing> customValidator = null, int extraMinBuildingOrItemMarketValue = 0, int maxDistance = 40)
		{
			outCandidates.Clear();
			if (pawn.CanReach(near, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				Region region = near.GetRegion(pawn.Map, RegionType.Set_Passable);
				if (region != null)
				{
					TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
					RegionTraverser.BreadthFirstTraverse(region, (Region from, Region to) => to.Allows(traverseParams, false), delegate(Region r)
					{
						List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
						for (int i = 0; i < list.Count; i++)
						{
							if (list[i].Position.InHorDistOf(near, (float)maxDistance) && TantrumMentalStateUtility.CanSmash(pawn, list[i], true, customValidator, extraMinBuildingOrItemMarketValue))
							{
								outCandidates.Add(list[i]);
							}
						}
						List<Thing> list2 = r.ListerThings.ThingsInGroup(ThingRequestGroup.HaulableEver);
						for (int j = 0; j < list2.Count; j++)
						{
							if (list2[j].Position.InHorDistOf(near, (float)maxDistance) && TantrumMentalStateUtility.CanSmash(pawn, list2[j], true, customValidator, extraMinBuildingOrItemMarketValue))
							{
								outCandidates.Add(list2[j]);
							}
						}
						List<Thing> list3 = r.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
						for (int k = 0; k < list3.Count; k++)
						{
							if (list3[k].Position.InHorDistOf(near, (float)maxDistance) && TantrumMentalStateUtility.CanSmash(pawn, list3[k], true, customValidator, extraMinBuildingOrItemMarketValue))
							{
								outCandidates.Add(list3[k]);
							}
						}
						return false;
					}, 40, RegionType.Set_Passable);
				}
			}
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x001F8B94 File Offset: 0x001F6F94
		public static void GetSmashableThingsIn(Room room, Pawn pawn, List<Thing> outCandidates, Predicate<Thing> customValidator = null, int extraMinBuildingOrItemMarketValue = 0)
		{
			outCandidates.Clear();
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (TantrumMentalStateUtility.CanSmash(pawn, thing, false, customValidator, extraMinBuildingOrItemMarketValue))
				{
					outCandidates.Add(containedAndAdjacentThings[i]);
				}
			}
		}

		// Token: 0x06003BE0 RID: 15328 RVA: 0x001F8BF4 File Offset: 0x001F6FF4
		public static bool CanAttackPrisoner(Pawn pawn, Thing prisoner)
		{
			Pawn pawn2 = prisoner as Pawn;
			return pawn2 != null && pawn2.IsPrisoner && !pawn2.Downed && pawn2.HostFaction == pawn.Faction;
		}

		// Token: 0x0400258B RID: 9611
		private const int MaxRegionsToSearch = 40;

		// Token: 0x0400258C RID: 9612
		private const int AbsoluteMinItemMarketValue = 75;
	}
}
