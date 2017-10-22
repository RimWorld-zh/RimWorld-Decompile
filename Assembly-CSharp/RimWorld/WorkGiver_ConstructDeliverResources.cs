using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class WorkGiver_ConstructDeliverResources : WorkGiver_Scanner
	{
		private const float MultiPickupRadius = 5f;

		private const float NearbyConstructScanRadius = 8f;

		private static List<Thing> resourcesAvailable = new List<Thing>();

		private static string MissingMaterialsTranslated;

		public WorkGiver_ConstructDeliverResources()
		{
			if (WorkGiver_ConstructDeliverResources.MissingMaterialsTranslated == null)
			{
				WorkGiver_ConstructDeliverResources.MissingMaterialsTranslated = "MissingMaterials".Translate();
			}
		}

		private static bool ResourceValidator(Pawn pawn, ThingCountClass need, Thing th)
		{
			if (th.def != need.thingDef)
			{
				return false;
			}
			if (th.IsForbidden(pawn))
			{
				return false;
			}
			if (!pawn.CanReserve(th, 1, -1, null, false))
			{
				return false;
			}
			return true;
		}

		protected Job ResourceDeliverJobFor(Pawn pawn, IConstructible c, bool canRemoveExistingFloorUnderNearbyNeeders = true)
		{
			Blueprint_Install blueprint_Install = c as Blueprint_Install;
			if (blueprint_Install != null)
			{
				return this.InstallJob(pawn, blueprint_Install);
			}
			bool flag = false;
			List<ThingCountClass> list = c.MaterialsNeeded();
			int count = list.Count;
			for (int num = 0; num < count; num++)
			{
				ThingCountClass need = list[num];
				if (!pawn.Map.itemAvailability.ThingsAvailableAnywhere(need, pawn))
				{
					flag = true;
					break;
				}
				Predicate<Thing> validator = (Predicate<Thing>)((Thing r) => WorkGiver_ConstructDeliverResources.ResourceValidator(pawn, need, r));
				Thing foundRes = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(need.thingDef), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
				if (foundRes != null)
				{
					int resTotalAvailable = default(int);
					this.FindAvailableNearbyResources(foundRes, pawn, out resTotalAvailable);
					int num2 = default(int);
					Job job = default(Job);
					HashSet<Thing> hashSet = this.FindNearbyNeeders(pawn, need, c, resTotalAvailable, canRemoveExistingFloorUnderNearbyNeeders, out num2, out job);
					if (job != null)
					{
						return job;
					}
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
						HashSet<Thing>.Enumerator enumerator = hashSet.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								Thing current = enumerator.Current;
								job2.targetQueueB.Add(current);
							}
						}
						finally
						{
							((IDisposable)(object)enumerator).Dispose();
						}
					}
					job2.targetC = (Thing)c;
					job2.count = num2;
					job2.haulMode = HaulMode.ToContainer;
					return job2;
				}
				flag = true;
			}
			if (flag)
			{
				JobFailReason.Is(WorkGiver_ConstructDeliverResources.MissingMaterialsTranslated);
			}
			return null;
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
			if (t is IConstructible && t != constructible && !(t is Blueprint_Install) && t.Faction == pawn.Faction && !t.IsForbidden(pawn) && !nearbyNeeders.Contains(t) && GenConstruct.CanConstruct(t, pawn, false))
			{
				return true;
			}
			return false;
		}

		private static bool ShouldRemoveExistingFloorFirst(Pawn pawn, Blueprint blue)
		{
			if (!(blue.def.entityDefToBuild is TerrainDef))
			{
				return false;
			}
			if (!pawn.Map.terrainGrid.CanRemoveTopLayerAt(blue.Position))
			{
				return false;
			}
			return true;
		}

		protected Job RemoveExistingFloorJob(Pawn pawn, Blueprint blue)
		{
			if (!WorkGiver_ConstructDeliverResources.ShouldRemoveExistingFloorFirst(pawn, blue))
			{
				return null;
			}
			ReservationLayerDef floor = ReservationLayerDefOf.Floor;
			if (!pawn.CanReserve(blue.Position, 1, -1, floor, false))
			{
				return null;
			}
			Job job = new Job(JobDefOf.RemoveFloor, blue.Position);
			job.ignoreDesignations = true;
			return job;
		}

		private Job InstallJob(Pawn pawn, Blueprint_Install install)
		{
			Thing miniToInstallOrBuildingToReinstall = install.MiniToInstallOrBuildingToReinstall;
			if (!miniToInstallOrBuildingToReinstall.IsForbidden(pawn) && pawn.CanReserveAndReach(miniToInstallOrBuildingToReinstall, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false))
			{
				Job job = new Job(JobDefOf.HaulToContainer);
				job.targetA = miniToInstallOrBuildingToReinstall;
				job.targetB = (Thing)install;
				job.count = 1;
				job.haulMode = HaulMode.ToContainer;
				return job;
			}
			return null;
		}
	}
}
