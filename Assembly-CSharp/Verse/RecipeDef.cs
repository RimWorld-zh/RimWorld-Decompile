using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B60 RID: 2912
	public class RecipeDef : Def
	{
		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x06003F9B RID: 16283 RVA: 0x00218794 File Offset: 0x00216B94
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

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x06003F9C RID: 16284 RVA: 0x002187E0 File Offset: 0x00216BE0
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

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x06003F9D RID: 16285 RVA: 0x0021882C File Offset: 0x00216C2C
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

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x06003F9E RID: 16286 RVA: 0x00218868 File Offset: 0x00216C68
		public bool AvailableNow
		{
			get
			{
				bool result;
				if (this.researchPrerequisite != null && !this.researchPrerequisite.IsFinished)
				{
					result = false;
				}
				else
				{
					if (this.factionPrerequisiteTags != null)
					{
						if (this.factionPrerequisiteTags.Any((string tag) => Faction.OfPlayer.def.recipePrerequisiteTags == null || !Faction.OfPlayer.def.recipePrerequisiteTags.Contains(tag)))
						{
							return false;
						}
					}
					result = true;
				}
				return result;
			}
		}

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x06003F9F RID: 16287 RVA: 0x002188E0 File Offset: 0x00216CE0
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
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							"   ",
							skillRequirement.skill.skillLabel.CapitalizeFirst(),
							": ",
							skillRequirement.minLevel
						}));
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

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x06003FA0 RID: 16288 RVA: 0x002189A8 File Offset: 0x00216DA8
		public IEnumerable<ThingDef> AllRecipeUsers
		{
			get
			{
				if (this.recipeUsers != null)
				{
					for (int i = 0; i < this.recipeUsers.Count; i++)
					{
						yield return this.recipeUsers[i];
					}
				}
				List<ThingDef> thingDefs = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int j = 0; j < thingDefs.Count; j++)
				{
					if (thingDefs[j].recipes != null)
					{
						if (thingDefs[j].recipes.Contains(this))
						{
							yield return thingDefs[j];
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x06003FA1 RID: 16289 RVA: 0x002189D4 File Offset: 0x00216DD4
		public bool UsesUnfinishedThing
		{
			get
			{
				return this.unfinishedThingDef != null;
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x06003FA2 RID: 16290 RVA: 0x002189F8 File Offset: 0x00216DF8
		public bool IsSurgery
		{
			get
			{
				foreach (ThingDef thingDef in this.AllRecipeUsers)
				{
					if (thingDef.category == ThingCategory.Pawn)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06003FA3 RID: 16291 RVA: 0x00218A6C File Offset: 0x00216E6C
		public ThingDef ProducedThingDef
		{
			get
			{
				ThingDef result;
				if (this.specialProducts != null)
				{
					result = null;
				}
				else if (this.products == null || this.products.Count != 1)
				{
					result = null;
				}
				else
				{
					result = this.products[0].thingDef;
				}
				return result;
			}
		}

		// Token: 0x06003FA4 RID: 16292 RVA: 0x00218AC8 File Offset: 0x00216EC8
		public float WorkAmountTotal(ThingDef stuffDef)
		{
			float statValueAbstract;
			if (this.workAmount >= 0f)
			{
				statValueAbstract = this.workAmount;
			}
			else
			{
				statValueAbstract = this.products[0].thingDef.GetStatValueAbstract(StatDefOf.WorkToMake, stuffDef);
			}
			return statValueAbstract;
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x00218B18 File Offset: 0x00216F18
		public IEnumerable<ThingDef> PotentiallyMissingIngredients(Pawn billDoer, Map map)
		{
			for (int i = 0; i < this.ingredients.Count; i++)
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
						yield return ing.filter.AllowedThingDefs.First<ThingDef>();
					}
					else
					{
						ThingDef def = (from x in ing.filter.AllowedThingDefs
						orderby x.BaseMarketValue
						select x).FirstOrDefault((ThingDef x) => this.fixedIngredientFilter.Allows(x));
						if (def != null)
						{
							yield return def;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x00218B50 File Offset: 0x00216F50
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

		// Token: 0x06003FA7 RID: 16295 RVA: 0x00218BD0 File Offset: 0x00216FD0
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			if (this.workerClass == null)
			{
				yield return "workerClass is null.";
			}
			if (this.surgerySuccessChanceFactor < 99999f && !this.requireBed)
			{
				yield return "failable surgery does not require bed";
			}
			yield break;
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x00218BFC File Offset: 0x00216FFC
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.workTableSpeedStat == null)
			{
				this.workTableSpeedStat = StatDefOf.WorkTableWorkSpeedFactor;
			}
			if (this.workTableEfficiencyStat == null)
			{
				this.workTableEfficiencyStat = StatDefOf.WorkTableEfficiencyFactor;
			}
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

		// Token: 0x06003FA9 RID: 16297 RVA: 0x00218CC0 File Offset: 0x002170C0
		public bool PawnSatisfiesSkillRequirements(Pawn pawn)
		{
			return this.FirstSkillRequirementPawnDoesntSatisfy(pawn) == null;
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x00218CE0 File Offset: 0x002170E0
		public SkillRequirement FirstSkillRequirementPawnDoesntSatisfy(Pawn pawn)
		{
			SkillRequirement result;
			if (this.skillRequirements == null)
			{
				result = null;
			}
			else
			{
				for (int i = 0; i < this.skillRequirements.Count; i++)
				{
					if (!this.skillRequirements[i].PawnSatisfies(pawn))
					{
						return this.skillRequirements[i];
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x00218D50 File Offset: 0x00217150
		public List<ThingDef> GetPremultipliedSmallIngredients()
		{
			List<ThingDef> result;
			if (this.premultipliedSmallIngredients != null)
			{
				result = this.premultipliedSmallIngredients;
			}
			else
			{
				this.premultipliedSmallIngredients = (from td in this.ingredients.SelectMany((IngredientCount ingredient) => ingredient.filter.AllowedThingDefs)
				where td.smallVolume
				select td).Distinct<ThingDef>().ToList<ThingDef>();
				bool flag = true;
				while (flag)
				{
					flag = false;
					for (int i = 0; i < this.ingredients.Count; i++)
					{
						bool flag2 = this.ingredients[i].filter.AllowedThingDefs.Any((ThingDef td) => !this.premultipliedSmallIngredients.Contains(td));
						if (flag2)
						{
							foreach (ThingDef item in this.ingredients[i].filter.AllowedThingDefs)
							{
								flag |= this.premultipliedSmallIngredients.Remove(item);
							}
						}
					}
				}
				result = this.premultipliedSmallIngredients;
			}
			return result;
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x00218EAC File Offset: 0x002172AC
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.workSkill != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Skill".Translate(), this.workSkill.LabelCap, 0, "");
			}
			if (this.ingredients != null && this.ingredients.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Ingredients".Translate(), (from ic in this.ingredients
				select ic.Summary).ToCommaList(true), 0, "");
			}
			if (this.skillRequirements != null && this.skillRequirements.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "SkillRequirements".Translate(), (from sr in this.skillRequirements
				select sr.Summary).ToCommaList(true), 0, "");
			}
			if (this.products != null && this.products.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Products".Translate(), (from pr in this.products
				select pr.Summary).ToCommaList(true), 0, "");
			}
			if (this.workSpeedStat != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "WorkSpeedStat".Translate(), this.workSpeedStat.LabelCap, 0, "");
			}
			if (this.efficiencyStat != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "EfficiencyStat".Translate(), this.efficiencyStat.LabelCap, 0, "");
			}
			if (this.IsSurgery)
			{
				if (this.surgerySuccessChanceFactor >= 99999f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgerySuccessChanceFactor".Translate(), "Always", 0, "");
				}
				else
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgerySuccessChanceFactor".Translate(), this.surgerySuccessChanceFactor.ToStringPercent(), 0, "");
				}
				yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgeryRequireBed".Translate(), this.requireBed.ToStringYesNo(), 0, "");
			}
			yield break;
		}

		// Token: 0x04002A7C RID: 10876
		public Type workerClass = typeof(RecipeWorker);

		// Token: 0x04002A7D RID: 10877
		public Type workerCounterClass = typeof(RecipeWorkerCounter);

		// Token: 0x04002A7E RID: 10878
		[MustTranslate]
		public string jobString = "Doing an unknown recipe.";

		// Token: 0x04002A7F RID: 10879
		public WorkTypeDef requiredGiverWorkType = null;

		// Token: 0x04002A80 RID: 10880
		public float workAmount = -1f;

		// Token: 0x04002A81 RID: 10881
		public StatDef workSpeedStat = null;

		// Token: 0x04002A82 RID: 10882
		public StatDef efficiencyStat = null;

		// Token: 0x04002A83 RID: 10883
		public StatDef workTableEfficiencyStat = null;

		// Token: 0x04002A84 RID: 10884
		public StatDef workTableSpeedStat = null;

		// Token: 0x04002A85 RID: 10885
		public List<IngredientCount> ingredients = new List<IngredientCount>();

		// Token: 0x04002A86 RID: 10886
		public ThingFilter fixedIngredientFilter = new ThingFilter();

		// Token: 0x04002A87 RID: 10887
		public ThingFilter defaultIngredientFilter = null;

		// Token: 0x04002A88 RID: 10888
		public bool allowMixingIngredients = false;

		// Token: 0x04002A89 RID: 10889
		private Type ingredientValueGetterClass = typeof(IngredientValueGetter_Volume);

		// Token: 0x04002A8A RID: 10890
		public List<SpecialThingFilterDef> forceHiddenSpecialFilters = null;

		// Token: 0x04002A8B RID: 10891
		public bool autoStripCorpses = true;

		// Token: 0x04002A8C RID: 10892
		public List<ThingDefCountClass> products = new List<ThingDefCountClass>();

		// Token: 0x04002A8D RID: 10893
		public List<SpecialProductType> specialProducts = null;

		// Token: 0x04002A8E RID: 10894
		public bool productHasIngredientStuff = false;

		// Token: 0x04002A8F RID: 10895
		public int targetCountAdjustment = 1;

		// Token: 0x04002A90 RID: 10896
		public ThingDef unfinishedThingDef = null;

		// Token: 0x04002A91 RID: 10897
		public List<SkillRequirement> skillRequirements = null;

		// Token: 0x04002A92 RID: 10898
		public SkillDef workSkill = null;

		// Token: 0x04002A93 RID: 10899
		public float workSkillLearnFactor = 1f;

		// Token: 0x04002A94 RID: 10900
		public EffecterDef effectWorking = null;

		// Token: 0x04002A95 RID: 10901
		public SoundDef soundWorking = null;

		// Token: 0x04002A96 RID: 10902
		public List<ThingDef> recipeUsers = null;

		// Token: 0x04002A97 RID: 10903
		public List<BodyPartDef> appliedOnFixedBodyParts = new List<BodyPartDef>();

		// Token: 0x04002A98 RID: 10904
		public HediffDef addsHediff = null;

		// Token: 0x04002A99 RID: 10905
		public HediffDef removesHediff = null;

		// Token: 0x04002A9A RID: 10906
		public bool hideBodyPartNames = false;

		// Token: 0x04002A9B RID: 10907
		public bool isViolation = false;

		// Token: 0x04002A9C RID: 10908
		[MustTranslate]
		public string successfullyRemovedHediffMessage = null;

		// Token: 0x04002A9D RID: 10909
		public float surgerySuccessChanceFactor = 1f;

		// Token: 0x04002A9E RID: 10910
		public float deathOnFailedSurgeryChance = 0f;

		// Token: 0x04002A9F RID: 10911
		public bool targetsBodyPart = true;

		// Token: 0x04002AA0 RID: 10912
		public bool anesthetize = true;

		// Token: 0x04002AA1 RID: 10913
		public bool requireBed = true;

		// Token: 0x04002AA2 RID: 10914
		public ResearchProjectDef researchPrerequisite = null;

		// Token: 0x04002AA3 RID: 10915
		[NoTranslate]
		public List<string> factionPrerequisiteTags = null;

		// Token: 0x04002AA4 RID: 10916
		public ConceptDef conceptLearned = null;

		// Token: 0x04002AA5 RID: 10917
		public bool dontShowIfAnyIngredientMissing;

		// Token: 0x04002AA6 RID: 10918
		[Unsaved]
		private RecipeWorker workerInt = null;

		// Token: 0x04002AA7 RID: 10919
		[Unsaved]
		private RecipeWorkerCounter workerCounterInt = null;

		// Token: 0x04002AA8 RID: 10920
		[Unsaved]
		private IngredientValueGetter ingredientValueGetterInt = null;

		// Token: 0x04002AA9 RID: 10921
		[Unsaved]
		private List<ThingDef> premultipliedSmallIngredients = null;
	}
}
