using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006AE RID: 1710
	public class Building_OrbitalTradeBeacon : Building
	{
		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x060024A2 RID: 9378 RVA: 0x001395EC File Offset: 0x001379EC
		public IEnumerable<IntVec3> TradeableCells
		{
			get
			{
				return Building_OrbitalTradeBeacon.TradeableCellsAround(base.Position, base.Map);
			}
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x00139614 File Offset: 0x00137A14
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			if (DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Resources>() != null)
			{
				yield return new Command_Action
				{
					action = new Action(this.MakeMatchingStockpile),
					hotKey = KeyBindingDefOf.Misc1,
					defaultDesc = "CommandMakeBeaconStockpileDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true),
					defaultLabel = "CommandMakeBeaconStockpileLabel".Translate()
				};
			}
			yield break;
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x00139640 File Offset: 0x00137A40
		private void MakeMatchingStockpile()
		{
			Designator des = DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Resources>();
			des.DesignateMultiCell(from c in this.TradeableCells
			where des.CanDesignateCell(c).Accepted
			select c);
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x00139684 File Offset: 0x00137A84
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
					RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.portal == null, delegate(Region r)
					{
						foreach (IntVec3 item in r.Cells)
						{
							if (item.InHorDistOf(pos, 7.9f))
							{
								Building_OrbitalTradeBeacon.tradeableCells.Add(item);
							}
						}
						return false;
					}, 13, RegionType.Set_Passable);
					result = Building_OrbitalTradeBeacon.tradeableCells;
				}
			}
			return result;
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x00139724 File Offset: 0x00137B24
		public static IEnumerable<Building_OrbitalTradeBeacon> AllPowered(Map map)
		{
			foreach (Building_OrbitalTradeBeacon b in map.listerBuildings.AllBuildingsColonistOfClass<Building_OrbitalTradeBeacon>())
			{
				CompPowerTrader power = b.GetComp<CompPowerTrader>();
				if (power == null || power.PowerOn)
				{
					yield return b;
				}
			}
			yield break;
		}

		// Token: 0x0400143C RID: 5180
		private const float TradeRadius = 7.9f;

		// Token: 0x0400143D RID: 5181
		private static List<IntVec3> tradeableCells = new List<IntVec3>();
	}
}
