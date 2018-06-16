using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079A RID: 1946
	public class Alert_LowMedicine : Alert
	{
		// Token: 0x06002B11 RID: 11025 RVA: 0x0016BB93 File Offset: 0x00169F93
		public Alert_LowMedicine()
		{
			this.defaultLabel = "LowMedicine".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x0016BBB4 File Offset: 0x00169FB4
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

		// Token: 0x06002B13 RID: 11027 RVA: 0x0016BC20 File Offset: 0x0016A020
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

		// Token: 0x06002B14 RID: 11028 RVA: 0x0016BC68 File Offset: 0x0016A068
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

		// Token: 0x06002B15 RID: 11029 RVA: 0x0016BCF0 File Offset: 0x0016A0F0
		private int MedicineCount(Map map)
		{
			return map.resourceCounter.GetCountIn(ThingRequestGroup.Medicine);
		}

		// Token: 0x0400172C RID: 5932
		private const float MedicinePerColonistThreshold = 2f;
	}
}
