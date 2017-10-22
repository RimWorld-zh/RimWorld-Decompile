using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public static class ShipChunkDropCellFinder
	{
		public static bool TryFindShipChunkDropCell(IntVec3 nearLoc, Map map, int maxDist, out IntVec3 pos)
		{
			ThingDef chunkDef = ThingDefOf.ShipChunk;
			return CellFinder.TryFindRandomCellNear(nearLoc, map, maxDist, (Predicate<IntVec3>)delegate(IntVec3 x)
			{
				foreach (IntVec3 item in GenAdj.OccupiedRect(x, Rot4.North, chunkDef.size))
				{
					if (item.InBounds(map) && !item.Fogged(map) && item.Standable(map) && (!item.Roofed(map) || !item.GetRoof(map).isThickRoof))
					{
						if (!item.SupportsStructureType(map, chunkDef.terrainAffordanceNeeded))
						{
							return false;
						}
						List<Thing> thingList = item.GetThingList(map);
						for (int i = 0; i < thingList.Count; i++)
						{
							Thing thing = thingList[i];
							if (thing.def.category != ThingCategory.Plant && thing.def.category != ThingCategory.Filth && GenSpawn.SpawningWipes(chunkDef, thing.def))
							{
								return false;
							}
						}
						continue;
					}
					return false;
				}
				return true;
			}, out pos);
		}
	}
}
