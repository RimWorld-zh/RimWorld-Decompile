using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class DropPodUtility
	{
		private static List<List<Thing>> tempList = new List<List<Thing>>();

		public static void MakeDropPodAt(IntVec3 c, Map map, ActiveDropPodInfo info)
		{
			DropPodIncoming dropPodIncoming = (DropPodIncoming)ThingMaker.MakeThing(ThingDefOf.DropPodIncoming, null);
			dropPodIncoming.Contents = info;
			GenSpawn.Spawn(dropPodIncoming, c, map);
		}

		public static void DropThingsNear(IntVec3 dropCenter, Map map, IEnumerable<Thing> things, int openDelay = 110, bool canInstaDropDuringInit = false, bool leaveSlag = false, bool canRoofPunch = true)
		{
			foreach (Thing item in things)
			{
				List<Thing> list = new List<Thing>();
				list.Add(item);
				DropPodUtility.tempList.Add(list);
			}
			DropPodUtility.DropThingGroupsNear(dropCenter, map, DropPodUtility.tempList, openDelay, canInstaDropDuringInit, leaveSlag, canRoofPunch);
			DropPodUtility.tempList.Clear();
		}

		public static void DropThingGroupsNear(IntVec3 dropCenter, Map map, List<List<Thing>> thingsGroups, int openDelay = 110, bool instaDrop = false, bool leaveSlag = false, bool canRoofPunch = true)
		{
			List<List<Thing>>.Enumerator enumerator = thingsGroups.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					List<Thing> current = enumerator.Current;
					IntVec3 intVec = default(IntVec3);
					if (!DropCellFinder.TryFindDropSpotNear(dropCenter, map, out intVec, true, canRoofPunch))
					{
						Log.Warning("DropThingsNear failed to find a place to drop " + current.FirstOrDefault() + " near " + dropCenter + ". Dropping on random square instead.");
						intVec = CellFinderLoose.RandomCellWith((Predicate<IntVec3>)((IntVec3 c) => c.Walkable(map)), map, 1000);
					}
					for (int i = 0; i < current.Count; i++)
					{
						current[i].SetForbidden(true, false);
					}
					if (instaDrop)
					{
						List<Thing>.Enumerator enumerator2 = current.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								Thing current2 = enumerator2.Current;
								GenPlace.TryPlaceThing(current2, intVec, map, ThingPlaceMode.Near, null);
							}
						}
						finally
						{
							((IDisposable)(object)enumerator2).Dispose();
						}
					}
					else
					{
						ActiveDropPodInfo activeDropPodInfo = new ActiveDropPodInfo();
						List<Thing>.Enumerator enumerator3 = current.GetEnumerator();
						try
						{
							while (enumerator3.MoveNext())
							{
								Thing current3 = enumerator3.Current;
								activeDropPodInfo.innerContainer.TryAdd(current3, true);
							}
						}
						finally
						{
							((IDisposable)(object)enumerator3).Dispose();
						}
						activeDropPodInfo.openDelay = openDelay;
						activeDropPodInfo.leaveSlag = leaveSlag;
						DropPodUtility.MakeDropPodAt(intVec, map, activeDropPodInfo);
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
		}
	}
}
