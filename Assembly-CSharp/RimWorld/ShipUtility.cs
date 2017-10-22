using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class ShipUtility
	{
		private static Dictionary<ThingDef, int> requiredParts;

		private static List<Building> closedSet = new List<Building>();

		private static List<Building> openSet = new List<Building>();

		public static Dictionary<ThingDef, int> RequiredParts()
		{
			if (ShipUtility.requiredParts == null)
			{
				ShipUtility.requiredParts = new Dictionary<ThingDef, int>();
				ShipUtility.requiredParts[ThingDefOf.Ship_CryptosleepCasket] = 1;
				ShipUtility.requiredParts[ThingDefOf.Ship_ComputerCore] = 1;
				ShipUtility.requiredParts[ThingDefOf.Ship_Reactor] = 1;
				ShipUtility.requiredParts[ThingDefOf.Ship_Engine] = 3;
				ShipUtility.requiredParts[ThingDefOf.Ship_Beam] = 1;
				ShipUtility.requiredParts[ThingDefOf.Ship_SensorCluster] = 1;
			}
			return ShipUtility.requiredParts;
		}

		public static IEnumerable<string> LaunchFailReasons(Building rootBuilding)
		{
			List<Building> shipParts = ShipUtility.ShipBuildingsAttachedTo(rootBuilding).ToList();
			foreach (KeyValuePair<ThingDef, int> item in ShipUtility.RequiredParts())
			{
				int shipPartCount = shipParts.Count((Func<Building, bool>)((Building pa) => pa.def == item.Key));
				if (shipPartCount < item.Value)
				{
					yield return string.Format("{0}: {1}x {2} ({3} {4})", "ShipReportMissingPart".Translate(), item.Value - shipPartCount, item.Key.label, "ShipReportMissingPartRequires".Translate(), item.Value);
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			bool fullPodFound = false;
			foreach (Building item2 in shipParts)
			{
				if (item2.def == ThingDefOf.Ship_CryptosleepCasket)
				{
					Building_CryptosleepCasket building_CryptosleepCasket = item2 as Building_CryptosleepCasket;
					if (building_CryptosleepCasket != null && building_CryptosleepCasket.HasAnyContents)
					{
						fullPodFound = true;
						break;
					}
				}
			}
			foreach (Building item3 in shipParts)
			{
				CompHibernatable hibernatable = item3.TryGetComp<CompHibernatable>();
				if (hibernatable != null && hibernatable.State == HibernatableStateDefOf.Hibernating)
				{
					yield return string.Format("{0}: {1}", "ShipReportHibernating".Translate(), item3.Label);
					/*Error: Unable to find new state assignment for yield return*/;
				}
				if (hibernatable != null && !hibernatable.Running)
				{
					yield return string.Format("{0}: {1}", "ShipReportNotReady".Translate(), item3.Label);
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (fullPodFound)
				yield break;
			yield return "ShipReportNoFullPods".Translate();
			/*Error: Unable to find new state assignment for yield return*/;
			IL_037c:
			/*Error near IL_037d: Unexpected return in MoveNext()*/;
		}

		public static bool HasHibernatingParts(Building rootBuilding)
		{
			List<Building> list = ShipUtility.ShipBuildingsAttachedTo(rootBuilding).ToList();
			foreach (Building item in list)
			{
				CompHibernatable compHibernatable = item.TryGetComp<CompHibernatable>();
				if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Hibernating)
				{
					return true;
				}
			}
			return false;
		}

		public static void StartupHibernatingParts(Building rootBuilding)
		{
			List<Building> list = ShipUtility.ShipBuildingsAttachedTo(rootBuilding).ToList();
			foreach (Building item in list)
			{
				CompHibernatable compHibernatable = item.TryGetComp<CompHibernatable>();
				if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Hibernating)
				{
					compHibernatable.Startup();
				}
			}
		}

		public static List<Building> ShipBuildingsAttachedTo(Building root)
		{
			List<Building> result;
			if (root == null || root.Destroyed)
			{
				result = new List<Building>();
			}
			else
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
				result = ShipUtility.closedSet;
			}
			return result;
		}
	}
}
