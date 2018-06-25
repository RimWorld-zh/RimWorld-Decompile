using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000129 RID: 297
	public abstract class WorkGiver_ConstructDeliverResources : WorkGiver_Scanner
	{
		// Token: 0x0400030B RID: 779
		private static List<Thing> resourcesAvailable = new List<Thing>();

		// Token: 0x0400030C RID: 780
		private const float MultiPickupRadius = 5f;

		// Token: 0x0400030D RID: 781
		private const float NearbyConstructScanRadius = 8f;

		// Token: 0x0400030E RID: 782
		private static string MissingMaterialsTranslated;

		// Token: 0x0400030F RID: 783
		private static string ForbiddenLowerTranslated;

		// Token: 0x04000310 RID: 784
		private static string NoPathTranslated;

		// Token: 0x0600061A RID: 1562 RVA: 0x0004099C File Offset: 0x0003ED9C
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x000409B2 File Offset: 0x0003EDB2
		public static void ResetStaticData()
		{
			WorkGiver_ConstructDeliverResources.MissingMaterialsTranslated = "MissingMaterials".Translate();
			WorkGiver_ConstructDeliverResources.ForbiddenLowerTranslated = "ForbiddenLower".Translate();
			WorkGiver_ConstructDeliverResources.NoPathTranslated = "NoPath".Translate();
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x000409E4 File Offset: 0x0003EDE4
		private static bool ResourceValidator(Pawn pawn, ThingDefCountClass need, Thing th)
		{
			return th.def == need.thingDef && !th.IsForbidden(pawn) && pawn.CanReserve(th, 1, -1, null, false);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00040A44 File Offset: 0x0003EE44
		protected Job ResourceDeliverJobFor(Pawn pawn, IConstructible c, bool canRemoveExistingFloorUnderNearbyNeeders = true)
		{
			Blueprint_Install blueprint_Install = c as Blueprint_Install;
			Job result;
			if (blueprint_Install != null)
			{
				result = this.InstallJob(pawn, blueprint_Install);
			}
			else
			{
				bool flag = false;
				ThingDefCountClass thingDefCountClass = null;
				List<ThingDefCountClass> list = c.MaterialsNeeded();
				int count = list.Count;
				int i = 0;
				while (i < count)
				{
					ThingDefCountClass need = list[i];
					if (!pawn.Map.itemAvailability.ThingsAvailableAnywhere(need, pawn))
					{
						flag = true;
						thingDefCountClass = need;
						break;
					}
					Thing foundRes = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(need.thingDef), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Thing r) => WorkGiver_ConstructDeliverResources.ResourceValidator(pawn, need, r), null, 0, -1, false, RegionType.Set_Passable, false);
					if (foundRes != null)
					{
						int resTotalAvailable;
						this.FindAvailableNearbyResources(foundRes, pawn, out resTotalAvailable);
						int num;
						Job job;
						HashSet<Thing> hashSet = this.FindNearbyNeeders(pawn, need, c, resTotalAvailable, canRemoveExistingFloorUnderNearbyNeeders, out num, out job);
						if (job != null)
						{
							return job;
						}
						hashSet.Add((Thing)c);
						Thing thing = hashSet.MinBy((Thing nee) => IntVec3Utility.ManhattanDistanceFlat(foundRes.Position, nee.Position));
						hashSet.Remove(thing);
						int num2 = 0;
						int j = 0;
						do
						{
							num2 += WorkGiver_ConstructDeliverResources.resourcesAvailable[j].stackCount;
							j++;
						}
						while (num2 < num && j < WorkGiver_ConstructDeliverResources.resourcesAvailable.Count);
						WorkGiver_ConstructDeliverResources.resourcesAvailable.RemoveRange(j, WorkGiver_ConstructDeliverResources.resourcesAvailable.Count - j);
						WorkGiver_ConstructDeliverResources.resourcesAvailable.Remove(foundRes);
						Job job2 = new Job(JobDefOf.HaulToContainer);
						job2.targetA = foundRes;
						job2.targetQueueA = new List<LocalTargetInfo>();
						for (j = 0; j < WorkGiver_ConstructDeliverResources.resourcesAvailable.Count; j++)
						{
							job2.targetQueueA.Add(WorkGiver_ConstructDeliverResources.resourcesAvailable[j]);
						}
						job2.targetB = thing;
						if (hashSet.Count > 0)
						{
							job2.targetQueueB = new List<LocalTargetInfo>();
							foreach (Thing t in hashSet)
							{
								job2.targetQueueB.Add(t);
							}
						}
						job2.targetC = (Thing)c;
						job2.count = num;
						job2.haulMode = HaulMode.ToContainer;
						return job2;
					}
					else
					{
						flag = true;
						thingDefCountClass = need;
						i++;
					}
				}
				if (flag)
				{
					JobFailReason.Is(string.Format("{0}: {1}", WorkGiver_ConstructDeliverResources.MissingMaterialsTranslated, thingDefCountClass.thingDef.label), null);
				}
				result = null;
			}
			return result;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00040D9C File Offset: 0x0003F19C
		private void FindAvailableNearbyResources(Thing firstFoundResource, Pawn pawn, out int resTotalAvailable)
		{
			int num = Mathf.Min(firstFoundResource.def.stackLimit, pawn.carryTracker.MaxStackSpaceEver(firstFoundResource.def));
			resTotalAvailable = 0;
			WorkGiver_ConstructDeliverResources.resourcesAvailable.Clear();
			WorkGiver_ConstructDeliverResources.resourcesAvailable.Add(firstFoundResource);
			resTotalAvailable += firstFoundResource.stackCount;
			if (resTotalAvailable < num)
			{
				foreach (Thing thing in GenRadial.RadialDistinctThingsAround(firstFoundResource.Position, firstFoundResource.Map, 5f, false))
				{
					if (resTotalAvailable >= num)
					{
						break;
					}
					if (thing.def == firstFoundResource.def)
					{
						if (GenAI.CanUseItemForWork(pawn, thing))
						{
							WorkGiver_ConstructDeliverResources.resourcesAvailable.Add(thing);
							resTotalAvailable += thing.stackCount;
						}
					}
				}
			}
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00040E9C File Offset: 0x0003F29C
		private HashSet<Thing> FindNearbyNeeders(Pawn pawn, ThingDefCountClass need, IConstructible c, int resTotalAvailable, bool canRemoveExistingFloorUnderNearbyNeeders, out int neededTotal, out Job jobToMakeNeederAvailable)
		{
			neededTotal = need.count;
			HashSet<Thing> hashSet = new HashSet<Thing>();
			Thing thing = (Thing)c;
			foreach (Thing thing2 in GenRadial.RadialDistinctThingsAround(thing.Position, thing.Map, 8f, true))
			{
				if (neededTotal >= resTotalAvailable)
				{
					break;
				}
				if (this.IsNewValidNearbyNeeder(thing2, hashSet, c, pawn))
				{
					Blueprint blueprint = thing2 as Blueprint;
					if (blueprint == null || !WorkGiver_ConstructDeliverResources.ShouldRemoveExistingFloorFirst(pawn, blueprint))
					{
						int num = GenConstruct.AmountNeededByOf((IConstructible)thing2, need.thingDef);
						if (num > 0)
						{
							hashSet.Add(thing2);
							neededTotal += num;
						}
					}
				}
			}
			Blueprint blueprint2 = c as Blueprint;
			if (blueprint2 != null && blueprint2.def.entityDefToBuild is TerrainDef && canRemoveExistingFloorUnderNearbyNeeders && neededTotal < resTotalAvailable)
			{
				foreach (Thing thing3 in GenRadial.RadialDistinctThingsAround(thing.Position, thing.Map, 3f, false))
				{
					if (this.IsNewValidNearbyNeeder(thing3, hashSet, c, pawn))
					{
						Blueprint blueprint3 = thing3 as Blueprint;
						if (blueprint3 != null)
						{
							Job job = this.RemoveExistingFloorJob(pawn, blueprint3);
							if (job != null)
							{
								jobToMakeNeederAvailable = job;
								return hashSet;
							}
						}
					}
				}
			}
			jobToMakeNeederAvailable = null;
			return hashSet;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00041078 File Offset: 0x0003F478
		private bool IsNewValidNearbyNeeder(Thing t, HashSet<Thing> nearbyNeeders, IConstructible constructible, Pawn pawn)
		{
			return t is IConstructible && t != constructible && !(t is Blueprint_Install) && t.Faction == pawn.Faction && !t.IsForbidden(pawn) && !nearbyNeeders.Contains(t) && GenConstruct.CanConstruct(t, pawn, false, false);
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x000410F0 File Offset: 0x0003F4F0
		protected static bool ShouldRemoveExistingFloorFirst(Pawn pawn, Blueprint blue)
		{
			return blue.def.entityDefToBuild is TerrainDef && pawn.Map.terrainGrid.CanRemoveTopLayerAt(blue.Position);
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00041144 File Offset: 0x0003F544
		protected Job RemoveExistingFloorJob(Pawn pawn, Blueprint blue)
		{
			Job result;
			if (!WorkGiver_ConstructDeliverResources.ShouldRemoveExistingFloorFirst(pawn, blue))
			{
				result = null;
			}
			else
			{
				LocalTargetInfo target = blue.Position;
				ReservationLayerDef floor = ReservationLayerDefOf.Floor;
				if (!pawn.CanReserve(target, 1, -1, floor, false))
				{
					result = null;
				}
				else
				{
					result = new Job(JobDefOf.RemoveFloor, blue.Position)
					{
						ignoreDesignations = true
					};
				}
			}
			return result;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x000411B8 File Offset: 0x0003F5B8
		private Job InstallJob(Pawn pawn, Blueprint_Install install)
		{
			Thing miniToInstallOrBuildingToReinstall = install.MiniToInstallOrBuildingToReinstall;
			Job result;
			if (miniToInstallOrBuildingToReinstall.IsForbidden(pawn))
			{
				JobFailReason.Is(WorkGiver_ConstructDeliverResources.ForbiddenLowerTranslated, null);
				result = null;
			}
			else if (!pawn.CanReach(miniToInstallOrBuildingToReinstall, PathEndMode.ClosestTouch, pawn.NormalMaxDanger(), false, TraverseMode.ByPawn))
			{
				JobFailReason.Is(WorkGiver_ConstructDeliverResources.NoPathTranslated, null);
				result = null;
			}
			else if (!pawn.CanReserve(miniToInstallOrBuildingToReinstall, 1, -1, null, false))
			{
				Pawn pawn2 = pawn.Map.reservationManager.FirstRespectedReserver(miniToInstallOrBuildingToReinstall, pawn);
				if (pawn2 != null)
				{
					JobFailReason.Is("ReservedBy".Translate(new object[]
					{
						pawn2.LabelShort
					}), null);
				}
				result = null;
			}
			else
			{
				result = new Job(JobDefOf.HaulToContainer)
				{
					targetA = miniToInstallOrBuildingToReinstall,
					targetB = install,
					count = 1,
					haulMode = HaulMode.ToContainer
				};
			}
			return result;
		}
	}
}
