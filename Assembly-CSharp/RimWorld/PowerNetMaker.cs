using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000429 RID: 1065
	public static class PowerNetMaker
	{
		// Token: 0x04000B58 RID: 2904
		private static HashSet<Building> closedSet = new HashSet<Building>();

		// Token: 0x04000B59 RID: 2905
		private static HashSet<Building> openSet = new HashSet<Building>();

		// Token: 0x04000B5A RID: 2906
		private static HashSet<Building> currentSet = new HashSet<Building>();

		// Token: 0x06001292 RID: 4754 RVA: 0x000A1668 File Offset: 0x0009FA68
		private static IEnumerable<CompPower> ContiguousPowerBuildings(Building root)
		{
			PowerNetMaker.closedSet.Clear();
			PowerNetMaker.openSet.Clear();
			PowerNetMaker.currentSet.Clear();
			PowerNetMaker.openSet.Add(root);
			do
			{
				foreach (Building item in PowerNetMaker.openSet)
				{
					PowerNetMaker.closedSet.Add(item);
				}
				HashSet<Building> hashSet = PowerNetMaker.currentSet;
				PowerNetMaker.currentSet = PowerNetMaker.openSet;
				PowerNetMaker.openSet = hashSet;
				PowerNetMaker.openSet.Clear();
				foreach (Building building in PowerNetMaker.currentSet)
				{
					foreach (IntVec3 c in GenAdj.CellsAdjacentCardinal(building))
					{
						if (c.InBounds(building.Map))
						{
							List<Thing> thingList = c.GetThingList(building.Map);
							for (int i = 0; i < thingList.Count; i++)
							{
								Building building2 = thingList[i] as Building;
								if (building2 != null)
								{
									if (building2.TransmitsPowerNow)
									{
										if (!PowerNetMaker.openSet.Contains(building2) && !PowerNetMaker.currentSet.Contains(building2) && !PowerNetMaker.closedSet.Contains(building2))
										{
											PowerNetMaker.openSet.Add(building2);
											break;
										}
									}
								}
							}
						}
					}
				}
			}
			while (PowerNetMaker.openSet.Count > 0);
			CompPower[] result = (from b in PowerNetMaker.closedSet
			select b.PowerComp).ToArray<CompPower>();
			PowerNetMaker.closedSet.Clear();
			PowerNetMaker.openSet.Clear();
			PowerNetMaker.currentSet.Clear();
			return result;
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x000A18EC File Offset: 0x0009FCEC
		public static PowerNet NewPowerNetStartingFrom(Building root)
		{
			return new PowerNet(PowerNetMaker.ContiguousPowerBuildings(root));
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x000A190C File Offset: 0x0009FD0C
		public static void UpdateVisualLinkagesFor(PowerNet net)
		{
		}
	}
}
