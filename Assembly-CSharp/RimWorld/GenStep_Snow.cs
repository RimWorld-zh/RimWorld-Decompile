using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020003F7 RID: 1015
	public class GenStep_Snow : GenStep
	{
		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06001178 RID: 4472 RVA: 0x0009743C File Offset: 0x0009583C
		public override int SeedPart
		{
			get
			{
				return 306693816;
			}
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x00097458 File Offset: 0x00095858
		public override void Generate(Map map)
		{
			int num = 0;
			for (int i = (int)(GenLocalDate.Twelfth(map) - Twelfth.Third); i <= (int)GenLocalDate.Twelfth(map); i++)
			{
				int num2 = i;
				if (num2 < 0)
				{
					num2 += 12;
				}
				Twelfth twelfth = (Twelfth)num2;
				float num3 = GenTemperature.AverageTemperatureAtTileForTwelfth(map.Tile, twelfth);
				if (num3 < 0f)
				{
					num++;
				}
			}
			float num4 = 0f;
			switch (num)
			{
			case 0:
				return;
			case 1:
				num4 = 0.3f;
				break;
			case 2:
				num4 = 0.7f;
				break;
			case 3:
				num4 = 1f;
				break;
			}
			if (map.mapTemperature.SeasonalTemp > 0f)
			{
				num4 *= 0.4f;
			}
			if ((double)num4 >= 0.3)
			{
				foreach (IntVec3 c in map.AllCells)
				{
					if (!c.Roofed(map))
					{
						map.steadyEnvironmentEffects.AddFallenSnowAt(c, num4);
					}
				}
			}
		}
	}
}
