using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000C13 RID: 3091
	public static class GetOrGenerateMapUtility
	{
		// Token: 0x0600438A RID: 17290 RVA: 0x0023A40C File Offset: 0x0023880C
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

		// Token: 0x0600438B RID: 17291 RVA: 0x0023A4B0 File Offset: 0x002388B0
		public static Map GetOrGenerateMap(int tile, WorldObjectDef suggestedMapParentDef)
		{
			return GetOrGenerateMapUtility.GetOrGenerateMap(tile, Find.World.info.initialMapSize, suggestedMapParentDef);
		}
	}
}
