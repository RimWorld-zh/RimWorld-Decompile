using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200070A RID: 1802
	public static class DeepDrillUtility
	{
		// Token: 0x0600277C RID: 10108 RVA: 0x0015307C File Offset: 0x0015147C
		public static ThingDef GetNextResource(IntVec3 p, Map map)
		{
			ThingDef result;
			int num;
			IntVec3 intVec;
			DeepDrillUtility.GetNextResource(p, map, out result, out num, out intVec);
			return result;
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x001530A0 File Offset: 0x001514A0
		public static bool GetNextResource(IntVec3 p, Map map, out ThingDef resDef, out int countPresent, out IntVec3 cell)
		{
			for (int i = 0; i < 9; i++)
			{
				IntVec3 intVec = p + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map))
				{
					ThingDef thingDef = map.deepResourceGrid.ThingDefAt(intVec);
					if (thingDef != null)
					{
						resDef = thingDef;
						countPresent = map.deepResourceGrid.CountAt(intVec);
						cell = intVec;
						return true;
					}
				}
			}
			resDef = DeepDrillUtility.GetBaseResource(map);
			countPresent = int.MaxValue;
			cell = p;
			return false;
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x00153144 File Offset: 0x00151544
		public static ThingDef GetBaseResource(Map map)
		{
			ThingDef result;
			if (!map.Biome.hasBedrock)
			{
				result = null;
			}
			else
			{
				result = (from rock in Find.World.NaturalRockTypesIn(map.Tile)
				select rock.building.mineableThing).FirstOrDefault<ThingDef>();
			}
			return result;
		}
	}
}
