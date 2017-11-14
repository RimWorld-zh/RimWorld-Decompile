using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Building_OrbitalTradeBeacon : Building
	{
		private const float TradeRadius = 7.9f;

		private static List<IntVec3> tradeableCells = new List<IntVec3>();

		public IEnumerable<IntVec3> TradeableCells
		{
			get
			{
				return Building_OrbitalTradeBeacon.TradeableCellsAround(base.Position, base.Map);
			}
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			using (IEnumerator<Gizmo> enumerator = base.GetGizmos().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo g = enumerator.Current;
					yield return g;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Resources>() == null)
				yield break;
			yield return (Gizmo)new Command_Action
			{
				action = this.MakeMatchingStockpile,
				hotKey = KeyBindingDefOf.Misc1,
				defaultDesc = "CommandMakeBeaconStockpileDesc".Translate(),
				icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true),
				defaultLabel = "CommandMakeBeaconStockpileLabel".Translate()
			};
			/*Error: Unable to find new state assignment for yield return*/;
			IL_015e:
			/*Error near IL_015f: Unexpected return in MoveNext()*/;
		}

		private void MakeMatchingStockpile()
		{
			Designator des = DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Resources>();
			des.DesignateMultiCell(from c in this.TradeableCells
			where des.CanDesignateCell(c).Accepted
			select c);
		}

		public static List<IntVec3> TradeableCellsAround(IntVec3 pos, Map map)
		{
			Building_OrbitalTradeBeacon.tradeableCells.Clear();
			if (!pos.InBounds(map))
			{
				return Building_OrbitalTradeBeacon.tradeableCells;
			}
			Region region = pos.GetRegion(map, RegionType.Set_Passable);
			if (region == null)
			{
				return Building_OrbitalTradeBeacon.tradeableCells;
			}
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.portal == null, delegate(Region r)
			{
				foreach (IntVec3 cell in r.Cells)
				{
					if (cell.InHorDistOf(pos, 7.9f))
					{
						Building_OrbitalTradeBeacon.tradeableCells.Add(cell);
					}
				}
				return false;
			}, 13, RegionType.Set_Passable);
			return Building_OrbitalTradeBeacon.tradeableCells;
		}

		public static IEnumerable<Building_OrbitalTradeBeacon> AllPowered(Map map)
		{
			foreach (Building_OrbitalTradeBeacon item in map.listerBuildings.AllBuildingsColonistOfClass<Building_OrbitalTradeBeacon>())
			{
				CompPowerTrader power = item.GetComp<CompPowerTrader>();
				if (power != null && !power.PowerOn)
				{
					continue;
				}
				yield return item;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			yield break;
			IL_00ef:
			/*Error near IL_00f0: Unexpected return in MoveNext()*/;
		}
	}
}
