using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020006E3 RID: 1763
	public static class DropPodUtility
	{
		// Token: 0x06002664 RID: 9828 RVA: 0x00149B70 File Offset: 0x00147F70
		public static void MakeDropPodAt(IntVec3 c, Map map, ActiveDropPodInfo info, bool explode = false)
		{
			ActiveDropPod activeDropPod = (ActiveDropPod)ThingMaker.MakeThing(ThingDefOf.ActiveDropPod, null);
			activeDropPod.Contents = info;
			ThingDef skyfaller = (!explode) ? ThingDefOf.DropPodIncoming : ThingDefOf.ExplosiveDropPodIncoming;
			SkyfallerMaker.SpawnSkyfaller(skyfaller, activeDropPod, c, map);
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x00149BB8 File Offset: 0x00147FB8
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

		// Token: 0x06002666 RID: 9830 RVA: 0x00149C4C File Offset: 0x0014804C
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

		// Token: 0x04001568 RID: 5480
		private static List<List<Thing>> tempList = new List<List<Thing>>();
	}
}
