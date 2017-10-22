using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class ShipUtility
	{
		private static List<Building> closedSet = new List<Building>();

		private static List<Building> openSet = new List<Building>();

		public static IEnumerable<string> LaunchFailReasons(Building rootBuilding)
		{
			List<Building> shipParts = ShipUtility.ShipBuildingsAttachedTo(rootBuilding).ToList();
			List<ThingDef>.Enumerator enumerator = new List<ThingDef>
			{
				ThingDefOf.Ship_CryptosleepCasket,
				ThingDefOf.Ship_ComputerCore,
				ThingDefOf.Ship_Reactor,
				ThingDefOf.Ship_Engine
			}.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ThingDef partDef = enumerator.Current;
					if (!shipParts.Any((Predicate<Building>)((Building pa) => pa.def == ((_003CLaunchFailReasons_003Ec__Iterator171)/*Error near IL_00c4: stateMachine*/)._003CpartDef_003E__3)))
					{
						yield return "ShipReportMissingPart".Translate() + ": " + partDef.label;
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			bool fullPodFound = false;
			List<Building>.Enumerator enumerator2 = shipParts.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					Building part = enumerator2.Current;
					if (part.def == ThingDefOf.Ship_CryptosleepCasket)
					{
						Building_CryptosleepCasket pod = part as Building_CryptosleepCasket;
						if (pod != null && pod.HasAnyContents)
						{
							fullPodFound = true;
							break;
						}
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator2).Dispose();
			}
			if (!fullPodFound)
			{
				yield return "ShipReportNoFullPods".Translate();
			}
		}

		public static List<Building> ShipBuildingsAttachedTo(Building root)
		{
			if (root != null && !root.Destroyed)
			{
				ShipUtility.closedSet.Clear();
				ShipUtility.openSet.Clear();
				ShipUtility.openSet.Add(root);
				while (ShipUtility.openSet.Count > 0)
				{
					Building building = ShipUtility.openSet[ShipUtility.openSet.Count - 1];
					ShipUtility.openSet.Remove(building);
					ShipUtility.closedSet.Add(building);
					foreach (IntVec3 item in GenAdj.CellsAdjacentCardinal(building))
					{
						Building edifice = item.GetEdifice(building.Map);
						if (edifice != null && edifice.def.building.shipPart && !ShipUtility.closedSet.Contains(edifice) && !ShipUtility.openSet.Contains(edifice))
						{
							ShipUtility.openSet.Add(edifice);
						}
					}
				}
				return ShipUtility.closedSet;
			}
			return new List<Building>();
		}
	}
}
