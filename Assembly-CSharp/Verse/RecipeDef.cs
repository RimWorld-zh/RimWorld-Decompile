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

		public WorkTypeDef requiredGiverWorkType = null;

		public float workAmount = -1f;

		public StatDef workSpeedStat = null;

		public StatDef efficiencyStat = null;

		public List<IngredientCount> ingredients = new List<IngredientCount>();

		public ThingFilter fixedIngredientFilter = new ThingFilter();

		public ThingFilter defaultIngredientFilter = null;

		public bool allowMixingIngredients = false;

		private Type ingredientValueGetterClass = typeof(IngredientValueGetter_Volume);

		public List<SpecialThingFilterDef> forceHiddenSpecialFilters = null;

		public List<ThingCountClass> products = new List<ThingCountClass>();

		public List<SpecialProductType> specialProducts = null;

		public bool productHasIngredientStuff = false;

		public int targetCountAdjustment = 1;

		public ThingDef unfinishedThingDef = null;

		public List<SkillRequirement> skillRequirements = null;

		public SkillDef workSkill = null;

		public float workSkillLearnFactor = 1f;

		public EffecterDef effectWorking = null;

		public SoundDef soundWorking = null;

		public List<ThingDef> recipeUsers = null;

		public List<BodyPartDef> appliedOnFixedBodyParts = new List<BodyPartDef>();

		public HediffDef addsHediff = null;

		public HediffDef removesHediff = null;

		public bool hideBodyPartNames = false;

		public bool isViolation = false;

		[MustTranslate]
		public string successfullyRemovedHediffMessage = (string)null;

		public float surgerySuccessChanceFactor = 1f;

		public float deathOnFailedSurgeryChance = 0f;

		public bool targetsBodyPart = true;

		public bool anesthetize = true;

		public bool requireBed = true;

		public ResearchProjectDef researchPrerequisite = null;

		[NoTranslate]
		public List<string> factionPrerequisiteTags = null;

		public ConceptDef conceptLearned = null;

		public bool dontShowIfAnyIngredientMissing;

		[Unsaved]
		private RecipeWorker workerInt = null;

		[Unsaved]
		private RecipeWorkerCounter workerCounterInt = null;

		[Unsaved]
		private IngredientValueGetter ingredientValueGetterInt = null;

		[Unsaved]
		private List<ThingDef> premultipliedSmallIngredients = null;

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
				return (byte)((this.researchPrerequisite == null || this.researchPrerequisite.IsFinished) ? ((this.factionPrerequisiteTags == null || !this.factionPrerequisiteTags.Any((Predicate<string>)((string tag) => Faction.OfPlayer.def.recipePrerequisiteTags == null || !Faction.OfPlayer.def.recipePrerequisiteTags.Contains(tag)))) ? 1 : 0) : 0) != 0;
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
						stringBuilder.AppendLine("   " + skillRequirement.skill.skillLabel.CapitalizeFirst() + ": " + skillRequirement.minLevel);
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
				if (this.recipeUsers != null)
				{
					int j = 0;
					if (j < this.recipeUsers.Count)
					{
						yield return this.recipeUsers[j];
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				List<ThingDef> thingDefs = DefDatabase<ThingDef>.AllDefsListForReading;
				int i = 0;
				while (true)
				{
					if (i < thingDefs.Count)
					{
						if (thingDefs[i].recipes != null && thingDefs[i].recipes.Contains(this))
							break;
						i++;
						continue;
					}
					yield break;
				}
				yield return thingDefs[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public bool UsesUnfinishedThing
		{
			get
			{
				return this.unfinishedThingDef != null;
			}
		}

		public bool IsSurgery
		{
			get
			{
				foreach (ThingDef allRecipeUser in this.AllRecipeUsers)
				{
					if (allRecipeUser.category == ThingCategory.Pawn)
					{
						return true;
					}
				}
				return false;
			}
		}

		public float WorkAmountTotal(ThingDef stuffDef)
		{
			return (!(this.workAmount >= 0.0)) ? this.products[0].thingDef.GetStatValueAbstract(StatDefOf.WorkToMake, stuffDef) : this.workAmount;
		}

		public IEnumerable<ThingDef> PotentiallyMissingIngredients(Pawn billDoer, Map map)
		{
			int i = 0;
			ThingDef def;
			while (true)
			{
				if (i < this.ingredients.Count)
				{
					IngredientCount ing = this.ingredients[i];
					bool foundIng = false;
					List<Thing> thingList = map.listerThings.ThingsInGroup(ThingRequestGroup.HaulableEver);
					for (int j = 0; j < thingList.Count; j++)
					{
						Thing thing = thingList[j];
						if ((billDoer == null || !thing.IsForbidden(billDoer)) && !thing.Position.Fogged(map) && (ing.IsFixedIngredient || this.fixedIngredientFilter.Allows(thing)) && ing.filter.Allows(thing))
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
							/*Error: Unable to find new state assignment for yield return*/;
						}
						def = (from x in ing.filter.AllowedThingDefs
						orderby x.BaseMarketValue
						select x).FirstOrDefault((Func<ThingDef, bool>)((ThingDef x) => ((_003CPotentiallyMissingIngredients_003Ec__Iterator1)/*Error near IL_0197: stateMachine*/)._0024this.fixedIngredientFilter.Allows(x)));
						if (def != null)
							break;
					}
					i++;
					continue;
				}
				yield break;
			}
			yield return def;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public bool IsIngredient(ThingDef th)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.ingredients.Count)
				{
					if (this.ingredients[num].filter.Allows(th) && (this.ingredients[num].IsFixedIngredient || this.fixedIngredientFilter.Allows(th)))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string e = enumerator.Current;
					yield return e;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.workerClass == null)
			{
				yield return "workerClass is null.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (!(this.surgerySuccessChanceFactor < 99999.0))
				yield break;
			if (this.requireBed)
				yield break;
			yield return "failable surgery does not require bed";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0138:
			/*Error near IL_0139: Unexpected return in MoveNext()*/;
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
			List<ThingDef> result;
			if (this.premultipliedSmallIngredients != null)
			{
				result = this.premultipliedSmallIngredients;
			}
			else
			{
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
				result = this.premultipliedSmallIngredients;
			}
			return result;
		}

		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.workSkill != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Skill".Translate(), this.workSkill.LabelCap, 0, "");
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.ingredients != null && this.ingredients.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Ingredients".Translate(), GenText.ToCommaList(from ic in this.ingredients
				select ic.Summary, true), 0, "");
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.skillRequirements != null && this.skillRequirements.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "SkillRequirements".Translate(), GenText.ToCommaList(from sr in this.skillRequirements
				select sr.Summary, true), 0, "");
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.products != null && this.products.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Products".Translate(), GenText.ToCommaList(from pr in this.products
				select pr.Summary, true), 0, "");
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.workSpeedStat != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "WorkSpeedStat".Translate(), this.workSpeedStat.LabelCap, 0, "");
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.efficiencyStat != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "EfficiencyStat".Translate(), this.efficiencyStat.LabelCap, 0, "");
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (!this.IsSurgery)
				yield break;
			if (this.surgerySuccessChanceFactor >= 99999.0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgerySuccessChanceFactor".Translate(), "Always", 0, "");
				/*Error: Unable to find new state assignment for yield return*/;
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgerySuccessChanceFactor".Translate(), this.surgerySuccessChanceFactor.ToStringPercent(), 0, "");
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
