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
				HashSet<Building>.Enumerator enumerator = PowerNetMaker.openSet.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Building current = enumerator.Current;
						PowerNetMaker.closedSet.Add(current);
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
				HashSet<Building> hashSet = PowerNetMaker.currentSet;
				PowerNetMaker.currentSet = PowerNetMaker.openSet;
				PowerNetMaker.openSet = hashSet;
				PowerNetMaker.openSet.Clear();
				HashSet<Building>.Enumerator enumerator2 = PowerNetMaker.currentSet.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						Building current2 = enumerator2.Current;
						foreach (IntVec3 item in GenAdj.CellsAdjacentCardinal(current2))
						{
							if (item.InBounds(current2.Map))
							{
								List<Thing> thingList = item.GetThingList(current2.Map);
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
				}
				finally
				{
					((IDisposable)(object)enumerator2).Dispose();
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
