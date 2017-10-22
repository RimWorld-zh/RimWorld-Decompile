using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	internal static class RecipeDefGenerator
	{
		public static IEnumerable<RecipeDef> ImpliedRecipeDefs()
		{
			foreach (RecipeDef item in RecipeDefGenerator.DefsFromRecipeMakers().Concat(RecipeDefGenerator.DrugAdministerDefs()))
			{
				yield return item;
			}
		}

		private static IEnumerable<RecipeDef> DefsFromRecipeMakers()
		{
			foreach (ThingDef item in from d in DefDatabase<ThingDef>.AllDefs
			where d.recipeMaker != null
			select d)
			{
				RecipeMakerProperties rm = item.recipeMaker;
				RecipeDef r = new RecipeDef
				{
					defName = "Make_" + item.defName,
					label = "RecipeMake".Translate(item.label),
					jobString = "RecipeMakeJobString".Translate(item.label),
					workAmount = (float)rm.workAmount,
					workSpeedStat = rm.workSpeedStat,
					efficiencyStat = rm.efficiencyStat
				};
				if (item.MadeFromStuff)
				{
					IngredientCount ic2 = new IngredientCount();
					ic2.SetBaseCount((float)item.costStuffCount);
					ic2.filter.SetAllowAllWhoCanMake(item);
					r.ingredients.Add(ic2);
					r.fixedIngredientFilter.SetAllowAllWhoCanMake(item);
					r.productHasIngredientStuff = true;
				}
				if (item.costList != null)
				{
					List<ThingCountClass>.Enumerator enumerator2 = item.costList.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							ThingCountClass c = enumerator2.Current;
							IngredientCount ic = new IngredientCount();
							ic.SetBaseCount((float)c.count);
							ic.filter.SetAllow(c.thingDef, true);
							r.ingredients.Add(ic);
						}
					}
					finally
					{
						((IDisposable)(object)enumerator2).Dispose();
					}
				}
				r.defaultIngredientFilter = rm.defaultIngredientFilter;
				r.products.Add(new ThingCountClass(item, rm.productCount));
				r.skillRequirements = rm.skillRequirements.ListFullCopyOrNull();
				r.workSkill = rm.workSkill;
				r.workSkillLearnFactor = rm.workSkillLearnPerTick;
				r.unfinishedThingDef = rm.unfinishedThingDef;
				r.recipeUsers = rm.recipeUsers.ListFullCopyOrNull();
				r.effectWorking = rm.effectWorking;
				r.soundWorking = rm.soundWorking;
				r.researchPrerequisite = rm.researchPrerequisite;
				yield return r;
			}
		}

		private static IEnumerable<RecipeDef> DrugAdministerDefs()
		{
			foreach (ThingDef item in from d in DefDatabase<ThingDef>.AllDefs
			where d.IsDrug
			select d)
			{
				RecipeDef r = new RecipeDef
				{
					defName = "Administer_" + item.defName,
					label = "RecipeAdminister".Translate(item.label),
					jobString = "RecipeAdministerJobString".Translate(item.label),
					workerClass = typeof(Recipe_AdministerIngestible),
					targetsBodyPart = false,
					anesthetize = false,
					workAmount = (float)item.ingestible.baseIngestTicks
				};
				IngredientCount ic = new IngredientCount();
				ic.SetBaseCount(1f);
				ic.filter.SetAllow(item, true);
				r.ingredients.Add(ic);
				r.fixedIngredientFilter.SetAllow(item, true);
				r.recipeUsers = new List<ThingDef>();
				foreach (ThingDef item2 in DefDatabase<ThingDef>.AllDefs.Where((Func<ThingDef, bool>)((ThingDef d) => d.category == ThingCategory.Pawn && d.race.IsFlesh)))
				{
					r.recipeUsers.Add(item2);
				}
				yield return r;
			}
		}
	}
}
