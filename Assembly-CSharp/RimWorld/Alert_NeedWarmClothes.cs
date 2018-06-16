using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079B RID: 1947
	public class Alert_NeedWarmClothes : Alert
	{
		// Token: 0x06002B16 RID: 11030 RVA: 0x0016BD12 File Offset: 0x0016A112
		public Alert_NeedWarmClothes()
		{
			this.defaultLabel = "NeedWarmClothes".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x0016BD34 File Offset: 0x0016A134
		private int NeededWarmClothesCount(Map map)
		{
			return map.mapPawns.FreeColonistsSpawnedCount;
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x0016BD54 File Offset: 0x0016A154
		private int ColonistsWithWarmClothesCount(Map map)
		{
			float num = this.LowestTemperatureComing(map);
			int num2 = 0;
			foreach (Pawn thing in map.mapPawns.FreeColonistsSpawned)
			{
				if (thing.GetStatValue(StatDefOf.ComfyTemperatureMin, true) <= num)
				{
					num2++;
				}
			}
			return num2;
		}

		// Token: 0x06002B19 RID: 11033 RVA: 0x0016BDDC File Offset: 0x0016A1DC
		private int FreeWarmClothesSetsCount(Map map)
		{
			Alert_NeedWarmClothes.jackets.Clear();
			Alert_NeedWarmClothes.shirts.Clear();
			Alert_NeedWarmClothes.pants.Clear();
			List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.Apparel);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].IsInAnyStorage())
				{
					if (list[i].GetStatValue(StatDefOf.Insulation_Cold, true) > 0f)
					{
						if (list[i].def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))
						{
							if (list[i].def.apparel.layers.Contains(ApparelLayerDefOf.OnSkin))
							{
								Alert_NeedWarmClothes.shirts.Add(list[i]);
							}
							else
							{
								Alert_NeedWarmClothes.jackets.Add(list[i]);
							}
						}
						if (list[i].def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Legs))
						{
							Alert_NeedWarmClothes.pants.Add(list[i]);
						}
					}
				}
			}
			Alert_NeedWarmClothes.jackets.SortBy((Thing x) => x.GetStatValue(StatDefOf.Insulation_Cold, true));
			Alert_NeedWarmClothes.shirts.SortBy((Thing x) => x.GetStatValue(StatDefOf.Insulation_Cold, true));
			Alert_NeedWarmClothes.pants.SortBy((Thing x) => x.GetStatValue(StatDefOf.Insulation_Cold, true));
			float num = ThingDefOf.Human.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) - this.LowestTemperatureComing(map);
			int result;
			if (num <= 0f)
			{
				result = GenMath.Max(Alert_NeedWarmClothes.jackets.Count, Alert_NeedWarmClothes.shirts.Count, Alert_NeedWarmClothes.pants.Count);
			}
			else
			{
				int num2 = 0;
				while (Alert_NeedWarmClothes.jackets.Any<Thing>() || Alert_NeedWarmClothes.shirts.Any<Thing>() || Alert_NeedWarmClothes.pants.Any<Thing>())
				{
					float num3 = 0f;
					if (Alert_NeedWarmClothes.jackets.Any<Thing>())
					{
						Thing thing = Alert_NeedWarmClothes.jackets[Alert_NeedWarmClothes.jackets.Count - 1];
						Alert_NeedWarmClothes.jackets.RemoveLast<Thing>();
						num3 += thing.GetStatValue(StatDefOf.Insulation_Cold, true);
					}
					if (num3 < num && Alert_NeedWarmClothes.shirts.Any<Thing>())
					{
						Thing thing2 = Alert_NeedWarmClothes.shirts[Alert_NeedWarmClothes.shirts.Count - 1];
						Alert_NeedWarmClothes.shirts.RemoveLast<Thing>();
						num3 += thing2.GetStatValue(StatDefOf.Insulation_Cold, true);
					}
					if (num3 < num && Alert_NeedWarmClothes.pants.Any<Thing>())
					{
						for (int j = 0; j < Alert_NeedWarmClothes.pants.Count; j++)
						{
							float statValue = Alert_NeedWarmClothes.pants[j].GetStatValue(StatDefOf.Insulation_Cold, true);
							if (statValue + num3 >= num)
							{
								num3 += statValue;
								Alert_NeedWarmClothes.pants.RemoveAt(j);
								break;
							}
						}
					}
					if (num3 < num)
					{
						break;
					}
					num2++;
				}
				Alert_NeedWarmClothes.jackets.Clear();
				Alert_NeedWarmClothes.shirts.Clear();
				Alert_NeedWarmClothes.pants.Clear();
				result = num2;
			}
			return result;
		}

		// Token: 0x06002B1A RID: 11034 RVA: 0x0016C158 File Offset: 0x0016A558
		private int MissingWarmClothesCount(Map map)
		{
			int result;
			if (this.LowestTemperatureComing(map) >= ThingDefOf.Human.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null))
			{
				result = 0;
			}
			else
			{
				result = Mathf.Max(this.NeededWarmClothesCount(map) - this.ColonistsWithWarmClothesCount(map) - this.FreeWarmClothesSetsCount(map), 0);
			}
			return result;
		}

		// Token: 0x06002B1B RID: 11035 RVA: 0x0016C1B0 File Offset: 0x0016A5B0
		private float LowestTemperatureComing(Map map)
		{
			Twelfth twelfth = GenLocalDate.Twelfth(map);
			float a = this.GetTemperature(twelfth, map);
			for (int i = 0; i < 3; i++)
			{
				twelfth = twelfth.NextTwelfth();
				a = Mathf.Min(a, this.GetTemperature(twelfth, map));
			}
			return Mathf.Min(a, map.mapTemperature.OutdoorTemp);
		}

		// Token: 0x06002B1C RID: 11036 RVA: 0x0016C210 File Offset: 0x0016A610
		public override string GetExplanation()
		{
			Map map = this.MapWithMissingWarmClothes();
			string result;
			if (map == null)
			{
				result = "";
			}
			else
			{
				int num = this.MissingWarmClothesCount(map);
				if (num == this.NeededWarmClothesCount(map))
				{
					result = "NeedWarmClothesDesc1All".Translate() + "\n\n" + "NeedWarmClothesDesc2".Translate(new object[]
					{
						this.LowestTemperatureComing(map).ToStringTemperature("F0")
					});
				}
				else
				{
					result = "NeedWarmClothesDesc1".Translate(new object[]
					{
						num
					}) + "\n\n" + "NeedWarmClothesDesc2".Translate(new object[]
					{
						this.LowestTemperatureComing(map).ToStringTemperature("F0")
					});
				}
			}
			return result;
		}

		// Token: 0x06002B1D RID: 11037 RVA: 0x0016C2D8 File Offset: 0x0016A6D8
		public override AlertReport GetReport()
		{
			Map map = this.MapWithMissingWarmClothes();
			AlertReport result;
			if (map == null)
			{
				result = false;
			}
			else
			{
				float num = this.LowestTemperatureComing(map);
				foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
				{
					if (pawn.GetStatValue(StatDefOf.ComfyTemperatureMin, true) > num)
					{
						return pawn;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x0016C384 File Offset: 0x0016A784
		private Map MapWithMissingWarmClothes()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome)
				{
					if (this.LowestTemperatureComing(map) < 5f)
					{
						if (this.MissingWarmClothesCount(map) > 0)
						{
							return map;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x0016C400 File Offset: 0x0016A800
		private float GetTemperature(Twelfth twelfth, Map map)
		{
			return GenTemperature.AverageTemperatureAtTileForTwelfth(map.Tile, twelfth);
		}

		// Token: 0x0400172D RID: 5933
		private static List<Thing> jackets = new List<Thing>();

		// Token: 0x0400172E RID: 5934
		private static List<Thing> shirts = new List<Thing>();

		// Token: 0x0400172F RID: 5935
		private static List<Thing> pants = new List<Thing>();

		// Token: 0x04001730 RID: 5936
		private const float MedicinePerColonistThreshold = 2f;

		// Token: 0x04001731 RID: 5937
		private const int CheckNextTwelfthsCount = 3;

		// Token: 0x04001732 RID: 5938
		private const float CanShowAlertOnlyIfTempBelow = 5f;
	}
}
