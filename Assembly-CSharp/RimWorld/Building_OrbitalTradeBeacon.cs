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
		// (get) Token: 0x060024A4 RID: 9380 RVA: 0x00139664 File Offset: 0x00137A64
		public IEnumerable<IntVec3> TradeableCells
		{
			get
			{
				return Building_OrbitalTradeBeacon.TradeableCellsAround(base.Position, base.Map);
			}
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x0013968C File Offset: 0x00137A8C
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

		// Token: 0x060024A6 RID: 9382 RVA: 0x001396B8 File Offset: 0x00137AB8
		private void MakeMatchingStockpile()
		{
			Designator des = DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Resources>();
			des.DesignateMultiCell(from c in this.TradeableCells
			where des.CanDesignateCell(c).Accepted
			select c);
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x001396FC File Offset: 0x00137AFC
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

		// Token: 0x060024A8 RID: 9384 RVA: 0x0013979C File Offset: 0x00137B9C
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
