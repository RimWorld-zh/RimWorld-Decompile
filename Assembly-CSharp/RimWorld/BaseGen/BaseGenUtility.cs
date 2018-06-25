using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.BaseGen
{
	public static class BaseGenUtility
	{
		[CompilerGenerated]
		private static Func<TerrainDef, bool> <>f__am$cache0;

		public static ThingDef RandomCheapWallStuff(Faction faction, bool notVeryFlammable = false)
		{
			TechLevel techLevel = (faction != null) ? faction.def.techLevel : TechLevel.Spacer;
			return BaseGenUtility.RandomCheapWallStuff(techLevel, notVeryFlammable);
		}

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

		public static bool IsCheapWallStuff(ThingDef d)
		{
			return d.IsStuff && d.stuffProps.CanMake(ThingDefOf.Wall) && d.BaseMarketValue / d.VolumePerUnit < 5f;
		}

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

		public static TerrainDef RandomHightechFloorDef()
		{
			return Rand.Element<TerrainDef>(TerrainDefOf.Concrete, TerrainDefOf.Concrete, TerrainDefOf.PavedTile, TerrainDefOf.PavedTile, TerrainDefOf.MetalTile);
		}

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

		public static TerrainDef RegionalRockTerrainDef(int tile, bool beautiful)
		{
			ThingDef thingDef = Find.World.NaturalRockTypesIn(tile).RandomElementWithFallback(null);
			ThingDef thingDef2 = (thingDef == null) ? null : thingDef.building.mineableThing;
			ThingDef stuffDef = (thingDef2 == null || thingDef2.butcherProducts == null || thingDef2.butcherProducts.Count <= 0) ? null : thingDef2.butcherProducts[0].thingDef;
			return BaseGenUtility.CorrespondingTerrainDef(stuffDef, beautiful);
		}

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

		[CompilerGenerated]
		private static bool <RandomBasicFloorDef>m__0(TerrainDef x)
		{
			return x.IsCarpet;
		}

		[CompilerGenerated]
		private sealed class <RandomCheapWallStuff>c__AnonStorey0
		{
			internal bool notVeryFlammable;

			public <RandomCheapWallStuff>c__AnonStorey0()
			{
			}

			internal bool <>m__0(ThingDef d)
			{
				return BaseGenUtility.IsCheapWallStuff(d) && (!this.notVeryFlammable || d.BaseFlammability < 0.5f);
			}
		}
	}
}
