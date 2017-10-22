using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class RoadDefGenStep_Bulldoze : RoadDefGenStep
	{
		public override void Place(Map map, IntVec3 tile, TerrainDef rockDef)
		{
			while (tile.Impassable(map))
			{
				List<Thing>.Enumerator enumerator = tile.GetThingList(map).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Thing current = enumerator.Current;
						if (current.def.passability == Traversability.Impassable)
						{
							current.Destroy(DestroyMode.Vanish);
							break;
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
			}
		}
	}
}
