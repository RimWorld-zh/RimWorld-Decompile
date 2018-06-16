using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003B2 RID: 946
	public class SymbolResolver_FloorFill : SymbolResolver
	{
		// Token: 0x0600106F RID: 4207 RVA: 0x0008AFD4 File Offset: 0x000893D4
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			TerrainGrid terrainGrid = map.terrainGrid;
			TerrainDef newTerr = rp.floorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false);
			CellRect.CellRectIterator iterator = rp.rect.GetIterator();
			while (!iterator.Done())
			{
				if (rp.chanceToSkipFloor == null || !Rand.Chance(rp.chanceToSkipFloor.Value))
				{
					terrainGrid.SetTerrain(iterator.Current, newTerr);
					if (rp.filthDef != null)
					{
						FilthMaker.MakeFilth(iterator.Current, map, rp.filthDef, (rp.filthDensity == null) ? 1 : Mathf.RoundToInt(rp.filthDensity.Value.RandomInRange));
					}
				}
				iterator.MoveNext();
			}
		}
	}
}
