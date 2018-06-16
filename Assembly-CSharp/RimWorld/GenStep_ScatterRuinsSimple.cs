using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020003FF RID: 1023
	public class GenStep_ScatterRuinsSimple : GenStep_Scatterer
	{
		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06001195 RID: 4501 RVA: 0x00098510 File Offset: 0x00096910
		public override int SeedPart
		{
			get
			{
				return 1348417666;
			}
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x0009852C File Offset: 0x0009692C
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			return base.CanScatterAt(c, map) && c.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy);
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x00098570 File Offset: 0x00096970
		protected bool CanPlaceAncientBuildingInRange(CellRect rect, Map map)
		{
			foreach (IntVec3 c in rect.Cells)
			{
				if (c.InBounds(map))
				{
					TerrainDef terrainDef = map.terrainGrid.TerrainAt(c);
					if (terrainDef.HasTag("River") || terrainDef.HasTag("Road"))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x00098614 File Offset: 0x00096A14
		protected override void ScatterAt(IntVec3 c, Map map, int stackCount = 1)
		{
			ThingDef stuffDef = BaseGenUtility.RandomCheapWallStuff(null, true);
			if (Rand.Bool)
			{
				bool @bool = Rand.Bool;
				int randomInRange = this.WallLengthRange.RandomInRange;
				CellRect cellRect = new CellRect(c.x, c.z, (!@bool) ? 1 : randomInRange, (!@bool) ? randomInRange : 1);
				if (this.CanPlaceAncientBuildingInRange(cellRect.ExpandedBy(1), map))
				{
					this.MakeLongWall(c, map, this.WallLengthRange.RandomInRange, @bool, stuffDef);
				}
			}
			else
			{
				CellRect cellRect2 = new CellRect(c.x, c.z, this.ShedSizeRange.RandomInRange, this.ShedSizeRange.RandomInRange);
				CellRect rect = cellRect2.ClipInsideMap(map);
				if (this.CanPlaceAncientBuildingInRange(rect, map))
				{
					BaseGen.globalSettings.map = map;
					BaseGen.symbolStack.Push("ancientRuins", rect);
					BaseGen.Generate();
				}
			}
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00098710 File Offset: 0x00096B10
		private void TrySetCellAsWall(IntVec3 c, Map map, ThingDef stuffDef)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (!thingList[i].def.destroyable)
				{
					return;
				}
			}
			for (int j = thingList.Count - 1; j >= 0; j--)
			{
				thingList[j].Destroy(DestroyMode.Vanish);
			}
			map.terrainGrid.SetTerrain(c, BaseGenUtility.CorrespondingTerrainDef(stuffDef, true));
			Thing newThing = ThingMaker.MakeThing(ThingDefOf.Wall, stuffDef);
			GenSpawn.Spawn(newThing, c, map, WipeMode.Vanish);
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x000987AC File Offset: 0x00096BAC
		private void MakeLongWall(IntVec3 start, Map map, int extendDist, bool horizontal, ThingDef stuffDef)
		{
			TerrainDef newTerr = BaseGenUtility.CorrespondingTerrainDef(stuffDef, true);
			IntVec3 intVec = start;
			for (int i = 0; i < extendDist; i++)
			{
				if (!intVec.InBounds(map))
				{
					break;
				}
				this.TrySetCellAsWall(intVec, map, stuffDef);
				if (Rand.Chance(0.4f))
				{
					for (int j = 0; j < 9; j++)
					{
						IntVec3 c = intVec + GenAdj.AdjacentCellsAndInside[j];
						if (c.InBounds(map))
						{
							if (Rand.Bool)
							{
								map.terrainGrid.SetTerrain(c, newTerr);
							}
						}
					}
				}
				if (horizontal)
				{
					intVec.x++;
				}
				else
				{
					intVec.z++;
				}
			}
		}

		// Token: 0x04000AAD RID: 2733
		public IntRange ShedSizeRange = new IntRange(3, 10);

		// Token: 0x04000AAE RID: 2734
		public IntRange WallLengthRange = new IntRange(4, 14);
	}
}
