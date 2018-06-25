using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public static class PowerNetMaker
	{
		private static HashSet<Building> closedSet = new HashSet<Building>();

		private static HashSet<Building> openSet = new HashSet<Building>();

		private static HashSet<Building> currentSet = new HashSet<Building>();

		[CompilerGenerated]
		private static Func<Building, CompPower> <>f__am$cache0;

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

		public static PowerNet NewPowerNetStartingFrom(Building root)
		{
			return new PowerNet(PowerNetMaker.ContiguousPowerBuildings(root));
		}

		public static void UpdateVisualLinkagesFor(PowerNet net)
		{
		}

		// Note: this type is marked as 'beforefieldinit'.
		static PowerNetMaker()
		{
		}

		[CompilerGenerated]
		private static CompPower <ContiguousPowerBuildings>m__0(Building b)
		{
			return b.PowerComp;
		}
	}
}
