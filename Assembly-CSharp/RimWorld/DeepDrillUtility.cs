using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200070C RID: 1804
	public static class DeepDrillUtility
	{
		// Token: 0x0600277F RID: 10111 RVA: 0x00152AB0 File Offset: 0x00150EB0
		public static ThingDef GetNextResource(IntVec3 p, Map map)
		{
			ThingDef result;
			int num;
			IntVec3 intVec;
			DeepDrillUtility.GetNextResource(p, map, out result, out num, out intVec);
			return result;
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x00152AD4 File Offset: 0x00150ED4
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

		// Token: 0x06002781 RID: 10113 RVA: 0x00152B78 File Offset: 0x00150F78
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
