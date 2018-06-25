using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_FloorFill : SymbolResolver
	{
		public SymbolResolver_FloorFill()
		{
		}

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
