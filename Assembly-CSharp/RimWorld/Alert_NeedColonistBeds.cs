using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079C RID: 1948
	public class Alert_NeedColonistBeds : Alert
	{
		// Token: 0x06002B24 RID: 11044 RVA: 0x0016C4A4 File Offset: 0x0016A8A4
		public Alert_NeedColonistBeds()
		{
			this.defaultLabel = "NeedColonistBeds".Translate();
			this.defaultExplanation = "NeedColonistBedsDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x0016C4D4 File Offset: 0x0016A8D4
		public override AlertReport GetReport()
		{
			AlertReport result;
			if (GenDate.DaysPassed > 30)
			{
				result = false;
			}
			else
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (this.NeedColonistBeds(maps[i]))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06002B26 RID: 11046 RVA: 0x0016C544 File Offset: 0x0016A944
		private bool NeedColonistBeds(Map map)
		{
			bool result;
			if (!map.IsPlayerHome)
			{
				result = false;
			}
			else
			{
				int num = 0;
				int num2 = 0;
				List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
				for (int i = 0; i < allBuildingsColonist.Count; i++)
				{
					Building_Bed building_Bed = allBuildingsColonist[i] as Building_Bed;
					if (building_Bed != null)
					{
						if (!building_Bed.ForPrisoners && !building_Bed.Medical && building_Bed.def.building.bed_humanlike)
						{
							if (building_Bed.SleepingSlotsCount == 1)
							{
								num++;
							}
							else
							{
								num2++;
							}
						}
					}
				}
				int num3 = 0;
				int num4 = 0;
				foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
				{
					Pawn pawn2 = LovePartnerRelationUtility.ExistingMostLikedLovePartner(pawn, false);
					if (pawn2 == null || !pawn2.Spawned || pawn2.Map != pawn.Map || pawn2.Faction != Faction.OfPlayer || pawn2.HostFaction != null)
					{
						num3++;
					}
					else
					{
						num4++;
					}
				}
				if (num4 % 2 != 0)
				{
					Log.ErrorOnce("partneredCols % 2 != 0", 743211, false);
				}
				for (int j = 0; j < num4 / 2; j++)
				{
					if (num2 > 0)
					{
						num2--;
					}
					else
					{
						num -= 2;
					}
				}
				for (int k = 0; k < num3; k++)
				{
					if (num2 > 0)
					{
						num2--;
					}
					else
					{
						num--;
					}
				}
				result = (num < 0 || num2 < 0);
			}
			return result;
		}
	}
}
