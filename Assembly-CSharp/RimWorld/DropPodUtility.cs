using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public static class DropPodUtility
	{
		private static List<List<Thing>> tempList = new List<List<Thing>>();

		public static void MakeDropPodAt(IntVec3 c, Map map, ActiveDropPodInfo info, bool explode = false)
		{
			ActiveDropPod activeDropPod = (ActiveDropPod)ThingMaker.MakeThing(ThingDefOf.ActiveDropPod, null);
			activeDropPod.Contents = info;
			ThingDef skyfaller = (!explode) ? ThingDefOf.DropPodIncoming : ThingDefOf.ExplosiveDropPodIncoming;
			SkyfallerMaker.SpawnSkyfaller(skyfaller, activeDropPod, c, map);
		}

		public static void DropThingsNear(IntVec3 dropCenter, Map map, IEnumerable<Thing> things, int openDelay = 110, bool canInstaDropDuringInit = false, bool leaveSlag = false, bool canRoofPunch = true, bool explode = false)
		{
			DropPodUtility.tempList.Clear();
			foreach (Thing item in things)
			{
				List<Thing> list = new List<Thing>();
				list.Add(item);
				DropPodUtility.tempList.Add(list);
			}
			DropPodUtility.DropThingGroupsNear(dropCenter, map, DropPodUtility.tempList, openDelay, canInstaDropDuringInit, leaveSlag, canRoofPunch, explode);
			DropPodUtility.tempList.Clear();
		}

		public static void DropThingGroupsNear(IntVec3 dropCenter, Map map, List<List<Thing>> thingsGroups, int openDelay = 110, bool instaDrop = false, bool leaveSlag = false, bool canRoofPunch = true, bool explode = false)
		{
			foreach (List<Thing> list in thingsGroups)
			{
				IntVec3 intVec;
				if (!DropCellFinder.TryFindDropSpotNear(dropCenter, map, out intVec, true, canRoofPunch, explode))
				{
					Log.Warning(string.Concat(new object[]
					{
						"DropThingsNear failed to find a place to drop ",
						list.FirstOrDefault<Thing>(),
						" near ",
						dropCenter,
						". Dropping on random square instead."
					}), false);
					intVec = CellFinderLoose.RandomCellWith((IntVec3 c) => c.Walkable(map), map, 1000);
				}
				for (int i = 0; i < list.Count; i++)
				{
					list[i].SetForbidden(true, false);
				}
				if (instaDrop)
				{
					foreach (Thing thing in list)
					{
						GenPlace.TryPlaceThing(thing, intVec, map, ThingPlaceMode.Near, null, null);
					}
				}
				else
				{
					ActiveDropPodInfo activeDropPodInfo = new ActiveDropPodInfo();
					foreach (Thing item in list)
					{
						activeDropPodInfo.innerContainer.TryAdd(item, true);
					}
					activeDropPodInfo.openDelay = openDelay;
					activeDropPodInfo.leaveSlag = leaveSlag;
					DropPodUtility.MakeDropPodAt(intVec, map, activeDropPodInfo, explode);
				}
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static DropPodUtility()
		{
		}

		[CompilerGenerated]
		private sealed class <DropThingGroupsNear>c__AnonStorey0
		{
			internal Map map;

			public <DropThingGroupsNear>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 c)
			{
				return c.Walkable(this.map);
			}
		}
	}
}
