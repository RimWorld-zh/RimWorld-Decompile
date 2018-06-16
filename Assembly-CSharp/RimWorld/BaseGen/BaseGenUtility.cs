using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200038F RID: 911
	public static class BaseGenUtility
	{
		// Token: 0x06000FE5 RID: 4069 RVA: 0x000855F8 File Offset: 0x000839F8
		public static ThingDef RandomCheapWallStuff(Faction faction, bool notVeryFlammable = false)
		{
			TechLevel techLevel = (faction != null) ? faction.def.techLevel : TechLevel.Spacer;
			return BaseGenUtility.RandomCheapWallStuff(techLevel, notVeryFlammable);
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0008562C File Offset: 0x00083A2C
		public static ThingDef RandomCheapWallStuff(TechLevel techLevel, bool notVeryFlammable = false)
		{
			ThingDef result;
			if (techLevel.IsNeolithicOrWorse())
			{
				result = ThingDefOf.WoodLog;
			}
			else
			{
				result = (from d in DefDatabase<ThingDef>.AllDefsListForReading
				where BaseGenUtility.IsCheapWallStuff(d) && (!notVeryFlammable || d.BaseFlammability < 0.5f)
				select d).RandomElement<ThingDef>();
			}
			return result;
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x00085680 File Offset: 0x00083A80
		public static bool IsCheapWallStuff(ThingDef d)
		{
			return d.IsStuff && d.stuffProps.CanMake(ThingDefOf.Wall) && d.BaseMarketValue / d.VolumePerUnit < 5f;
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x000856CC File Offset: 0x00083ACC
		public static ThingDef RandomHightechWallStuff()
		{
			ThingDef result;
			if (Rand.Value < 0.15f)
			{
				result = ThingDefOf.Plasteel;
			}
			else
			{
				result = ThingDefOf.Steel;
			}
			return result;
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x00085700 File Offset: 0x00083B00
		public static TerrainDef RandomHightechFloorDef()
		{
			return Rand.Element<TerrainDef>(TerrainDefOf.Concrete, TerrainDefOf.Concrete, TerrainDefOf.PavedTile, TerrainDefOf.PavedTile, TerrainDefOf.MetalTile);
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x00085734 File Offset: 0x00083B34
		public static TerrainDef RandomBasicFloorDef(Faction faction, bool allowCarpet = false)
		{
			TerrainDef result;
			if (allowCarpet && (faction == null || !faction.def.techLevel.IsNeolithicOrWorse()) && Rand.Chance(0.1f))
			{
				result = (from x in DefDatabase<TerrainDef>.AllDefsListForReading
				where x.IsCarpet
				select x).RandomElement<TerrainDef>();
			}
			else
			{
				result = Rand.Element<TerrainDef>(TerrainDefOf.MetalTile, TerrainDefOf.PavedTile, TerrainDefOf.WoodPlankFloor, TerrainDefOf.TileSandstone);
			}
			return result;
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x000857C8 File Offset: 0x00083BC8
		public static TerrainDef CorrespondingTerrainDef(ThingDef stuffDef, bool beautiful)
		{
			TerrainDef terrainDef = null;
			List<TerrainDef> allDefsListForReading = DefDatabase<TerrainDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].costList != null)
				{
					for (int j = 0; j < allDefsListForReading[i].costList.Count; j++)
					{
						if (allDefsListForReading[i].costList[j].thingDef == stuffDef)
						{
							if (terrainDef == null || ((!beautiful) ? (terrainDef.statBases.GetStatOffsetFromList(StatDefOf.Beauty) > allDefsListForReading[i].statBases.GetStatOffsetFromList(StatDefOf.Beauty)) : (terrainDef.statBases.GetStatOffsetFromList(StatDefOf.Beauty) < allDefsListForReading[i].statBases.GetStatOffsetFromList(StatDefOf.Beauty))))
							{
								terrainDef = allDefsListForReading[i];
							}
						}
					}
				}
			}
			if (terrainDef == null)
			{
				terrainDef = TerrainDefOf.Concrete;
			}
			return terrainDef;
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x000858D4 File Offset: 0x00083CD4
		public static TerrainDef RegionalRockTerrainDef(int tile, bool beautiful)
		{
			ThingDef thingDef = Find.World.NaturalRockTypesIn(tile).RandomElementWithFallback(null);
			ThingDef thingDef2 = (thingDef == null) ? null : thingDef.building.mineableThing;
			ThingDef stuffDef = (thingDef2 == null || thingDef2.butcherProducts == null || thingDef2.butcherProducts.Count <= 0) ? null : thingDef2.butcherProducts[0].thingDef;
			return BaseGenUtility.CorrespondingTerrainDef(stuffDef, beautiful);
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x00085954 File Offset: 0x00083D54
		public static bool AnyDoorAdjacentCardinalTo(IntVec3 cell, Map map)
		{
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = cell + GenAdj.CardinalDirections[i];
				if (c.InBounds(map) && c.GetDoor(map) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x000859B4 File Offset: 0x00083DB4
		public static bool AnyDoorAdjacentCardinalTo(CellRect rect, Map map)
		{
			foreach (IntVec3 c in rect.AdjacentCellsCardinal)
			{
				if (c.InBounds(map))
				{
					if (c.GetDoor(map) != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x00085A38 File Offset: 0x00083E38
		public static ThingDef WallStuffAt(IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			ThingDef result;
			if (edifice != null && edifice.def == ThingDefOf.Wall)
			{
				result = edifice.Stuff;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
