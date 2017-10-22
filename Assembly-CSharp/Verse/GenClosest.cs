using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	public static class GenClosest
	{
		private const int DefaultLocalTraverseRegionsBeforeGlobal = 30;

		private static bool EarlyOutSearch(IntVec3 start, Map map, ThingRequest thingReq, IEnumerable<Thing> customGlobalSearchSet)
		{
			bool result;
			if (thingReq.group == ThingRequestGroup.Everything)
			{
				Log.Error("Cannot do ClosestThingReachable searching everything without restriction.");
				result = true;
			}
			else if (!start.InBounds(map))
			{
				Log.Error("Did FindClosestThing with start out of bounds (" + start + "), thingReq=" + thingReq);
				result = true;
			}
			else
			{
				result = ((byte)((thingReq.group == ThingRequestGroup.Nothing) ? 1 : ((customGlobalSearchSet == null && !thingReq.IsUndefined && map.listerThings.ThingsMatching(thingReq).Count == 0) ? 1 : 0)) != 0);
			}
			return result;
		}

		public static Thing ClosestThingReachable(IntVec3 root, Map map, ThingRequest thingReq, PathEndMode peMode, TraverseParms traverseParams, float maxDistance = 9999f, Predicate<Thing> validator = null, IEnumerable<Thing> customGlobalSearchSet = null, int searchRegionsMin = 0, int searchRegionsMax = -1, bool forceGlobalSearch = false, RegionType traversableRegionTypes = RegionType.Set_Passable, bool ignoreEntirelyForbiddenRegions = false)
		{
			if (searchRegionsMax > 0 && customGlobalSearchSet != null && !forceGlobalSearch)
			{
				Log.ErrorOnce("searchRegionsMax > 0 && customGlobalSearchSet != null && !forceGlobalSearch. customGlobalSearchSet will never be used.", 634984);
			}
			Thing result;
			if (GenClosest.EarlyOutSearch(root, map, thingReq, customGlobalSearchSet))
			{
				result = null;
			}
			else
			{
				Thing thing = null;
				if (!thingReq.IsUndefined)
				{
					int maxRegions = (searchRegionsMax <= 0) ? 30 : searchRegionsMax;
					thing = GenClosest.RegionwiseBFSWorker(root, map, thingReq, peMode, traverseParams, validator, null, searchRegionsMin, maxRegions, maxDistance, traversableRegionTypes, ignoreEntirelyForbiddenRegions);
				}
				if (thing == null && (searchRegionsMax < 0 || forceGlobalSearch))
				{
					if (traversableRegionTypes != RegionType.Set_Passable)
					{
						Log.ErrorOnce("ClosestThingReachable had to do a global search, but traversableRegionTypes is not set to passable only. It's not supported, because Reachability is based on passable regions only.", 14384767);
					}
					Predicate<Thing> validator2 = (Predicate<Thing>)((Thing t) => (byte)(map.reachability.CanReach(root, t, peMode, traverseParams) ? (((object)validator == null || validator(t)) ? 1 : 0) : 0) != 0);
					IEnumerable<Thing> searchSet = customGlobalSearchSet ?? map.listerThings.ThingsMatching(thingReq);
					thing = GenClosest.ClosestThing_Global(root, searchSet, maxDistance, validator2, null);
				}
				result = thing;
			}
			return result;
		}

		public static Thing ClosestThing_Regionwise_ReachablePrioritized(IntVec3 root, Map map, ThingRequest thingReq, PathEndMode peMode, TraverseParms traverseParams, float maxDistance = 9999f, Predicate<Thing> validator = null, Func<Thing, float> priorityGetter = null, int minRegions = 24, int maxRegions = 30)
		{
			Thing result;
			if (GenClosest.EarlyOutSearch(root, map, thingReq, null))
			{
				result = null;
			}
			else
			{
				if (maxRegions < minRegions)
				{
					Log.ErrorOnce("maxRegions < minRegions", 754343);
				}
				Thing thing = null;
				if (!thingReq.IsUndefined)
				{
					thing = GenClosest.RegionwiseBFSWorker(root, map, thingReq, peMode, traverseParams, validator, priorityGetter, minRegions, maxRegions, maxDistance, RegionType.Set_Passable, false);
				}
				result = thing;
			}
			return result;
		}

		public static Thing RegionwiseBFSWorker(IntVec3 root, Map map, ThingRequest req, PathEndMode peMode, TraverseParms traverseParams, Predicate<Thing> validator, Func<Thing, float> priorityGetter, int minRegions, int maxRegions, float maxDistance, RegionType traversableRegionTypes = RegionType.Set_Passable, bool ignoreEntirelyForbiddenRegions = false)
		{
			Thing result;
			if (traverseParams.mode == TraverseMode.PassAllDestroyableThings)
			{
				Log.Error("RegionwiseBFSWorker with traverseParams.mode PassAllDestroyableThings. Use ClosestThingGlobal.");
				result = null;
			}
			else
			{
				Region region = root.GetRegion(map, traversableRegionTypes);
				if (region == null)
				{
					result = null;
				}
				else
				{
					float maxDistSquared = maxDistance * maxDistance;
					RegionEntryPredicate entryCondition = (RegionEntryPredicate)((Region from, Region to) => to.Allows(traverseParams, false) && (maxDistance > 5000.0 || to.extentsClose.ClosestDistSquaredTo(root) < maxDistSquared));
					Thing closestThing = null;
					float closestDistSquared = 9999999f;
					float bestPrio = -3.40282347E+38f;
					int regionsSeen = 0;
					RegionProcessor regionProcessor = (RegionProcessor)delegate(Region r)
					{
						bool result2;
						if (r.portal == null && !r.Allows(traverseParams, true))
						{
							result2 = false;
						}
						else
						{
							if (!ignoreEntirelyForbiddenRegions || !r.IsForbiddenEntirely(traverseParams.pawn))
							{
								List<Thing> list = r.ListerThings.ThingsMatching(req);
								for (int i = 0; i < list.Count; i++)
								{
									Thing thing = list[i];
									if (ReachabilityWithinRegion.ThingFromRegionListerReachable(thing, r, peMode, traverseParams.pawn))
									{
										float num = (float)(((object)priorityGetter == null) ? 0.0 : priorityGetter(thing));
										if (!(num < bestPrio))
										{
											float num2 = (float)(thing.Position - root).LengthHorizontalSquared;
											if ((num > bestPrio || num2 < closestDistSquared) && num2 < maxDistSquared && ((object)validator == null || validator(thing)))
											{
												closestThing = thing;
												closestDistSquared = num2;
												bestPrio = num;
											}
										}
									}
								}
							}
							regionsSeen++;
							result2 = (regionsSeen >= minRegions && closestThing != null);
						}
						return result2;
					};
					RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
					result = closestThing;
				}
			}
			return result;
		}

		public static Thing ClosestThing_Global(IntVec3 center, IEnumerable searchSet, float maxDistance = 99999f, Predicate<Thing> validator = null, Func<Thing, float> priorityGetter = null)
		{
			Thing result;
			if (searchSet == null)
			{
				result = null;
			}
			else
			{
				float num = 2.14748365E+09f;
				Thing thing = null;
				float num2 = -3.40282347E+38f;
				float num3 = maxDistance * maxDistance;
				IEnumerator enumerator = searchSet.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Thing thing2 = (Thing)enumerator.Current;
						float num4;
						float num5;
						if (thing2.Spawned)
						{
							num4 = (float)(center - thing2.Position).LengthHorizontalSquared;
							if (!(num4 > num3) && ((object)priorityGetter != null || num4 < num) && ((object)validator == null || validator(thing2)))
							{
								num5 = 0f;
								if ((object)priorityGetter != null)
								{
									num5 = priorityGetter(thing2);
									if (!(num5 < num2) && (num5 != num2 || !(num4 >= num)))
									{
										goto IL_00e2;
									}
									continue;
								}
								goto IL_00e2;
							}
						}
						continue;
						IL_00e2:
						thing = thing2;
						num = num4;
						num2 = num5;
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				result = thing;
			}
			return result;
		}

		public static Thing ClosestThing_Global_Reachable(IntVec3 center, Map map, IEnumerable<Thing> searchSet, PathEndMode peMode, TraverseParms traverseParams, float maxDistance = 9999f, Predicate<Thing> validator = null, Func<Thing, float> priorityGetter = null)
		{
			Thing result;
			if (searchSet == null)
			{
				result = null;
			}
			else
			{
				int num = 0;
				int num2 = 0;
				Thing thing = null;
				float num3 = -3.40282347E+38f;
				float num4 = maxDistance * maxDistance;
				float num5 = 2.14748365E+09f;
				foreach (Thing item in searchSet)
				{
					float num6;
					float num7;
					if (item.Spawned)
					{
						num2++;
						num6 = (float)(center - item.Position).LengthHorizontalSquared;
						if (!(num6 > num4) && ((object)priorityGetter != null || num6 < num5) && map.reachability.CanReach(center, item, peMode, traverseParams) && ((object)validator == null || validator(item)))
						{
							num7 = 0f;
							if ((object)priorityGetter != null)
							{
								num7 = priorityGetter(item);
								if (!(num7 < num3) && (num7 != num3 || !(num6 >= num5)))
								{
									goto IL_010f;
								}
								continue;
							}
							goto IL_010f;
						}
					}
					continue;
					IL_010f:
					thing = item;
					num5 = num6;
					num3 = num7;
					num++;
				}
				result = thing;
			}
			return result;
		}
	}
}
