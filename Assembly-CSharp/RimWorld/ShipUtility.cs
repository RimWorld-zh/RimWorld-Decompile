using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200074F RID: 1871
	public static class ShipUtility
	{
		// Token: 0x04001694 RID: 5780
		private static Dictionary<ThingDef, int> requiredParts;

		// Token: 0x04001695 RID: 5781
		private static List<Building> closedSet = new List<Building>();

		// Token: 0x04001696 RID: 5782
		private static List<Building> openSet = new List<Building>();

		// Token: 0x0600297C RID: 10620 RVA: 0x001609F8 File Offset: 0x0015EDF8
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

		// Token: 0x0600297D RID: 10621 RVA: 0x00160A88 File Offset: 0x0015EE88
		public static IEnumerable<string> LaunchFailReasons(Building rootBuilding)
		{
			List<Building> shipParts = ShipUtility.ShipBuildingsAttachedTo(rootBuilding).ToList<Building>();
			using (Dictionary<ThingDef, int>.Enumerator enumerator = ShipUtility.RequiredParts().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ThingDef, int> partDef = enumerator.Current;
					int shipPartCount = shipParts.Count((Building pa) => pa.def == partDef.Key);
					if (shipPartCount < partDef.Value)
					{
						yield return string.Format("{0}: {1}x {2} ({3} {4})", new object[]
						{
							"ShipReportMissingPart".Translate(),
							partDef.Value - shipPartCount,
							partDef.Key.label,
							"ShipReportMissingPartRequires".Translate(),
							partDef.Value
						});
					}
				}
			}
			bool fullPodFound = false;
			foreach (Building building in shipParts)
			{
				if (building.def == ThingDefOf.Ship_CryptosleepCasket)
				{
					Building_CryptosleepCasket building_CryptosleepCasket = building as Building_CryptosleepCasket;
					if (building_CryptosleepCasket != null && building_CryptosleepCasket.HasAnyContents)
					{
						fullPodFound = true;
						break;
					}
				}
			}
			foreach (Building part in shipParts)
			{
				CompHibernatable hibernatable = part.TryGetComp<CompHibernatable>();
				if (hibernatable != null && hibernatable.State == HibernatableStateDefOf.Hibernating)
				{
					yield return string.Format("{0}: {1}", "ShipReportHibernating".Translate(), part.LabelCap);
				}
				else if (hibernatable != null && !hibernatable.Running)
				{
					yield return string.Format("{0}: {1}", "ShipReportNotReady".Translate(), part.LabelCap);
				}
			}
			if (!fullPodFound)
			{
				yield return "ShipReportNoFullPods".Translate();
			}
			yield break;
		}

		// Token: 0x0600297E RID: 10622 RVA: 0x00160AB4 File Offset: 0x0015EEB4
		public static bool HasHibernatingParts(Building rootBuilding)
		{
			List<Building> list = ShipUtility.ShipBuildingsAttachedTo(rootBuilding).ToList<Building>();
			foreach (Building thing in list)
			{
				CompHibernatable compHibernatable = thing.TryGetComp<CompHibernatable>();
				if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Hibernating)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600297F RID: 10623 RVA: 0x00160B44 File Offset: 0x0015EF44
		public static void StartupHibernatingParts(Building rootBuilding)
		{
			List<Building> list = ShipUtility.ShipBuildingsAttachedTo(rootBuilding).ToList<Building>();
			foreach (Building thing in list)
			{
				CompHibernatable compHibernatable = thing.TryGetComp<CompHibernatable>();
				if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Hibernating)
				{
					compHibernatable.Startup();
				}
			}
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x00160BC8 File Offset: 0x0015EFC8
		public static List<Building> ShipBuildingsAttachedTo(Building root)
		{
			ShipUtility.closedSet.Clear();
			List<Building> result;
			if (root == null || root.Destroyed)
			{
				result = ShipUtility.closedSet;
			}
			else
			{
				ShipUtility.openSet.Clear();
				ShipUtility.openSet.Add(root);
				while (ShipUtility.openSet.Count > 0)
				{
					Building building = ShipUtility.openSet[ShipUtility.openSet.Count - 1];
					ShipUtility.openSet.Remove(building);
					ShipUtility.closedSet.Add(building);
					foreach (IntVec3 c in GenAdj.CellsAdjacentCardinal(building))
					{
						Building edifice = c.GetEdifice(building.Map);
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

		// Token: 0x06002981 RID: 10625 RVA: 0x00160D08 File Offset: 0x0015F108
		public static IEnumerable<Gizmo> ShipStartupGizmos(Building building)
		{
			if (ShipUtility.HasHibernatingParts(building))
			{
				yield return new Command_Action
				{
					action = delegate()
					{
						string text = "HibernateWarning";
						if (building.Map.info.parent.GetComponent<EscapeShipComp>() == null)
						{
							text += "Standalone";
						}
						if (!Find.Storyteller.difficulty.allowBigThreats)
						{
							text += "Pacifist";
						}
						DiaNode diaNode = new DiaNode(text.Translate());
						DiaOption diaOption = new DiaOption("Confirm".Translate());
						diaOption.action = delegate()
						{
							ShipUtility.StartupHibernatingParts(building);
						};
						diaOption.resolveTree = true;
						diaNode.options.Add(diaOption);
						DiaOption diaOption2 = new DiaOption("GoBack".Translate());
						diaOption2.resolveTree = true;
						diaNode.options.Add(diaOption2);
						Find.WindowStack.Add(new Dialog_NodeTree(diaNode, true, false, null));
					},
					defaultLabel = "CommandShipStartup".Translate(),
					defaultDesc = "CommandShipStartupDesc".Translate(),
					hotKey = KeyBindingDefOf.Misc1,
					icon = ContentFinder<Texture2D>.Get("UI/Commands/DesirePower", true)
				};
			}
			yield break;
		}
	}
}
