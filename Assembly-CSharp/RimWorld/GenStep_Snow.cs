using Verse;

namespace RimWorld
{
	public class GenStep_Snow : GenStep
	{
		public override void Generate(Map map)
		{
			int num = 0;
			for (int i = (int)(GenLocalDate.Twelfth(map) - 2); i <= (int)GenLocalDate.Twelfth(map); i++)
			{
				int num2 = i;
				if (num2 < 0)
				{
					num2 += 12;
				}
				Twelfth twelfth = (Twelfth)(byte)num2;
				float num3 = GenTemperature.AverageTemperatureAtTileForTwelfth(map.Tile, twelfth);
				if (num3 < 0.0)
				{
					num++;
				}
			}
			float num4 = 0f;
			switch (num)
			{
			case 1:
			{
				num4 = 0.3f;
				break;
			}
			case 2:
			{
				num4 = 0.7f;
				break;
			}
			case 3:
			{
				num4 = 1f;
				break;
			}
			case 0:
				return;
			}
			if (map.mapTemperature.SeasonalTemp > 0.0)
			{
				num4 = (float)(num4 * 0.40000000596046448);
			}
			if (!((double)num4 < 0.3))
			{
				foreach (IntVec3 allCell in map.AllCells)
				{
					if (!allCell.Roofed(map))
					{
						map.steadyAtmosphereEffects.AddFallenSnowAt(allCell, num4);
					}
				}
			}
		}
	}
}
