using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class WorkGiver_ConstructDeliverResources : WorkGiver_Scanner
	{
		private static List<Thing> resourcesAvailable = new List<Thing>();

		private const float MultiPickupRadius = 5f;

		private const float NearbyConstructScanRadius = 8f;

		private static string MissingMaterialsTranslated;

		public WorkGiver_ConstructDeliverResources()
		{
			if (WorkGiver_ConstructDeliverResources.MissingMaterialsTranslated == null)
			{
				WorkGiver_ConstructDeliverResources.MissingMaterialsTranslated = "MissingMaterials".Translate();
			}
		}

		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		private static bool ResourceValidator(Pawn pawn, ThingCountClass need, Thing th)
		{
			return (byte)((th.def == need.thingDef) ? ((!th.IsForbidden(pawn)) ? (pawn.CanReserve(th, 1, -1, null, false) ? 1 : 0) : 0) : 0) != 0;
		}

		protected Job ResourceDeliverJobFor(Pawn pawn, IConstructible c, bool canRemoveExistingFloorUnderNearbyNeeders = true)
		{
			Blueprint_Install blueprint_Install = c as Blueprint_Install;
			Job result;
			ThingCountClass need;
			Thing foundRes;
			if (blueprint_Install != null)
			{
				result = this.InstallJob(pawn, blueprint_Install);
			}
			else
			{
				bool flag = false;
				ThingCountClass thingCountClass = null;
				List<ThingCountClass> list = c.MaterialsNeeded();
				int count = list.Count;
				int num = 0;
				while (true)
				{
					if (num < count)
					{
						need = list[num];
						if (!pawn.Map.itemAvailability.ThingsAvailableAnywhere(need, pawn))
						{
							flag = true;
							thingCountClass = need;
							goto IL_030a;
						}
						foundRes = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(need.thingDef), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Predicate<Thing>)((Thing r) => WorkGiver_ConstructDeliverResources.ResourceValidator(pawn, need, r)), null, 0, -1, false, RegionType.Set_Passable, false);
						if (foundRes != null)
							goto IL_010a;
						flag = true;
						thingCountClass = need;
						num++;
						continue;
					}
					goto IL_030a;
					IL_030a:
					if (flag)
					{
						JobFailReason.Is(string.Format("{0}: {1}", WorkGiver_ConstructDeliverResources.MissingMaterialsTranslated, thingCountClass.thingDef.label));
					}
					result = null;
					break;
				}
			}
			goto IL_0337;
			IL_0337:
			return result;
			IL_010a:
			int resTotalAvailable = default(int);
			this.FindAvailableNearbyResources(foundRes, pawn, out resTotalAvailable);
			int num2 = default(int);
			Job job = default(Job);
			HashSet<Thing> hashSet = this.FindNearbyNeeders(pawn, need, c, resTotalAvailable, canRemoveExistingFloorUnderNearbyNeeders, out num2, out job);
			if (job != null)
			{
				result = job;
			}
			else
			{
				hashSet.Add((Thing)c);
				Thing thing = hashSet.MinBy((Func<Thing, int>)((Thing nee) => IntVec3Utility.ManhattanDistanceFlat(foundRes.Position, nee.Position)));
				hashSet.Remove(thing);
				int num3 = 0;
				int num4 = 0;
				while (true)
				{
					num3 += WorkGiver_ConstructDeliverResources.resourcesAvailable[num4].stackCount;
					num4++;
					if (num3 >= num2)
						break;
					if (num4 >= WorkGiver_ConstructDeliverResources.resourcesAvailable.Count)
						break;
				}
				WorkGiver_ConstructDeliverResources.resourcesAvailable.RemoveRange(num4, WorkGiver_ConstructDeliverResources.resourcesAvailable.Count - num4);
				WorkGiver_ConstructDeliverResources.resourcesAvailable.Remove(foundRes);
				Job job2 = new Job(JobDefOf.HaulToContainer);
				job2.targetA = foundRes;
				job2.targetQueueA = new List<LocalTargetInfo>();
				for (num4 = 0; num4 < WorkGiver_ConstructDeliverResources.resourcesAvailable.Count; num4++)
				{
					job2.targetQueueA.Add(WorkGiver_ConstructDeliverResources.resourcesAvailable[num4]);
				}
				job2.targetB = thing;
				if (hashSet.Count > 0)
				{
					job2.targetQueueB = new List<LocalTargetInfo>();
					foreach (Thing item in hashSet)
					{
						job2.targetQueueB.Add(item);
					}
				}
				job2.targetC = (Thing)c;
				job2.count = num2;
				job2.haulMode = HaulMode.ToContainer;
				result = job2;
			}
			goto IL_0337;
		}

		private void FindAvailableNearbyResources(Thing firstFoundResource, Pawn pawn, out int resTotalAvailable)
		{
			int num = Mathf.Min(firstFoundResource.def.stackLimit, pawn.carryTracker.MaxStackSpaceEver(firstFoundResource.def));
			resTotalAvailable = 0;
			WorkGiver_ConstructDeliverResources.resourcesAvailable.Clear();
			WorkGiver_ConstructDeliverResources.resourcesAvailable.Add(firstFoundResource);
			resTotalAvailable += firstFoundResource.stackCount;
			if (resTotalAvailable < num)
			{
				foreach (Thing item in GenRadial.RadialDistinctThingsAround(firstFoundResource.Position, firstFoundResource.Map, 5f, false))
				{
					if (resTotalAvailable < num)
					{
						if (item.def == firstFoundResource.def && GenAI.CanUseItemForWork(pawn, item))
						{
							WorkGiver_ConstructDeliverResources.resourcesAvailable.Add(item);
							resTotalAvailable += item.stackCount;
						}
						continue;
					}
					break;
				}
			}
		}

		private HashSet<Thing> FindNearbyNeeders(Pawn pawn, ThingCountClass need, IConstructible c, int resTotalAvailable, bool canRemoveExistingFloorUnderNearbyNeeders, out int neededTotal, out Job jobToMakeNeederAvailable)
		{
			neededTotal = need.count;
			HashSet<Thing> hashSet = new HashSet<Thing>();
			Thing thing = (Thing)c;
			foreach (Thing item in GenRadial.RadialDistinctThingsAround(thing.Position, thing.Map, 8f, false))
			{
				if (neededTotal < resTotalAvailable)
				{
					if (this.IsNewValidNearbyNeeder(item, hashSet, c, pawn))
					{
						Blueprint blueprint = item as Blueprint;
						if (blueprint == null || !WorkGiver_ConstructDeliverResources.ShouldRemoveExistingFloorFirst(pawn, blueprint))
						{
							int num = GenConstruct.AmountNeededByOf((IConstructible)item, need.thingDef);
							if (num > 0)
							{
								hashSet.Add(item);
								neededTotal += num;
							}
						}
					}
					continue;
				}
				break;
			}
			Blueprint blueprint2 = c as Blueprint;
			if (blueprint2 != null && blueprint2.def.entityDefToBuild is TerrainDef && canRemoveExistingFloorUnderNearbyNeeders && neededTotal < resTotalAvailable)
			{
				foreach (Thing item2 in GenRadial.RadialDistinctThingsAround(thing.Position, thing.Map, 3f, false))
				{
					if (this.IsNewValidNearbyNeeder(item2, hashSet, c, pawn))
					{
						Blueprint blueprint3 = item2 as Blueprint;
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

		private bool IsNewValidNearbyNeeder(Thing t, HashSet<Thing> nearbyNeeders, IConstructible constructible, Pawn pawn)
		{
			bool result;
			if (!(t is IConstructible) || t == constructible || t is Blueprint_Install || t.Faction != pawn.Faction || t.IsForbidden(pawn) || nearbyNeeders.Contains(t) || !GenConstruct.CanConstruct(t, pawn, false))
			{
				result = false;
			}
			else
			{
				Blueprint blueprint = t as Blueprint;
				if (blueprint != null)
				{
					if (blueprint.FirstBlockingThing(pawn, null, false) != null)
					{
						result = false;
						goto IL_009f;
					}
					if (WorkGiver_ConstructDeliverResourcesToBlueprints.GetFirstBlockingBuilding(blueprint, pawn) != null)
					{
						result = false;
						goto IL_009f;
					}
				}
				result = true;
			}
			goto IL_009f;
			IL_009f:
			return result;
		}

		private static bool ShouldRemoveExistingFloorFirst(Pawn pawn, Blueprint blue)
		{
			return (byte)((blue.def.entityDefToBuild is TerrainDef) ? (pawn.Map.terrainGrid.CanRemoveTopLayerAt(blue.Position) ? 1 : 0) : 0) != 0;
		}

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
					Job job = new Job(JobDefOf.RemoveFloor, blue.Position);
					job.ignoreDesignations = true;
					result = job;
				}
			}
			return result;
		}

		private Job InstallJob(Pawn pawn, Blueprint_Install install)
		{
			Thing miniToInstallOrBuildingToReinstall = install.MiniToInstallOrBuildingToReinstall;
			Job result;
			if (miniToInstallOrBuildingToReinstall.IsForbidden(pawn) || !pawn.CanReserveAndReach(miniToInstallOrBuildingToReinstall, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false))
			{
				result = null;
			}
			else
			{
				Job job = new Job(JobDefOf.HaulToContainer);
				job.targetA = miniToInstallOrBuildingToReinstall;
				job.targetB = (Thing)install;
				job.count = 1;
				job.haulMode = HaulMode.ToContainer;
				result = job;
			}
			return result;
		}
	}
}
