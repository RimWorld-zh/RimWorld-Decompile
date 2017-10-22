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
				for (int l = 0; l < recipeDef.products.Count; l++)
				{
					ThingCountClass prod = recipeDef.products[l];
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
						for (int k = 0; k < ingredients.Count; k++)
						{
							ingredientsComp.RegisterIngredient(ingredients[k].def);
						}
					}
					CompFoodPoisonable foodPoisonable = product3.TryGetComp<CompFoodPoisonable>();
					if (foodPoisonable != null)
					{
						float poisonChance = worker.GetStatValue(StatDefOf.FoodPoisonChance, true);
						Room room = worker.GetRoom(RegionType.Set_Passable);
						if (room != null)
						{
							poisonChance *= room.GetStat(RoomStatDefOf.FoodPoisonChanceFactor);
						}
						if (Rand.Value < poisonChance)
						{
							foodPoisonable.PoisonPercent = 1f;
						}
					}
					yield return GenRecipe.PostProcessProduct(product3, recipeDef, worker);
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
							foreach (Thing item in ing.ButcherProducts(worker, efficiency))
							{
								yield return GenRecipe.PostProcessProduct(item, recipeDef, worker);
							}
							break;
						}
						case SpecialProductType.Smelted:
						{
							foreach (Thing item2 in ing.SmeltProducts(efficiency))
							{
								yield return GenRecipe.PostProcessProduct(item2, recipeDef, worker);
							}
							break;
						}
						}
					}
				}
			}
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
				compQuality.SetQuality(QualityUtility.RandomCreationQuality(level), ArtGenerationContext.Colony);
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
