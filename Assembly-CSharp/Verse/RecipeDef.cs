using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Verse
{
	public class RecipeDef : Def
	{
		public Type workerClass = typeof(RecipeWorker);

		public Type workerCounterClass = typeof(RecipeWorkerCounter);

		[MustTranslate]
		public string jobString = "Doing an unknown recipe.";

		public WorkTypeDef requiredGiverWorkType;

		public float workAmount = -1f;

		public StatDef workSpeedStat;

		public StatDef efficiencyStat;

		public List<IngredientCount> ingredients = new List<IngredientCount>();

		public ThingFilter fixedIngredientFilter = new ThingFilter();

		public ThingFilter defaultIngredientFilter;

		public bool allowMixingIngredients;

		private Type ingredientValueGetterClass = typeof(IngredientValueGetter_Volume);

		public List<SpecialThingFilterDef> forceHiddenSpecialFilters;

		public List<ThingCountClass> products = new List<ThingCountClass>();

		public List<SpecialProductType> specialProducts;

		public bool productHasIngredientStuff;

		public int targetCountAdjustment = 1;

		public ThingDef unfinishedThingDef;

		public List<SkillRequirement> skillRequirements;

		public SkillDef workSkill;

		public float workSkillLearnFactor = 1f;

		public EffecterDef effectWorking;

		public SoundDef soundWorking;

		public List<ThingDef> recipeUsers;

		public List<BodyPartDef> appliedOnFixedBodyParts = new List<BodyPartDef>();

		public HediffDef addsHediff;

		public HediffDef removesHediff;

		public bool hideBodyPartNames;

		public bool isViolation;

		[MustTranslate]
		public string successfullyRemovedHediffMessage;

		public float surgerySuccessChanceFactor = 1f;

		public float deathOnFailedSurgeryChance;

		public bool targetsBodyPart = true;

		public bool anesthetize = true;

		public ResearchProjectDef researchPrerequisite;

		public ConceptDef conceptLearned;

		[Unsaved]
		private RecipeWorker workerInt;

		[Unsaved]
		private RecipeWorkerCounter workerCounterInt;

		[Unsaved]
		private IngredientValueGetter ingredientValueGetterInt;

		[Unsaved]
		private List<ThingDef> premultipliedSmallIngredients;

		public RecipeWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RecipeWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.recipe = this;
				}
				return this.workerInt;
			}
		}

		public RecipeWorkerCounter WorkerCounter
		{
			get
			{
				if (this.workerCounterInt == null)
				{
					this.workerCounterInt = (RecipeWorkerCounter)Activator.CreateInstance(this.workerCounterClass);
					this.workerCounterInt.recipe = this;
				}
				return this.workerCounterInt;
			}
		}

		public IngredientValueGetter IngredientValueGetter
		{
			get
			{
				if (this.ingredientValueGetterInt == null)
				{
					this.ingredientValueGetterInt = (IngredientValueGetter)Activator.CreateInstance(this.ingredientValueGetterClass);
				}
				return this.ingredientValueGetterInt;
			}
		}

		public bool AvailableNow
		{
			get
			{
				return this.researchPrerequisite == null || this.researchPrerequisite.IsFinished;
			}
		}

		public string MinSkillString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = false;
				if (this.skillRequirements != null)
				{
					for (int i = 0; i < this.skillRequirements.Count; i++)
					{
						SkillRequirement skillRequirement = this.skillRequirements[i];
						stringBuilder.AppendLine("   " + skillRequirement.skill.skillLabel + ": " + skillRequirement.minLevel);
						flag = true;
					}
				}
				if (!flag)
				{
					stringBuilder.AppendLine("   (" + "NoneLower".Translate() + ")");
				}
				return stringBuilder.ToString();
			}
		}

		public IEnumerable<ThingDef> AllRecipeUsers
		{
			get
			{
				for (int j = 0; j < this.recipeUsers.Count; j++)
				{
					yield return this.recipeUsers[j];
				}
				List<ThingDef> thingDefs = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int i = 0; i < thingDefs.Count; i++)
				{
					if (thingDefs[i].recipes != null && thingDefs[i].recipes.Contains(this))
					{
						yield return thingDefs[i];
					}
				}
			}
		}

		public bool UsesUnfinishedThing
		{
			get
			{
				return this.unfinishedThingDef != null;
			}
		}

		public float WorkAmountTotal(ThingDef stuffDef)
		{
			if (this.workAmount >= 0.0)
			{
				return this.workAmount;
			}
			return this.products[0].thingDef.GetStatValueAbstract(StatDefOf.WorkToMake, stuffDef);
		}

		public IEnumerable<ThingDef> PotentiallyMissingIngredients(Pawn billDoer, Map map)
		{
			for (int j = 0; j < this.ingredients.Count; j++)
			{
				IngredientCount ing = this.ingredients[j];
				bool foundIng = false;
				List<Thing> thingList = map.listerThings.ThingsInGroup(ThingRequestGroup.HaulableEver);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing t = thingList[i];
					if ((billDoer == null || !t.IsForbidden(billDoer)) && !t.Position.Fogged(map) && (ing.IsFixedIngredient || this.fixedIngredientFilter.Allows(t)) && ing.filter.Allows(t))
					{
						foundIng = true;
						break;
					}
				}
				if (!foundIng)
				{
					if (ing.IsFixedIngredient)
					{
						yield return ing.filter.AllowedThingDefs.First();
					}
					else
					{
						ThingDef def = (from x in ing.filter.AllowedThingDefs
						orderby x.BaseMarketValue
						select x).FirstOrDefault((Func<ThingDef, bool>)((ThingDef x) => ((_003CPotentiallyMissingIngredients_003Ec__Iterator1D7)/*Error near IL_01ba: stateMachine*/)._003C_003Ef__this.fixedIngredientFilter.Allows(x)));
						if (def != null)
						{
							yield return def;
						}
					}
				}
			}
		}

		public bool IsIngredient(ThingDef th)
		{
			for (int i = 0; i < this.ingredients.Count; i++)
			{
				if (this.ingredients[i].filter.Allows(th) && (this.ingredients[i].IsFixedIngredient || this.fixedIngredientFilter.Allows(th)))
				{
					return true;
				}
			}
			return false;
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string item in base.ConfigErrors())
			{
				yield return item;
			}
			if (this.workerClass == null)
			{
				yield return "workerClass is null.";
			}
		}

		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.ingredients.Count; i++)
			{
				this.ingredients[i].ResolveReferences();
			}
			if (this.fixedIngredientFilter != null)
			{
				this.fixedIngredientFilter.ResolveReferences();
			}
			if (this.defaultIngredientFilter == null)
			{
				this.defaultIngredientFilter = new ThingFilter();
				if (this.fixedIngredientFilter != null)
				{
					this.defaultIngredientFilter.CopyAllowancesFrom(this.fixedIngredientFilter);
				}
			}
			this.defaultIngredientFilter.ResolveReferences();
		}

		public bool PawnSatisfiesSkillRequirements(Pawn pawn)
		{
			return this.skillRequirements == null || !this.skillRequirements.Any((Predicate<SkillRequirement>)((SkillRequirement req) => !req.PawnSatisfies(pawn)));
		}

		public List<ThingDef> GetPremultipliedSmallIngredients()
		{
			if (this.premultipliedSmallIngredients != null)
			{
				return this.premultipliedSmallIngredients;
			}
			this.premultipliedSmallIngredients = (from td in this.ingredients.SelectMany((Func<IngredientCount, IEnumerable<ThingDef>>)((IngredientCount ingredient) => ingredient.filter.AllowedThingDefs))
			where td.smallVolume
			select td).Distinct().ToList();
			bool flag = true;
			while (flag)
			{
				flag = false;
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					if (this.ingredients[i].filter.AllowedThingDefs.Any((Func<ThingDef, bool>)((ThingDef td) => !this.premultipliedSmallIngredients.Contains(td))))
					{
						foreach (ThingDef allowedThingDef in this.ingredients[i].filter.AllowedThingDefs)
						{
							flag |= this.premultipliedSmallIngredients.Remove(allowedThingDef);
						}
					}
				}
			}
			return this.premultipliedSmallIngredients;
		}
	}
}
