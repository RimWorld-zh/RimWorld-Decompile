using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using RimWorld;

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

		public StatDef workTableEfficiencyStat;

		public StatDef workTableSpeedStat;

		public List<IngredientCount> ingredients = new List<IngredientCount>();

		public ThingFilter fixedIngredientFilter = new ThingFilter();

		public ThingFilter defaultIngredientFilter;

		public bool allowMixingIngredients;

		private Type ingredientValueGetterClass = typeof(IngredientValueGetter_Volume);

		public List<SpecialThingFilterDef> forceHiddenSpecialFilters;

		public bool autoStripCorpses = true;

		public List<ThingDefCountClass> products = new List<ThingDefCountClass>();

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

		[NoTranslate]
		public List<string> factionPrerequisiteTags;

		public ConceptDef conceptLearned;

		public bool dontShowIfAnyIngredientMissing;

		[Unsaved]
		private RecipeWorker workerInt;

		[Unsaved]
		private RecipeWorkerCounter workerCounterInt;

		[Unsaved]
		private IngredientValueGetter ingredientValueGetterInt;

		[Unsaved]
		private List<ThingDef> premultipliedSmallIngredients;

		[CompilerGenerated]
		private static Predicate<string> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<IngredientCount, IEnumerable<ThingDef>> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache2;

		public RecipeDef()
		{
		}

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
				if (this.researchPrerequisite != null && !this.researchPrerequisite.IsFinished)
				{
					return false;
				}
				if (this.factionPrerequisiteTags != null)
				{
					if (this.factionPrerequisiteTags.Any((string tag) => Faction.OfPlayer.def.recipePrerequisiteTags == null || !Faction.OfPlayer.def.recipePrerequisiteTags.Contains(tag)))
					{
						return false;
					}
				}
				return true;
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
					if (thingDefs[j].recipes != null && thingDefs[j].recipes.Contains(this))
					{
						yield return thingDefs[j];
					}
				}
				yield break;
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

		public ThingDef ProducedThingDef
		{
			get
			{
				if (this.specialProducts != null)
				{
					return null;
				}
				if (this.products == null || this.products.Count != 1)
				{
					return null;
				}
				return this.products[0].thingDef;
			}
		}

		public float WorkAmountTotal(ThingDef stuffDef)
		{
			if (this.workAmount >= 0f)
			{
				return this.workAmount;
			}
			return this.products[0].thingDef.GetStatValueAbstract(StatDefOf.WorkToMake, stuffDef);
		}

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
			foreach (string e in base.ConfigErrors())
			{
				yield return e;
			}
			if (this.workerClass == null)
			{
				yield return "workerClass is null.";
			}
			yield break;
		}

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

		public bool PawnSatisfiesSkillRequirements(Pawn pawn)
		{
			return this.FirstSkillRequirementPawnDoesntSatisfy(pawn) == null;
		}

		public SkillRequirement FirstSkillRequirementPawnDoesntSatisfy(Pawn pawn)
		{
			if (this.skillRequirements == null)
			{
				return null;
			}
			for (int i = 0; i < this.skillRequirements.Count; i++)
			{
				if (!this.skillRequirements[i].PawnSatisfies(pawn))
				{
					return this.skillRequirements[i];
				}
			}
			return null;
		}

		public List<ThingDef> GetPremultipliedSmallIngredients()
		{
			if (this.premultipliedSmallIngredients != null)
			{
				return this.premultipliedSmallIngredients;
			}
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
			return this.premultipliedSmallIngredients;
		}

		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (this.workSkill != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Skill".Translate(), this.workSkill.LabelCap, 0, string.Empty);
			}
			if (this.ingredients != null && this.ingredients.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Ingredients".Translate(), (from ic in this.ingredients
				select ic.Summary).ToCommaList(true), 0, string.Empty);
			}
			if (this.skillRequirements != null && this.skillRequirements.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "SkillRequirements".Translate(), (from sr in this.skillRequirements
				select sr.Summary).ToCommaList(true), 0, string.Empty);
			}
			if (this.products != null && this.products.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Products".Translate(), (from pr in this.products
				select pr.Summary).ToCommaList(true), 0, string.Empty);
			}
			if (this.workSpeedStat != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "WorkSpeedStat".Translate(), this.workSpeedStat.LabelCap, 0, string.Empty);
			}
			if (this.efficiencyStat != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "EfficiencyStat".Translate(), this.efficiencyStat.LabelCap, 0, string.Empty);
			}
			if (this.IsSurgery)
			{
				if (this.surgerySuccessChanceFactor >= 99999f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgerySuccessChanceFactor".Translate(), "Always", 0, string.Empty);
				}
				else
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgerySuccessChanceFactor".Translate(), this.surgerySuccessChanceFactor.ToStringPercent(), 0, string.Empty);
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private static bool <get_AvailableNow>m__0(string tag)
		{
			return Faction.OfPlayer.def.recipePrerequisiteTags == null || !Faction.OfPlayer.def.recipePrerequisiteTags.Contains(tag);
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<string> <ConfigErrors>__BaseCallProxy0()
		{
			return base.ConfigErrors();
		}

		[CompilerGenerated]
		private static IEnumerable<ThingDef> <GetPremultipliedSmallIngredients>m__1(IngredientCount ingredient)
		{
			return ingredient.filter.AllowedThingDefs;
		}

		[CompilerGenerated]
		private static bool <GetPremultipliedSmallIngredients>m__2(ThingDef td)
		{
			return td.smallVolume;
		}

		[CompilerGenerated]
		private bool <GetPremultipliedSmallIngredients>m__3(ThingDef td)
		{
			return !this.premultipliedSmallIngredients.Contains(td);
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<ThingDef>, IEnumerator, IDisposable, IEnumerator<ThingDef>
		{
			internal int <i>__1;

			internal List<ThingDef> <thingDefs>__0;

			internal int <i>__2;

			internal RecipeDef $this;

			internal ThingDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (this.recipeUsers == null)
					{
						goto IL_9A;
					}
					i = 0;
					break;
				case 1u:
					i++;
					break;
				case 2u:
					IL_11D:
					j++;
					goto IL_12B;
				default:
					return false;
				}
				if (i < this.recipeUsers.Count)
				{
					this.$current = this.recipeUsers[i];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				IL_9A:
				thingDefs = DefDatabase<ThingDef>.AllDefsListForReading;
				j = 0;
				IL_12B:
				if (j >= thingDefs.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (thingDefs[j].recipes != null && thingDefs[j].recipes.Contains(this))
					{
						this.$current = thingDefs[j];
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					goto IL_11D;
				}
				return false;
			}

			ThingDef IEnumerator<ThingDef>.Current
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
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.ThingDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				RecipeDef.<>c__Iterator0 <>c__Iterator = new RecipeDef.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <PotentiallyMissingIngredients>c__Iterator1 : IEnumerable, IEnumerable<ThingDef>, IEnumerator, IDisposable, IEnumerator<ThingDef>
		{
			internal int <i>__1;

			internal IngredientCount <ing>__2;

			internal bool <foundIng>__2;

			internal Map map;

			internal List<Thing> <thingList>__2;

			internal Pawn billDoer;

			internal ThingDef <def>__3;

			internal RecipeDef $this;

			internal ThingDef $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<ThingDef, float> <>f__am$cache0;

			[DebuggerHidden]
			public <PotentiallyMissingIngredients>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 0;
					goto IL_1DF;
				case 1u:
					break;
				case 2u:
					break;
				default:
					return false;
				}
				IL_1D1:
				i++;
				IL_1DF:
				if (i >= this.ingredients.Count)
				{
					this.$PC = -1;
				}
				else
				{
					ing = this.ingredients[i];
					foundIng = false;
					thingList = map.listerThings.ThingsInGroup(ThingRequestGroup.HaulableEver);
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
							this.$current = ing.filter.AllowedThingDefs.First<ThingDef>();
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
						}
						else
						{
							def = (from x in ing.filter.AllowedThingDefs
							orderby x.BaseMarketValue
							select x).FirstOrDefault((ThingDef x) => this.fixedIngredientFilter.Allows(x));
							if (def == null)
							{
								goto IL_1D1;
							}
							this.$current = def;
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
						}
						return true;
					}
					goto IL_1D1;
				}
				return false;
			}

			ThingDef IEnumerator<ThingDef>.Current
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
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.ThingDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				RecipeDef.<PotentiallyMissingIngredients>c__Iterator1 <PotentiallyMissingIngredients>c__Iterator = new RecipeDef.<PotentiallyMissingIngredients>c__Iterator1();
				<PotentiallyMissingIngredients>c__Iterator.$this = this;
				<PotentiallyMissingIngredients>c__Iterator.map = map;
				<PotentiallyMissingIngredients>c__Iterator.billDoer = billDoer;
				return <PotentiallyMissingIngredients>c__Iterator;
			}

			private static float <>m__0(ThingDef x)
			{
				return x.BaseMarketValue;
			}

			internal bool <>m__1(ThingDef x)
			{
				return this.fixedIngredientFilter.Allows(x);
			}
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator2 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal IEnumerator<string> $locvar0;

			internal string <e>__1;

			internal RecipeDef $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator2()
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
					enumerator = base.<ConfigErrors>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_E3;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						e = enumerator.Current;
						this.$current = e;
						if (!this.$disposing)
						{
							this.$PC = 1;
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
				if (this.workerClass != null)
				{
					goto IL_E3;
				}
				this.$current = "workerClass is null.";
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_E3:
				this.$PC = -1;
				return false;
			}

			string IEnumerator<string>.Current
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
				case 1u:
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
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				RecipeDef.<ConfigErrors>c__Iterator2 <ConfigErrors>c__Iterator = new RecipeDef.<ConfigErrors>c__Iterator2();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <SpecialDisplayStats>c__Iterator3 : IEnumerable, IEnumerable<StatDrawEntry>, IEnumerator, IDisposable, IEnumerator<StatDrawEntry>
		{
			internal RecipeDef $this;

			internal StatDrawEntry $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<IngredientCount, string> <>f__am$cache0;

			private static Func<SkillRequirement, string> <>f__am$cache1;

			private static Func<ThingDefCountClass, string> <>f__am$cache2;

			[DebuggerHidden]
			public <SpecialDisplayStats>c__Iterator3()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (this.workSkill != null)
					{
						this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "Skill".Translate(), this.workSkill.LabelCap, 0, string.Empty);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					goto IL_11E;
				case 3u:
					goto IL_1AB;
				case 4u:
					goto IL_238;
				case 5u:
					goto IL_28C;
				case 6u:
					goto IL_2E0;
				case 7u:
					goto IL_387;
				case 8u:
					goto IL_387;
				default:
					return false;
				}
				if (this.ingredients != null && this.ingredients.Count > 0)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "Ingredients".Translate(), (from ic in this.ingredients
					select ic.Summary).ToCommaList(true), 0, string.Empty);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_11E:
				if (this.skillRequirements != null && this.skillRequirements.Count > 0)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "SkillRequirements".Translate(), (from sr in this.skillRequirements
					select sr.Summary).ToCommaList(true), 0, string.Empty);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_1AB:
				if (this.products != null && this.products.Count > 0)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "Products".Translate(), (from pr in this.products
					select pr.Summary).ToCommaList(true), 0, string.Empty);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_238:
				if (this.workSpeedStat != null)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "WorkSpeedStat".Translate(), this.workSpeedStat.LabelCap, 0, string.Empty);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				IL_28C:
				if (this.efficiencyStat != null)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "EfficiencyStat".Translate(), this.efficiencyStat.LabelCap, 0, string.Empty);
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				}
				IL_2E0:
				if (base.IsSurgery)
				{
					if (this.surgerySuccessChanceFactor >= 99999f)
					{
						this.$current = new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgerySuccessChanceFactor".Translate(), "Always", 0, string.Empty);
						if (!this.$disposing)
						{
							this.$PC = 7;
						}
						return true;
					}
					this.$current = new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgerySuccessChanceFactor".Translate(), this.surgerySuccessChanceFactor.ToStringPercent(), 0, string.Empty);
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				}
				IL_387:
				this.$PC = -1;
				return false;
			}

			StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<RimWorld.StatDrawEntry>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				RecipeDef.<SpecialDisplayStats>c__Iterator3 <SpecialDisplayStats>c__Iterator = new RecipeDef.<SpecialDisplayStats>c__Iterator3();
				<SpecialDisplayStats>c__Iterator.$this = this;
				return <SpecialDisplayStats>c__Iterator;
			}

			private static string <>m__0(IngredientCount ic)
			{
				return ic.Summary;
			}

			private static string <>m__1(SkillRequirement sr)
			{
				return sr.Summary;
			}

			private static string <>m__2(ThingDefCountClass pr)
			{
				return pr.Summary;
			}
		}
	}
}
