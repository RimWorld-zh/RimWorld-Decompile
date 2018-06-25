using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public static class DeepDrillUtility
	{
		[CompilerGenerated]
		private static Func<ThingDef, ThingDef> <>f__am$cache0;

		public static ThingDef GetNextResource(IntVec3 p, Map map)
		{
			ThingDef result;
			int num;
			IntVec3 intVec;
			DeepDrillUtility.GetNextResource(p, map, out result, out num, out intVec);
			return result;
		}

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

		[CompilerGenerated]
		private static ThingDef <GetBaseResource>m__0(ThingDef rock)
		{
			return rock.building.mineableThing;
		}
	}
}
