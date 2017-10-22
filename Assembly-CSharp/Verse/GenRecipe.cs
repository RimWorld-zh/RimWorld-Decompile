using RimWorld;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public static class GenRecipe
	{
		public static IEnumerable<Thing> MakeRecipeProducts(RecipeDef recipeDef, Pawn worker, List<Thing> ingredients, Thing dominantIngredient)
		{
			float efficiency = (float)((recipeDef.efficiencyStat != null) ? worker.GetStatValue(recipeDef.efficiencyStat, true) : 1.0);
			if (recipeDef.products != null)
			{
				int k = 0;
				if (k < recipeDef.products.Count)
				{
					ThingCountClass prod = recipeDef.products[k];
					ThingDef stuffDef = (!prod.thingDef.MadeFromStuff) ? null : dominantIngredient.def;
					Thing product3 = ThingMaker.MakeThing(prod.thingDef, stuffDef);
					product3.stackCount = Mathf.CeilToInt((float)prod.count * efficiency);
					if (dominantIngredient != null)
					{
						product3.SetColor(dominantIngredient.DrawColor, false);
					}
					CompIngredients ingredientsComp = product3.TryGetComp<CompIngredients>();
					if (ingredientsComp != null)
					{
						for (int l = 0; l < ingredients.Count; l++)
						{
							ingredientsComp.RegisterIngredient(ingredients[l].def);
						}
					}
					CompFoodPoisonable foodPoisonable = product3.TryGetComp<CompFoodPoisonable>();
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
					yield return GenRecipe.PostProcessProduct(product3, recipeDef, worker);
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (recipeDef.specialProducts != null)
			{
				for (int j = 0; j < recipeDef.specialProducts.Count; j++)
				{
					for (int i = 0; i < ingredients.Count; i++)
					{
						Thing ing = ingredients[i];
						switch (recipeDef.specialProducts[j])
						{
						case SpecialProductType.Butchery:
						{
							using (IEnumerator<Thing> enumerator = ing.ButcherProducts(worker, efficiency).GetEnumerator())
							{
								if (enumerator.MoveNext())
								{
									Thing product = enumerator.Current;
									yield return GenRecipe.PostProcessProduct(product, recipeDef, worker);
									/*Error: Unable to find new state assignment for yield return*/;
								}
							}
							break;
						}
						case SpecialProductType.Smelted:
						{
							using (IEnumerator<Thing> enumerator2 = ing.SmeltProducts(efficiency).GetEnumerator())
							{
								if (enumerator2.MoveNext())
								{
									Thing product2 = enumerator2.Current;
									yield return GenRecipe.PostProcessProduct(product2, recipeDef, worker);
									/*Error: Unable to find new state assignment for yield return*/;
								}
							}
							break;
						}
						}
					}
				}
			}
			yield break;
			IL_048a:
			/*Error near IL_048b: Unexpected return in MoveNext()*/;
		}

		private static Thing PostProcessProduct(Thing product, RecipeDef recipeDef, Pawn worker)
		{
			CompQuality compQuality = product.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				if (recipeDef.workSkill == null)
				{
					Log.Error(recipeDef + " needs workSkill because it creates a product with a quality.");
				}
				int level = worker.skills.GetSkill(recipeDef.workSkill).Level;
				QualityCategory qualityCategory = QualityUtility.RandomCreationQuality(level);
				if (worker.InspirationDef == InspirationDefOf.InspiredArt && (product.def.IsArt || (product.def.minifiedDef != null && product.def.minifiedDef.IsArt)))
				{
					qualityCategory = qualityCategory.AddLevels(3);
					worker.mindState.inspirationHandler.EndInspiration(InspirationDefOf.InspiredArt);
				}
				compQuality.SetQuality(qualityCategory, ArtGenerationContext.Colony);
			}
			CompArt compArt = product.TryGetComp<CompArt>();
			if (compArt != null)
			{
				compArt.JustCreatedBy(worker);
				if ((int)compQuality.Quality >= 6)
				{
					TaleRecorder.RecordTale(TaleDefOf.CraftedArt, worker, product);
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
