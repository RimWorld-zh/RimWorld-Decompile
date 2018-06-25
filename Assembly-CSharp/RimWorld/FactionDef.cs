using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class FactionDef : Def
	{
		public bool isPlayer = false;

		public RulePackDef factionNameMaker;

		public RulePackDef settlementNameMaker;

		public RulePackDef playerInitialSettlementNameMaker;

		[MustTranslate]
		public string fixedName = null;

		public bool humanlikeFaction = true;

		public bool hidden = false;

		public float listOrderPriority = 0f;

		public List<PawnGroupMaker> pawnGroupMakers = null;

		public SimpleCurve raidCommonalityFromPointsCurve = null;

		public bool autoFlee = true;

		public bool canSiege = false;

		public bool canStageAttacks = false;

		public bool canUseAvoidGrid = true;

		public float earliestRaidDays = 0f;

		public FloatRange allowedArrivalTemperatureRange = new FloatRange(-1000f, 1000f);

		public PawnKindDef basicMemberKind;

		public List<ResearchProjectTagDef> startingResearchTags = null;

		[NoTranslate]
		public List<string> recipePrerequisiteTags = null;

		public bool rescueesCanJoin = false;

		[MustTranslate]
		public string pawnSingular = "member";

		[MustTranslate]
		public string pawnsPlural = "members";

		public string leaderTitle = "leader";

		public float forageabilityFactor = 1f;

		public SimpleCurve maxPawnCostPerTotalPointsCurve = null;

		public int requiredCountAtGameStart = 0;

		public int maxCountAtGameStart = 9999;

		public bool canMakeRandomly = false;

		public float settlementGenerationWeight = 0f;

		public RulePackDef pawnNameMaker;

		public TechLevel techLevel = TechLevel.Undefined;

		[NoTranslate]
		public string backstoryCategory = null;

		[NoTranslate]
		public List<string> hairTags = new List<string>();

		public ThingFilter apparelStuffFilter = null;

		public List<TraderKindDef> caravanTraderKinds = new List<TraderKindDef>();

		public List<TraderKindDef> visitorTraderKinds = new List<TraderKindDef>();

		public List<TraderKindDef> baseTraderKinds = new List<TraderKindDef>();

		public float geneticVariance = 1f;

		public IntRange startingGoodwill = IntRange.zero;

		public bool mustStartOneEnemy = false;

		public IntRange naturalColonyGoodwill = IntRange.zero;

		public float goodwillDailyGain = 0f;

		public float goodwillDailyFall = 0f;

		public bool permanentEnemy = false;

		[NoTranslate]
		public string homeIconPath;

		[NoTranslate]
		public string expandingIconTexture;

		public List<Color> colorSpectrum;

		[Unsaved]
		private Texture2D expandingIconTextureInt;

		[CompilerGenerated]
		private static Func<PawnGroupMaker, float> <>f__am$cache0;

		public FactionDef()
		{
		}

		public bool CanEverBeNonHostile
		{
			get
			{
				return !this.permanentEnemy;
			}
		}

		public Texture2D ExpandingIconTexture
		{
			get
			{
				if (this.expandingIconTextureInt == null)
				{
					if (!this.expandingIconTexture.NullOrEmpty())
					{
						this.expandingIconTextureInt = ContentFinder<Texture2D>.Get(this.expandingIconTexture, true);
					}
					else
					{
						this.expandingIconTextureInt = BaseContent.BadTex;
					}
				}
				return this.expandingIconTextureInt;
			}
		}

		public float MinPointsToGeneratePawnGroup(PawnGroupKindDef groupKind)
		{
			float result;
			if (this.pawnGroupMakers == null)
			{
				result = 0f;
			}
			else
			{
				IEnumerable<PawnGroupMaker> source = from x in this.pawnGroupMakers
				where x.kindDef == groupKind
				select x;
				if (!source.Any<PawnGroupMaker>())
				{
					result = 0f;
				}
				else
				{
					result = source.Min((PawnGroupMaker pgm) => pgm.MinPointsToGenerateAnything);
				}
			}
			return result;
		}

		public bool CanUseStuffForApparel(ThingDef stuffDef)
		{
			return this.apparelStuffFilter == null || this.apparelStuffFilter.Allows(stuffDef);
		}

		public float RaidCommonalityFromPoints(float points)
		{
			float result;
			if (points < 0f || this.raidCommonalityFromPointsCurve == null)
			{
				result = 1f;
			}
			else
			{
				result = this.raidCommonalityFromPointsCurve.Evaluate(points);
			}
			return result;
		}

		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.apparelStuffFilter != null)
			{
				this.apparelStuffFilter.ResolveReferences();
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string error in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return error;
			}
			if (this.pawnGroupMakers != null && this.maxPawnCostPerTotalPointsCurve == null)
			{
				yield return "has pawnGroupMakers but missing maxPawnCostPerTotalPointsCurve";
			}
			if (!this.isPlayer && this.factionNameMaker == null && this.fixedName == null)
			{
				yield return "FactionTypeDef " + this.defName + " lacks a factionNameMaker and a fixedName.";
			}
			if (this.techLevel == TechLevel.Undefined)
			{
				yield return this.defName + " has no tech level.";
			}
			if (this.humanlikeFaction)
			{
				if (this.backstoryCategory == null)
				{
					yield return this.defName + " is humanlikeFaction but has no backstory category.";
				}
				if (this.hairTags.Count == 0)
				{
					yield return this.defName + " is humanlikeFaction but has no hairTags.";
				}
			}
			if (this.isPlayer)
			{
				if (this.settlementNameMaker == null)
				{
					yield return "isPlayer is true but settlementNameMaker is null";
				}
				if (this.factionNameMaker == null)
				{
					yield return "isPlayer is true but factionNameMaker is null";
				}
				if (this.playerInitialSettlementNameMaker == null)
				{
					yield return "isPlayer is true but playerInitialSettlementNameMaker is null";
				}
			}
			if (this.permanentEnemy)
			{
				if (this.mustStartOneEnemy)
				{
					yield return "permanentEnemy has mustStartOneEnemy = true, which is redundant";
				}
				if (this.goodwillDailyFall != 0f || this.goodwillDailyGain != 0f)
				{
					yield return "permanentEnemy has a goodwillDailyFall or goodwillDailyGain";
				}
				if (this.startingGoodwill != IntRange.zero)
				{
					yield return "permanentEnemy has a startingGoodwill defined";
				}
				if (this.naturalColonyGoodwill != IntRange.zero)
				{
					yield return "permanentEnemy has a naturalColonyGoodwill defined";
				}
			}
			yield break;
		}

		public static FactionDef Named(string defName)
		{
			return DefDatabase<FactionDef>.GetNamed(defName, true);
		}

		[CompilerGenerated]
		private static float <MinPointsToGeneratePawnGroup>m__0(PawnGroupMaker pgm)
		{
			return pgm.MinPointsToGenerateAnything;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<string> <ConfigErrors>__BaseCallProxy0()
		{
			return base.ConfigErrors();
		}

		[CompilerGenerated]
		private sealed class <MinPointsToGeneratePawnGroup>c__AnonStorey1
		{
			internal PawnGroupKindDef groupKind;

			public <MinPointsToGeneratePawnGroup>c__AnonStorey1()
			{
			}

			internal bool <>m__0(PawnGroupMaker x)
			{
				return x.kindDef == this.groupKind;
			}
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal IEnumerator<string> $locvar0;

			internal string <error>__1;

			internal FactionDef $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator0()
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
					goto IL_121;
				case 3u:
					goto IL_185;
				case 4u:
					goto IL_1C4;
				case 5u:
					goto IL_214;
				case 6u:
					goto IL_258;
				case 7u:
					IL_299:
					if (this.factionNameMaker == null)
					{
						this.$current = "isPlayer is true but factionNameMaker is null";
						if (!this.$disposing)
						{
							this.$PC = 8;
						}
						return true;
					}
					goto IL_2C8;
				case 8u:
					goto IL_2C8;
				case 9u:
					goto IL_2F8;
				case 10u:
					IL_33A:
					if (this.goodwillDailyFall != 0f || this.goodwillDailyGain != 0f)
					{
						this.$current = "permanentEnemy has a goodwillDailyFall or goodwillDailyGain";
						if (!this.$disposing)
						{
							this.$PC = 11;
						}
						return true;
					}
					goto IL_384;
				case 11u:
					goto IL_384;
				case 12u:
					goto IL_3BE;
				case 13u:
					goto IL_3F8;
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
						error = enumerator.Current;
						this.$current = error;
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
				if (this.pawnGroupMakers != null && this.maxPawnCostPerTotalPointsCurve == null)
				{
					this.$current = "has pawnGroupMakers but missing maxPawnCostPerTotalPointsCurve";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_121:
				if (!this.isPlayer && this.factionNameMaker == null && this.fixedName == null)
				{
					this.$current = "FactionTypeDef " + this.defName + " lacks a factionNameMaker and a fixedName.";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_185:
				if (this.techLevel == TechLevel.Undefined)
				{
					this.$current = this.defName + " has no tech level.";
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_1C4:
				if (!this.humanlikeFaction)
				{
					goto IL_259;
				}
				if (this.backstoryCategory == null)
				{
					this.$current = this.defName + " is humanlikeFaction but has no backstory category.";
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				IL_214:
				if (this.hairTags.Count == 0)
				{
					this.$current = this.defName + " is humanlikeFaction but has no hairTags.";
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				}
				IL_258:
				IL_259:
				if (!this.isPlayer)
				{
					goto IL_2F9;
				}
				if (this.settlementNameMaker == null)
				{
					this.$current = "isPlayer is true but settlementNameMaker is null";
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				}
				goto IL_299;
				IL_2C8:
				if (this.playerInitialSettlementNameMaker == null)
				{
					this.$current = "isPlayer is true but playerInitialSettlementNameMaker is null";
					if (!this.$disposing)
					{
						this.$PC = 9;
					}
					return true;
				}
				IL_2F8:
				IL_2F9:
				if (!this.permanentEnemy)
				{
					goto IL_3F9;
				}
				if (this.mustStartOneEnemy)
				{
					this.$current = "permanentEnemy has mustStartOneEnemy = true, which is redundant";
					if (!this.$disposing)
					{
						this.$PC = 10;
					}
					return true;
				}
				goto IL_33A;
				IL_384:
				if (this.startingGoodwill != IntRange.zero)
				{
					this.$current = "permanentEnemy has a startingGoodwill defined";
					if (!this.$disposing)
					{
						this.$PC = 12;
					}
					return true;
				}
				IL_3BE:
				if (this.naturalColonyGoodwill != IntRange.zero)
				{
					this.$current = "permanentEnemy has a naturalColonyGoodwill defined";
					if (!this.$disposing)
					{
						this.$PC = 13;
					}
					return true;
				}
				IL_3F8:
				IL_3F9:
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
				FactionDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new FactionDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}
