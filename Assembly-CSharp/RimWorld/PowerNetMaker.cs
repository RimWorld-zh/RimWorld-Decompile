using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000427 RID: 1063
	public static class PowerNetMaker
	{
		// Token: 0x0600128F RID: 4751 RVA: 0x000A1134 File Offset: 0x0009F534
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

		// Token: 0x06001290 RID: 4752 RVA: 0x000A13B8 File Offset: 0x0009F7B8
		public static PowerNet NewPowerNetStartingFrom(Building root)
		{
			return new PowerNet(PowerNetMaker.ContiguousPowerBuildings(root));
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x000A13D8 File Offset: 0x0009F7D8
		public static void UpdateVisualLinkagesFor(PowerNet net)
		{
		}

		// Token: 0x04000B54 RID: 2900
		private static HashSet<Building> closedSet = new HashSet<Building>();

		// Token: 0x04000B55 RID: 2901
		private static HashSet<Building> openSet = new HashSet<Building>();

		// Token: 0x04000B56 RID: 2902
		private static HashSet<Building> currentSet = new HashSet<Building>();
	}
}
