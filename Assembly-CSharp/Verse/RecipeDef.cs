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

		public WorkTypeDef requiredGiverWorkType = null;

		public float workAmount = -1f;

		public StatDef workSpeedStat = null;

		public StatDef efficiencyStat = null;

		public StatDef workTableEfficiencyStat = null;

		public StatDef workTableSpeedStat = null;

		public List<IngredientCount> ingredients = new List<IngredientCount>();

		public ThingFilter fixedIngredientFilter = new ThingFilter();

		public ThingFilter defaultIngredientFilter = null;

		public bool allowMixingIngredients = false;

		private Type ingredientValueGetterClass = typeof(IngredientValueGetter_Volume);

		public List<SpecialThingFilterDef> forceHiddenSpecialFilters = null;

		public bool autoStripCorpses = true;

		public List<ThingDefCountClass> products = new List<ThingDefCountClass>();

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
		public string successfullyRemovedHediffMessage = null;

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

		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
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
						goto IL_9F;
					}
					i = 0;
					break;
				case 1u:
					i++;
					break;
				case 2u:
					IL_124:
					goto IL_125;
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
				IL_9F:
				thingDefs = DefDatabase<ThingDef>.AllDefsListForReading;
				j = 0;
				goto IL_134;
				IL_125:
				j++;
				IL_134:
				if (j >= thingDefs.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (thingDefs[j].recipes == null)
					{
						goto IL_125;
					}
					if (thingDefs[j].recipes.Contains(this))
					{
						this.$current = thingDefs[j];
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					goto IL_124;
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
					goto IL_1E9;
				case 1u:
					break;
				case 2u:
					IL_1D8:
					break;
				default:
					return false;
				}
				IL_1DA:
				i++;
				IL_1E9:
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
								goto IL_1D8;
							}
							this.$current = def;
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
						}
						return true;
					}
					goto IL_1DA;
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
					goto IL_EB;
				case 3u:
					goto IL_12F;
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
				if (this.workerClass == null)
				{
					this.$current = "workerClass is null.";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_EB:
				if (this.surgerySuccessChanceFactor < 99999f && !this.requireBed)
				{
					this.$current = "failable surgery does not require bed";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_12F:
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
						this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "Skill".Translate(), this.workSkill.LabelCap, 0, "");
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
					goto IL_123;
				case 3u:
					goto IL_1B0;
				case 4u:
					goto IL_23D;
				case 5u:
					goto IL_291;
				case 6u:
					goto IL_2E5;
				case 7u:
					goto IL_38D;
				case 8u:
					goto IL_38D;
				case 9u:
					goto IL_3D3;
				default:
					return false;
				}
				if (this.ingredients != null && this.ingredients.Count > 0)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "Ingredients".Translate(), (from ic in this.ingredients
					select ic.Summary).ToCommaList(true), 0, "");
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_123:
				if (this.skillRequirements != null && this.skillRequirements.Count > 0)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "SkillRequirements".Translate(), (from sr in this.skillRequirements
					select sr.Summary).ToCommaList(true), 0, "");
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_1B0:
				if (this.products != null && this.products.Count > 0)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "Products".Translate(), (from pr in this.products
					select pr.Summary).ToCommaList(true), 0, "");
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_23D:
				if (this.workSpeedStat != null)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "WorkSpeedStat".Translate(), this.workSpeedStat.LabelCap, 0, "");
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				IL_291:
				if (this.efficiencyStat != null)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "EfficiencyStat".Translate(), this.efficiencyStat.LabelCap, 0, "");
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				}
				IL_2E5:
				if (!base.IsSurgery)
				{
					goto IL_3D3;
				}
				if (this.surgerySuccessChanceFactor >= 99999f)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgerySuccessChanceFactor".Translate(), "Always", 0, "");
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				}
				this.$current = new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgerySuccessChanceFactor".Translate(), this.surgerySuccessChanceFactor.ToStringPercent(), 0, "");
				if (!this.$disposing)
				{
					this.$PC = 8;
				}
				return true;
				IL_38D:
				this.$current = new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgeryRequireBed".Translate(), this.requireBed.ToStringYesNo(), 0, "");
				if (!this.$disposing)
				{
					this.$PC = 9;
				}
				return true;
				IL_3D3:
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
