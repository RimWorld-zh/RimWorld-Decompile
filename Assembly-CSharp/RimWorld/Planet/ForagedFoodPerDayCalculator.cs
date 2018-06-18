using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005F5 RID: 1525
	public static class ForagedFoodPerDayCalculator
	{
		// Token: 0x06001E54 RID: 7764 RVA: 0x00106020 File Offset: 0x00104420
		public static Pair<ThingDef, float> ForagedFoodPerDay(List<Pawn> pawns, BiomeDef biome, Faction faction, bool caravanMovingNow, bool caravanResting, StringBuilder explanation = null)
		{
			float foragedFoodCountPerInterval = ForagedFoodPerDayCalculator.GetForagedFoodCountPerInterval(pawns, biome, faction, explanation);
			float progressPerTick = ForagedFoodPerDayCalculator.GetProgressPerTick(caravanMovingNow, caravanResting, explanation);
			float num = foragedFoodCountPerInterval * progressPerTick * 60000f;
			float num2;
			if (num != 0f)
			{
				num2 = num * biome.foragedFood.GetStatValueAbstract(StatDefOf.Nutrition, null);
			}
			else
			{
				num2 = 0f;
			}
			if (explanation != null)
			{
				explanation.AppendLine();
				explanation.AppendLine();
				string text = string.Concat(new string[]
				{
					"TotalNutrition".Translate(),
					": ",
					num2.ToString("0.##"),
					" / ",
					"day".Translate()
				});
				if (num2 > 0f)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"\n= ",
						biome.LabelCap,
						": ",
						biome.foragedFood.label.CapitalizeFirst(),
						" x",
						num.ToString("0.##"),
						" / ",
						"day".Translate()
					});
				}
				explanation.Append(text);
			}
			return new Pair<ThingDef, float>(biome.foragedFood, num);
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x00106170 File Offset: 0x00104570
		public static float GetProgressPerTick(bool caravanMovingNow, bool caravanResting, StringBuilder explanation = null)
		{
			float num = 0.0001f;
			if (!caravanMovingNow && !caravanResting)
			{
				num *= 2f;
				if (explanation != null)
				{
					explanation.AppendLine();
					explanation.Append("CaravanNotMoving".Translate() + ": " + 2f.ToStringPercent());
				}
			}
			return num;
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x001061D8 File Offset: 0x001045D8
		public static float GetForagedFoodCountPerInterval(List<Pawn> pawns, BiomeDef biome, Faction faction, StringBuilder explanation = null)
		{
			float num = (biome.foragedFood == null) ? 0f : biome.forageability;
			if (explanation != null)
			{
				explanation.Append("ForagedNutritionPerDay".Translate() + ":");
			}
			float num2 = 0f;
			bool flag = false;
			int i = 0;
			int count = pawns.Count;
			while (i < count)
			{
				Pawn pawn = pawns[i];
				bool flag2;
				float baseForagedNutritionPerDay = ForagedFoodPerDayCalculator.GetBaseForagedNutritionPerDay(pawn, out flag2);
				if (!flag2)
				{
					num2 += baseForagedNutritionPerDay;
					flag = true;
					if (explanation != null)
					{
						explanation.AppendLine();
						explanation.Append("  - " + pawn.LabelShortCap + ": +" + baseForagedNutritionPerDay.ToString("0.##"));
					}
				}
				i++;
			}
			float num3 = num2;
			num2 /= 6f;
			if (explanation != null)
			{
				explanation.AppendLine();
				if (flag)
				{
					explanation.Append("  = " + num3.ToString("0.##"));
				}
				else
				{
					explanation.Append("  (" + "NoneCapable".Translate().ToLower() + ")");
				}
				explanation.AppendLine();
				explanation.AppendLine();
				explanation.Append(string.Concat(new string[]
				{
					"Biome".Translate(),
					": x",
					num.ToStringPercent(),
					" (",
					biome.label,
					")"
				}));
				if (faction.def.forageabilityFactor != 1f)
				{
					explanation.AppendLine();
					explanation.Append("  " + "FactionType".Translate() + ": " + faction.def.forageabilityFactor.ToStringPercent());
				}
			}
			num2 *= num;
			num2 *= faction.def.forageabilityFactor;
			float result;
			if (biome.foragedFood != null)
			{
				result = num2 / biome.foragedFood.ingestible.CachedNutrition;
			}
			else
			{
				result = num2;
			}
			return result;
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x001063F4 File Offset: 0x001047F4
		public static float GetBaseForagedNutritionPerDay(Pawn p, out bool skip)
		{
			float result;
			if (!p.IsFreeColonist || p.InMentalState || p.Downed)
			{
				skip = true;
				result = 0f;
			}
			else
			{
				skip = false;
				result = ((!StatDefOf.ForagedNutritionPerDay.Worker.IsDisabledFor(p)) ? p.GetStatValue(StatDefOf.ForagedNutritionPerDay, true) : 0f);
			}
			return result;
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x00106468 File Offset: 0x00104868
		public static Pair<ThingDef, float> ForagedFoodPerDay(Caravan caravan, StringBuilder explanation = null)
		{
			return ForagedFoodPerDayCalculator.ForagedFoodPerDay(caravan.PawnsListForReading, caravan.Biome, caravan.Faction, caravan.pather.MovingNow, caravan.Resting, explanation);
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x001064A8 File Offset: 0x001048A8
		public static float GetProgressPerTick(Caravan caravan, StringBuilder explanation = null)
		{
			return ForagedFoodPerDayCalculator.GetProgressPerTick(caravan.pather.MovingNow, caravan.Resting, explanation);
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x001064D4 File Offset: 0x001048D4
		public static float GetForagedFoodCountPerInterval(Caravan caravan, StringBuilder explanation = null)
		{
			return ForagedFoodPerDayCalculator.GetForagedFoodCountPerInterval(caravan.PawnsListForReading, caravan.Biome, caravan.Faction, explanation);
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x00106504 File Offset: 0x00104904
		public static Pair<ThingDef, float> ForagedFoodPerDay(List<TransferableOneWay> transferables, BiomeDef biome, Faction faction, StringBuilder explanation = null)
		{
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = 0; j < transferableOneWay.CountToTransfer; j++)
					{
						ForagedFoodPerDayCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			Pair<ThingDef, float> result = ForagedFoodPerDayCalculator.ForagedFoodPerDay(ForagedFoodPerDayCalculator.tmpPawns, biome, faction, true, false, explanation);
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x001065B8 File Offset: 0x001049B8
		public static Pair<ThingDef, float> ForagedFoodPerDayLeftAfterTransfer(List<TransferableOneWay> transferables, BiomeDef biome, Faction faction, StringBuilder explanation = null)
		{
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = transferableOneWay.things.Count - 1; j >= transferableOneWay.CountToTransfer; j--)
					{
						ForagedFoodPerDayCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			Pair<ThingDef, float> result = ForagedFoodPerDayCalculator.ForagedFoodPerDay(ForagedFoodPerDayCalculator.tmpPawns, biome, faction, true, false, explanation);
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x00106678 File Offset: 0x00104A78
		public static Pair<ThingDef, float> ForagedFoodPerDayLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, BiomeDef biome, Faction faction, StringBuilder explanation = null)
		{
			ForagedFoodPerDayCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, ForagedFoodPerDayCalculator.tmpThingCounts);
			Pair<ThingDef, float> result = ForagedFoodPerDayCalculator.ForagedFoodPerDay(ForagedFoodPerDayCalculator.tmpThingCounts, biome, faction, explanation);
			ForagedFoodPerDayCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x001066C0 File Offset: 0x00104AC0
		public static Pair<ThingDef, float> ForagedFoodPerDay(List<ThingCount> thingCounts, BiomeDef biome, Faction faction, StringBuilder explanation = null)
		{
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < thingCounts.Count; i++)
			{
				if (thingCounts[i].Count > 0)
				{
					Pawn pawn = thingCounts[i].Thing as Pawn;
					if (pawn != null)
					{
						ForagedFoodPerDayCalculator.tmpPawns.Add(pawn);
					}
				}
			}
			Pair<ThingDef, float> result = ForagedFoodPerDayCalculator.ForagedFoodPerDay(ForagedFoodPerDayCalculator.tmpPawns, biome, faction, true, false, explanation);
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x04001201 RID: 4609
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04001202 RID: 4610
		private static List<ThingCount> tmpThingCounts = new List<ThingCount>();

		// Token: 0x04001203 RID: 4611
		private const float BaseProgressPerTick = 0.0001f;

		// Token: 0x04001204 RID: 4612
		public const float NotMovingProgressFactor = 2f;
	}
}
