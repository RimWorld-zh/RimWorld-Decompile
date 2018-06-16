using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F48 RID: 3912
	public static class GenRecipe
	{
		// Token: 0x06005E82 RID: 24194 RVA: 0x003002F0 File Offset: 0x002FE6F0
		public static IEnumerable<Thing> MakeRecipeProducts(RecipeDef recipeDef, Pawn worker, List<Thing> ingredients, Thing dominantIngredient, IBillGiver billGiver)
		{
			float efficiency;
			if (recipeDef.efficiencyStat == null)
			{
				efficiency = 1f;
			}
			else
			{
				efficiency = worker.GetStatValue(recipeDef.efficiencyStat, true);
			}
			if (recipeDef.workTableEfficiencyStat != null)
			{
				Building_WorkTable building_WorkTable = billGiver as Building_WorkTable;
				if (building_WorkTable != null)
				{
					efficiency *= building_WorkTable.GetStatValue(recipeDef.workTableEfficiencyStat, true);
				}
			}
			if (recipeDef.products != null)
			{
				for (int i = 0; i < recipeDef.products.Count; i++)
				{
					ThingDefCountClass prod = recipeDef.products[i];
					ThingDef stuffDef;
					if (prod.thingDef.MadeFromStuff)
					{
						stuffDef = dominantIngredient.def;
					}
					else
					{
						stuffDef = null;
					}
					Thing product = ThingMaker.MakeThing(prod.thingDef, stuffDef);
					product.stackCount = Mathf.CeilToInt((float)prod.count * efficiency);
					if (dominantIngredient != null)
					{
						product.SetColor(dominantIngredient.DrawColor, false);
					}
					CompIngredients ingredientsComp = product.TryGetComp<CompIngredients>();
					if (ingredientsComp != null)
					{
						for (int l = 0; l < ingredients.Count; l++)
						{
							ingredientsComp.RegisterIngredient(ingredients[l].def);
						}
					}
					CompFoodPoisonable foodPoisonable = product.TryGetComp<CompFoodPoisonable>();
					if (foodPoisonable != null)
					{
						float num = worker.GetStatValue(StatDefOf.FoodPoisonChance, true);
						Room room = worker.GetRoom(RegionType.Set_Passable);
						if (room != null)
						{
							num *= room.GetStat(RoomStatDefOf.FoodPoisonChanceFactor);
						}
						if (Rand.Value < num)
						{
							foodPoisonable.PoisonPercent = 1f;
						}
					}
					yield return GenRecipe.PostProcessProduct(product, recipeDef, worker);
				}
			}
			if (recipeDef.specialProducts != null)
			{
				for (int j = 0; j < recipeDef.specialProducts.Count; j++)
				{
					for (int k = 0; k < ingredients.Count; k++)
					{
						Thing ing = ingredients[k];
						SpecialProductType specialProductType = recipeDef.specialProducts[j];
						if (specialProductType != SpecialProductType.Butchery)
						{
							if (specialProductType == SpecialProductType.Smelted)
							{
								foreach (Thing product2 in ing.SmeltProducts(efficiency))
								{
									yield return GenRecipe.PostProcessProduct(product2, recipeDef, worker);
								}
							}
						}
						else
						{
							foreach (Thing product3 in ing.ButcherProducts(worker, efficiency))
							{
								yield return GenRecipe.PostProcessProduct(product3, recipeDef, worker);
							}
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06005E83 RID: 24195 RVA: 0x00300338 File Offset: 0x002FE738
		private static Thing PostProcessProduct(Thing product, RecipeDef recipeDef, Pawn worker)
		{
			CompQuality compQuality = product.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				if (recipeDef.workSkill == null)
				{
					Log.Error(recipeDef + " needs workSkill because it creates a product with a quality.", false);
				}
				QualityCategory q = QualityUtility.GenerateQualityCreatedByPawn(worker, recipeDef.workSkill);
				compQuality.SetQuality(q, ArtGenerationContext.Colony);
				QualityUtility.SendCraftNotification(product, worker);
			}
			CompArt compArt = product.TryGetComp<CompArt>();
			if (compArt != null)
			{
				compArt.JustCreatedBy(worker);
				if (compQuality.Quality >= QualityCategory.Excellent)
				{
					TaleRecorder.RecordTale(TaleDefOf.CraftedArt, new object[]
					{
						worker,
						product
					});
				}
			}
			if (product.def.Minifiable)
			{
				product = product.MakeMinified();
			}
			return product;
		}
	}
}
