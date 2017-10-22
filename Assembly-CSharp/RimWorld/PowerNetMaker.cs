using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class PowerNetMaker
	{
		private static HashSet<Building> closedSet = new HashSet<Building>();

		private static HashSet<Building> openSet = new HashSet<Building>();

		private static HashSet<Building> currentSet = new HashSet<Building>();

		private static IEnumerable<CompPower> ContiguousPowerBuildings(Building root)
		{
			PowerNetMaker.closedSet.Clear();
			PowerNetMaker.currentSet.Clear();
			PowerNetMaker.openSet.Add(root);
			while (true)
			{
				foreach (Building item in PowerNetMaker.openSet)
				{
					PowerNetMaker.closedSet.Add(item);
				}
				HashSet<Building> hashSet = PowerNetMaker.currentSet;
				PowerNetMaker.currentSet = PowerNetMaker.openSet;
				PowerNetMaker.openSet = hashSet;
				PowerNetMaker.openSet.Clear();
				foreach (Building item2 in PowerNetMaker.currentSet)
				{
					foreach (IntVec3 item3 in GenAdj.CellsAdjacentCardinal(item2))
					{
						if (item3.InBounds(item2.Map))
						{
							List<Thing> thingList = item3.GetThingList(item2.Map);
							for (int i = 0; i < thingList.Count; i++)
							{
								Building building = thingList[i] as Building;
								if (building != null && building.TransmitsPowerNow && !PowerNetMaker.openSet.Contains(building) && !PowerNetMaker.currentSet.Contains(building) && !PowerNetMaker.closedSet.Contains(building))
								{
									PowerNetMaker.openSet.Add(building);
									break;
								}
							}
						}
					}
				}
				if (PowerNetMaker.openSet.Count <= 0)
					break;
			}
			return from b in PowerNetMaker.closedSet
			select b.PowerComp;
		}

		public static PowerNet NewPowerNetStartingFrom(Building root)
		{
			return new PowerNet(PowerNetMaker.ContiguousPowerBuildings(root));
		}

		public static void UpdateVisualLinkagesFor(PowerNet net)
		{
		}
	}
}
