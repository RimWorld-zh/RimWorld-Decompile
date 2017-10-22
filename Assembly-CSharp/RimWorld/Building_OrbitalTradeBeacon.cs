using System;
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
			using (IEnumerator<Gizmo> enumerator = this._003CGetGizmos_003E__BaseCallProxy0().GetEnumerator())
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
				action = new Action(this.MakeMatchingStockpile),
				hotKey = KeyBindingDefOf.Misc1,
				defaultDesc = "CommandMakeBeaconStockpileDesc".Translate(),
				icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true),
				defaultLabel = "CommandMakeBeaconStockpileLabel".Translate()
			};
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0164:
			/*Error near IL_0165: Unexpected return in MoveNext()*/;
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
			List<IntVec3> result;
			if (!pos.InBounds(map))
			{
				result = Building_OrbitalTradeBeacon.tradeableCells;
			}
			else
			{
				Region region = pos.GetRegion(map, RegionType.Set_Passable);
				if (region == null)
				{
					result = Building_OrbitalTradeBeacon.tradeableCells;
				}
				else
				{
					RegionTraverser.BreadthFirstTraverse(region, (RegionEntryPredicate)((Region from, Region r) => r.portal == null), (RegionProcessor)delegate(Region r)
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
					result = Building_OrbitalTradeBeacon.tradeableCells;
				}
			}
			return result;
		}

		public static IEnumerable<Building_OrbitalTradeBeacon> AllPowered(Map map)
		{
			using (IEnumerator<Building_OrbitalTradeBeacon> enumerator = map.listerBuildings.AllBuildingsColonistOfClass<Building_OrbitalTradeBeacon>().GetEnumerator())
			{
				Building_OrbitalTradeBeacon b;
				while (true)
				{
					if (enumerator.MoveNext())
					{
						b = enumerator.Current;
						CompPowerTrader power = b.GetComp<CompPowerTrader>();
						if (power == null)
							break;
						if (power.PowerOn)
							break;
						continue;
					}
					yield break;
				}
				yield return b;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			IL_00f3:
			/*Error near IL_00f4: Unexpected return in MoveNext()*/;
		}
	}
}
