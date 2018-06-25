using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000798 RID: 1944
	public class Alert_LowMedicine : Alert
	{
		// Token: 0x0400172E RID: 5934
		private const float MedicinePerColonistThreshold = 2f;

		// Token: 0x06002B0F RID: 11023 RVA: 0x0016C1B3 File Offset: 0x0016A5B3
		public Alert_LowMedicine()
		{
			this.defaultLabel = "LowMedicine".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x0016C1D4 File Offset: 0x0016A5D4
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

		// Token: 0x06002B11 RID: 11025 RVA: 0x0016C240 File Offset: 0x0016A640
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

		// Token: 0x06002B12 RID: 11026 RVA: 0x0016C288 File Offset: 0x0016A688
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

		// Token: 0x06002B13 RID: 11027 RVA: 0x0016C310 File Offset: 0x0016A710
		private int MedicineCount(Map map)
		{
			return map.resourceCounter.GetCountIn(ThingRequestGroup.Medicine);
		}
	}
}
