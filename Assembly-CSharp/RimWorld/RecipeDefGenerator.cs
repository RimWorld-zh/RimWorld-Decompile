using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	internal static class RecipeDefGenerator
	{
		public static IEnumerable<RecipeDef> ImpliedRecipeDefs()
		{
			using (IEnumerator<RecipeDef> enumerator = RecipeDefGenerator.DefsFromRecipeMakers().Concat(RecipeDefGenerator.DrugAdministerDefs()).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					RecipeDef r = enumerator.Current;
					yield return r;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_00bd:
			/*Error near IL_00be: Unexpected return in MoveNext()*/;
		}

		private static IEnumerable<RecipeDef> DefsFromRecipeMakers()
		{
			using (IEnumerator<ThingDef> enumerator = (from d in DefDatabase<ThingDef>.AllDefs
			where d.recipeMaker != null
			select d).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					ThingDef def = enumerator.Current;
					RecipeMakerProperties rm = def.recipeMaker;
					RecipeDef r = new RecipeDef
					{
						defName = "Make_" + def.defName,
						label = "RecipeMake".Translate(def.label),
						jobString = "RecipeMakeJobString".Translate(def.label),
						workAmount = (float)rm.workAmount,
						workSpeedStat = rm.workSpeedStat,
						efficiencyStat = rm.efficiencyStat
					};
					if (def.MadeFromStuff)
					{
						IngredientCount ingredientCount = new IngredientCount();
						ingredientCount.SetBaseCount((float)def.costStuffCount);
						ingredientCount.filter.SetAllowAllWhoCanMake(def);
						r.ingredients.Add(ingredientCount);
						r.fixedIngredientFilter.SetAllowAllWhoCanMake(def);
						r.productHasIngredientStuff = true;
					}
					if (def.costList != null)
					{
						foreach (ThingCountClass cost in def.costList)
						{
							IngredientCount ingredientCount2 = new IngredientCount();
							ingredientCount2.SetBaseCount((float)cost.count);
							ingredientCount2.filter.SetAllow(cost.thingDef, true);
							r.ingredients.Add(ingredientCount2);
						}
					}
					r.defaultIngredientFilter = rm.defaultIngredientFilter;
					r.products.Add(new ThingCountClass(def, rm.productCount));
					r.targetCountAdjustment = rm.targetCountAdjustment;
					r.skillRequirements = rm.skillRequirements.ListFullCopyOrNull();
					r.workSkill = rm.workSkill;
					r.workSkillLearnFactor = rm.workSkillLearnPerTick;
					r.unfinishedThingDef = rm.unfinishedThingDef;
					r.recipeUsers = rm.recipeUsers.ListFullCopyOrNull();
					r.effectWorking = rm.effectWorking;
					r.soundWorking = rm.soundWorking;
					r.researchPrerequisite = rm.researchPrerequisite;
					r.factionPrerequisiteTags = rm.factionPrerequisiteTags;
					yield return r;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_03bc:
			/*Error near IL_03bd: Unexpected return in MoveNext()*/;
		}

		private static IEnumerable<RecipeDef> DrugAdministerDefs()
		{
			using (IEnumerator<ThingDef> enumerator = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsDrug
			select d).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					ThingDef def = enumerator.Current;
					RecipeDef r = new RecipeDef
					{
						defName = "Administer_" + def.defName,
						label = "RecipeAdminister".Translate(def.label),
						jobString = "RecipeAdministerJobString".Translate(def.label),
						workerClass = typeof(Recipe_AdministerIngestible),
						targetsBodyPart = false,
						anesthetize = false,
						surgerySuccessChanceFactor = 99999f,
						workAmount = (float)def.ingestible.baseIngestTicks
					};
					IngredientCount ic = new IngredientCount();
					ic.SetBaseCount(1f);
					ic.filter.SetAllow(def, true);
					r.ingredients.Add(ic);
					r.fixedIngredientFilter.SetAllow(def, true);
					r.recipeUsers = new List<ThingDef>();
					foreach (ThingDef item in from d in DefDatabase<ThingDef>.AllDefs
					where d.category == ThingCategory.Pawn && d.race.IsFlesh
					select d)
					{
						r.recipeUsers.Add(item);
					}
					yield return r;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_029a:
			/*Error near IL_029b: Unexpected return in MoveNext()*/;
		}
	}
}
