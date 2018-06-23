using System;
using System.Collections;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000F31 RID: 3889
	public static class GenClosest
	{
		// Token: 0x04003DCD RID: 15821
		private const int DefaultLocalTraverseRegionsBeforeGlobal = 30;

		// Token: 0x06005DAA RID: 23978 RVA: 0x002F91A0 File Offset: 0x002F75A0
		private static bool EarlyOutSearch(IntVec3 start, Map map, ThingRequest thingReq, IEnumerable<Thing> customGlobalSearchSet, Predicate<Thing> validator)
		{
			bool result;
			if (thingReq.group == ThingRequestGroup.Everything)
			{
				Log.Error("Cannot do ClosestThingReachable searching everything without restriction.", false);
				result = true;
			}
			else if (!start.InBounds(map))
			{
				Log.Error(string.Concat(new object[]
				{
					"Did FindClosestThing with start out of bounds (",
					start,
					"), thingReq=",
					thingReq
				}), false);
				result = true;
			}
			else
			{
				result = (thingReq.group == ThingRequestGroup.Nothing || (customGlobalSearchSet == null && !thingReq.IsUndefined && map.listerThings.ThingsMatching(thingReq).Count == 0));
			}
			return result;
		}

		// Token: 0x06005DAB RID: 23979 RVA: 0x002F9260 File Offset: 0x002F7660
		public static Thing ClosestThingReachable(IntVec3 root, Map map, ThingRequest thingReq, PathEndMode peMode, TraverseParms traverseParams, float maxDistance = 9999f, Predicate<Thing> validator = null, IEnumerable<Thing> customGlobalSearchSet = null, int searchRegionsMin = 0, int searchRegionsMax = -1, bool forceGlobalSearch = false, RegionType traversableRegionTypes = RegionType.Set_Passable, bool ignoreEntirelyForbiddenRegions = false)
		{
			bool flag = searchRegionsMax < 0 || forceGlobalSearch;
			if (!flag && customGlobalSearchSet != null)
			{
				Log.ErrorOnce("searchRegionsMax >= 0 && customGlobalSearchSet != null && !forceGlobalSearch. customGlobalSearchSet will never be used.", 634984, false);
			}
			Thing result;
			if (!flag && !thingReq.IsUndefined && !thingReq.CanBeFoundInRegion)
			{
				Log.ErrorOnce("ClosestThingReachable with thing request group " + thingReq.group + " and global search not allowed. This will never find anything because this group is never stored in regions. Either allow global search or don't call this method at all.", 518498981, false);
				result = null;
			}
			else if (GenClosest.EarlyOutSearch(root, map, thingReq, customGlobalSearchSet, validator))
			{
				result = null;
			}
			else
			{
				Thing thing = null;
				bool flag2 = false;
				if (!thingReq.IsUndefined && thingReq.CanBeFoundInRegion)
				{
					int num = (searchRegionsMax <= 0) ? 30 : searchRegionsMax;
					int num2;
					thing = GenClosest.RegionwiseBFSWorker(root, map, thingReq, peMode, traverseParams, validator, null, searchRegionsMin, num, maxDistance, out num2, traversableRegionTypes, ignoreEntirelyForbiddenRegions);
					flag2 = (thing == null && num2 < num);
				}
				if (thing == null && flag && !flag2)
				{
					if (traversableRegionTypes != RegionType.Set_Passable)
					{
						Log.ErrorOnce("ClosestThingReachable had to do a global search, but traversableRegionTypes is not set to passable only. It's not supported, because Reachability is based on passable regions only.", 14384767, false);
					}
					Predicate<Thing> validator2 = (Thing t) => map.reachability.CanReach(root, t, peMode, traverseParams) && (validator == null || validator(t));
					IEnumerable<Thing> searchSet = customGlobalSearchSet ?? map.listerThings.ThingsMatching(thingReq);
					thing = GenClosest.ClosestThing_Global(root, searchSet, maxDistance, validator2, null);
				}
				result = thing;
			}
			return result;
		}

		// Token: 0x06005DAC RID: 23980 RVA: 0x002F9420 File Offset: 0x002F7820
		public static Thing ClosestThing_Regionwise_ReachablePrioritized(IntVec3 root, Map map, ThingRequest thingReq, PathEndMode peMode, TraverseParms traverseParams, float maxDistance = 9999f, Predicate<Thing> validator = null, Func<Thing, float> priorityGetter = null, int minRegions = 24, int maxRegions = 30)
		{
			Thing result;
			if (!thingReq.IsUndefined && !thingReq.CanBeFoundInRegion)
			{
				Log.ErrorOnce("ClosestThing_Regionwise_ReachablePrioritized with thing request group " + thingReq.group + ". This will never find anything because this group is never stored in regions. Most likely a global search should have been used.", 738476712, false);
				result = null;
			}
			else if (GenClosest.EarlyOutSearch(root, map, thingReq, null, validator))
			{
				result = null;
			}
			else
			{
				if (maxRegions < minRegions)
				{
					Log.ErrorOnce("maxRegions < minRegions", 754343, false);
				}
				Thing thing = null;
				if (!thingReq.IsUndefined)
				{
					int num;
					thing = GenClosest.RegionwiseBFSWorker(root, map, thingReq, peMode, traverseParams, validator, priorityGetter, minRegions, maxRegions, maxDistance, out num, RegionType.Set_Passable, false);
				}
				result = thing;
			}
			return result;
		}

		// Token: 0x06005DAD RID: 23981 RVA: 0x002F94D8 File Offset: 0x002F78D8
		public static Thing RegionwiseBFSWorker(IntVec3 root, Map map, ThingRequest req, PathEndMode peMode, TraverseParms traverseParams, Predicate<Thing> validator, Func<Thing, float> priorityGetter, int minRegions, int maxRegions, float maxDistance, out int regionsSeen, RegionType traversableRegionTypes = RegionType.Set_Passable, bool ignoreEntirelyForbiddenRegions = false)
		{
			regionsSeen = 0;
			Thing result;
			if (traverseParams.mode == TraverseMode.PassAllDestroyableThings)
			{
				Log.Error("RegionwiseBFSWorker with traverseParams.mode PassAllDestroyableThings. Use ClosestThingGlobal.", false);
				result = null;
			}
			else if (traverseParams.mode == TraverseMode.PassAllDestroyableThingsNotWater)
			{
				Log.Error("RegionwiseBFSWorker with traverseParams.mode PassAllDestroyableThingsNotWater. Use ClosestThingGlobal.", false);
				result = null;
			}
			else if (!req.IsUndefined && !req.CanBeFoundInRegion)
			{
				Log.ErrorOnce("RegionwiseBFSWorker with thing request group " + req.group + ". This group is never stored in regions. Most likely a global search should have been used.", 385766189, false);
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
					RegionEntryPredicate entryCondition = (Region from, Region to) => to.Allows(traverseParams, false) && (maxDistance > 5000f || to.extentsClose.ClosestDistSquaredTo(root) < maxDistSquared);
					Thing closestThing = null;
					float closestDistSquared = 9999999f;
					float bestPrio = float.MinValue;
					int regionsSeenScan = 0;
					RegionProcessor regionProcessor = delegate(Region r)
					{
						regionsSeenScan++;
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
										float num = (priorityGetter == null) ? 0f : priorityGetter(thing);
										if (num >= bestPrio)
										{
											float num2 = (float)(thing.Position - root).LengthHorizontalSquared;
											if ((num > bestPrio || num2 < closestDistSquared) && num2 < maxDistSquared)
											{
												if (validator == null || validator(thing))
												{
													closestThing = thing;
													closestDistSquared = num2;
													bestPrio = num;
												}
											}
										}
									}
								}
							}
							result2 = (regionsSeenScan >= minRegions && closestThing != null);
						}
						return result2;
					};
					RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
					regionsSeen = regionsSeenScan;
					result = closestThing;
				}
			}
			return result;
		}

		// Token: 0x06005DAE RID: 23982 RVA: 0x002F9664 File Offset: 0x002F7A64
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
				float num2 = float.MinValue;
				float num3 = maxDistance * maxDistance;
				IEnumerator enumerator = searchSet.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Thing thing2 = (Thing)obj;
						if (thing2.Spawned)
						{
							float num4 = (float)(center - thing2.Position).LengthHorizontalSquared;
							if (num4 <= num3)
							{
								if (priorityGetter != null || num4 < num)
								{
									if (validator != null)
									{
										if (!validator(thing2))
										{
											continue;
										}
									}
									float num5 = 0f;
									if (priorityGetter != null)
									{
										num5 = priorityGetter(thing2);
										if (num5 < num2)
										{
											continue;
										}
										if (num5 == num2 && num4 >= num)
										{
											continue;
										}
									}
									thing = thing2;
									num = num4;
									num2 = num5;
								}
							}
						}
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

		// Token: 0x06005DAF RID: 23983 RVA: 0x002F97A0 File Offset: 0x002F7BA0
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
				float num3 = float.MinValue;
				float num4 = maxDistance * maxDistance;
				float num5 = 2.14748365E+09f;
				foreach (Thing thing2 in searchSet)
				{
					if (thing2.Spawned)
					{
						num2++;
						float num6 = (float)(center - thing2.Position).LengthHorizontalSquared;
						if (num6 <= num4)
						{
							if (priorityGetter != null || num6 < num5)
							{
								if (map.reachability.CanReach(center, thing2, peMode, traverseParams))
								{
									if (validator != null)
									{
										if (!validator(thing2))
										{
											continue;
										}
									}
									float num7 = 0f;
									if (priorityGetter != null)
									{
										num7 = priorityGetter(thing2);
										if (num7 < num3)
										{
											continue;
										}
										if (num7 == num3 && num6 >= num5)
										{
											continue;
										}
									}
									thing = thing2;
									num5 = num6;
									num3 = num7;
									num++;
								}
							}
						}
					}
				}
				result = thing;
			}
			return result;
		}
	}
}
