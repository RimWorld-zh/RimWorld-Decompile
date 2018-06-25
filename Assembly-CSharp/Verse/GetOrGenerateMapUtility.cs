using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000C12 RID: 3090
	public static class GetOrGenerateMapUtility
	{
		// Token: 0x06004396 RID: 17302 RVA: 0x0023B8B0 File Offset: 0x00239CB0
		public static Map GetOrGenerateMap(int tile, IntVec3 size, WorldObjectDef suggestedMapParentDef)
		{
			Map map = Current.Game.FindMap(tile);
			if (map == null)
			{
				MapParent mapParent = Find.WorldObjects.MapParentAt(tile);
				if (mapParent == null)
				{
					if (suggestedMapParentDef == null)
					{
						Log.Error("Tried to get or generate map at " + tile + ", but there isn't any MapParent world object here and map parent def argument is null.", false);
						return null;
					}
					mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(suggestedMapParentDef);
					mapParent.Tile = tile;
					Find.WorldObjects.Add(mapParent);
				}
				map = MapGenerator.GenerateMap(size, mapParent, mapParent.MapGeneratorDef, mapParent.ExtraGenStepDefs, null);
			}
			return map;
		}

		// Token: 0x06004397 RID: 17303 RVA: 0x0023B954 File Offset: 0x00239D54
		public static Map GetOrGenerateMap(int tile, WorldObjectDef suggestedMapParentDef)
		{
			return GetOrGenerateMapUtility.GetOrGenerateMap(tile, Find.World.info.initialMapSize, suggestedMapParentDef);
		}
	}
}
