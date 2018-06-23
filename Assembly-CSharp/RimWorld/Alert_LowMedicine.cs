using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000796 RID: 1942
	public class Alert_LowMedicine : Alert
	{
		// Token: 0x0400172A RID: 5930
		private const float MedicinePerColonistThreshold = 2f;

		// Token: 0x06002B0C RID: 11020 RVA: 0x0016BDFF File Offset: 0x0016A1FF
		public Alert_LowMedicine()
		{
			this.defaultLabel = "LowMedicine".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x0016BE20 File Offset: 0x0016A220
		public override string GetExplanation()
		{
			Map map = this.MapWithLowMedicine();
			string result;
			if (map == null)
			{
				result = "";
			}
			else
			{
				int num = this.MedicineCount(map);
				if (num == 0)
				{
					result = string.Format("NoMedicineDesc".Translate(), new object[0]);
				}
				else
				{
					result = string.Format("LowMedicineDesc".Translate(), num);
				}
			}
			return result;
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x0016BE8C File Offset: 0x0016A28C
		public override AlertReport GetReport()
		{
			AlertReport result;
			if (Find.TickManager.TicksGame < 150000)
			{
				result = false;
			}
			else
			{
				result = (this.MapWithLowMedicine() != null);
			}
			return result;
		}

		// Token: 0x06002B0F RID: 11023 RVA: 0x0016BED4 File Offset: 0x0016A2D4
		private Map MapWithLowMedicine()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome)
				{
					if (map.mapPawns.AnyColonistSpawned)
					{
						if ((float)this.MedicineCount(map) < 2f * (float)map.mapPawns.FreeColonistsSpawnedCount)
						{
							return map;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x0016BF5C File Offset: 0x0016A35C
		private int MedicineCount(Map map)
		{
			return map.resourceCounter.GetCountIn(ThingRequestGroup.Medicine);
		}
	}
}
