using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class TerrainThreshold
	{
		public TerrainDef terrain;

		public float min = -1000f;

		public float max = 1000f;

		public static TerrainDef TerrainAtValue(List<TerrainThreshold> threshes, float val)
		{
			int num = 0;
			TerrainDef result;
			while (true)
			{
				if (num < threshes.Count)
				{
					if (threshes[num].min <= val && threshes[num].max >= val)
					{
						result = threshes[num].terrain;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}
	}
}
