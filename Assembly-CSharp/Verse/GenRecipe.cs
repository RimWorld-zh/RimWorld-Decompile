using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public static class GenRecipe
	{
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

		[CompilerGenerated]
		private sealed class <MakeRecipeProducts>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal RecipeDef recipeDef;

			internal float <efficiency>__0;

			internal Pawn worker;

			internal IBillGiver billGiver;

			internal int <i>__1;

			internal ThingDefCountClass <prod>__2;

			internal Thing dominantIngredient;

			internal ThingDef <stuffDef>__2;

			internal Thing <product>__2;

			internal CompIngredients <ingredientsComp>__2;

			internal List<Thing> ingredients;

			internal CompFoodPoisonable <foodPoisonable>__2;

			internal int <i>__3;

			internal int <j>__4;

			internal Thing <ing>__5;

			internal SpecialProductType $locvar0;

			internal IEnumerator<Thing> $locvar1;

			internal Thing <product>__6;

			internal IEnumerator<Thing> $locvar2;

			internal Thing <product>__7;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <MakeRecipeProducts>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
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
					if (recipeDef.products == null)
					{
						goto IL_2A0;
					}
					i = 0;
					break;
				case 1u:
					i++;
					break;
				case 2u:
					Block_16:
					try
					{
						switch (num)
						{
						}
						if (enumerator2.MoveNext())
						{
							product3 = enumerator2.Current;
							this.$current = GenRecipe.PostProcessProduct(product3, recipeDef, worker);
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
					}
					goto IL_478;
				case 3u:
					Block_17:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							product2 = enumerator.Current;
							this.$current = GenRecipe.PostProcessProduct(product2, recipeDef, worker);
							if (!this.$disposing)
							{
								this.$PC = 3;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator != null)
							{
								enumerator.Dispose();
							}
						}
					}
					goto IL_478;
				default:
					return false;
				}
				if (i < recipeDef.products.Count)
				{
					prod = recipeDef.products[i];
					if (prod.thingDef.MadeFromStuff)
					{
						stuffDef = dominantIngredient.def;
					}
					else
					{
						stuffDef = null;
					}
					product = ThingMaker.MakeThing(prod.thingDef, stuffDef);
					product.stackCount = Mathf.CeilToInt((float)prod.count * efficiency);
					if (dominantIngredient != null)
					{
						product.SetColor(dominantIngredient.DrawColor, false);
					}
					ingredientsComp = product.TryGetComp<CompIngredients>();
					if (ingredientsComp != null)
					{
						for (int l = 0; l < ingredients.Count; l++)
						{
							ingredientsComp.RegisterIngredient(ingredients[l].def);
						}
					}
					foodPoisonable = product.TryGetComp<CompFoodPoisonable>();
					if (foodPoisonable != null)
					{
						float num2 = worker.GetStatValue(StatDefOf.FoodPoisonChance, true);
						Room room = worker.GetRoom(RegionType.Set_Passable);
						if (room != null)
						{
							num2 *= room.GetStat(RoomStatDefOf.FoodPoisonChanceFactor);
						}
						if (Rand.Value < num2)
						{
							foodPoisonable.PoisonPercent = 1f;
						}
					}
					this.$current = GenRecipe.PostProcessProduct(product, recipeDef, worker);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				IL_2A0:
				if (recipeDef.specialProducts != null)
				{
					j = 0;
					goto IL_4AC;
				}
				goto IL_4C8;
				IL_478:
				k++;
				IL_487:
				if (k >= ingredients.Count)
				{
					j++;
				}
				else
				{
					ing = ingredients[k];
					specialProductType = recipeDef.specialProducts[j];
					if (specialProductType == SpecialProductType.Butchery)
					{
						enumerator2 = ing.ButcherProducts(worker, efficiency).GetEnumerator();
						num = 4294967293u;
						goto Block_16;
					}
					if (specialProductType != SpecialProductType.Smelted)
					{
						goto IL_478;
					}
					enumerator = ing.SmeltProducts(efficiency).GetEnumerator();
					num = 4294967293u;
					goto Block_17;
				}
				IL_4AC:
				if (j < recipeDef.specialProducts.Count)
				{
					k = 0;
					goto IL_487;
				}
				IL_4C8:
				this.$PC = -1;
				return false;
			}

			Thing IEnumerator<Thing>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					break;
				case 3u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Thing>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GenRecipe.<MakeRecipeProducts>c__Iterator0 <MakeRecipeProducts>c__Iterator = new GenRecipe.<MakeRecipeProducts>c__Iterator0();
				<MakeRecipeProducts>c__Iterator.recipeDef = recipeDef;
				<MakeRecipeProducts>c__Iterator.worker = worker;
				<MakeRecipeProducts>c__Iterator.billGiver = billGiver;
				<MakeRecipeProducts>c__Iterator.dominantIngredient = dominantIngredient;
				<MakeRecipeProducts>c__Iterator.ingredients = ingredients;
				return <MakeRecipeProducts>c__Iterator;
			}
		}
	}
}
