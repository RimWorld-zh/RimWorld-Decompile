using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class FactionDef : Def
	{
		public bool isPlayer;

		public RulePackDef factionNameMaker;

		public RulePackDef baseNameMaker;

		public RulePackDef factionNameMakerPlayer;

		public RulePackDef baseNameMakerPlayer;

		public string fixedName;

		public bool humanlikeFaction = true;

		public bool hidden;

		public List<PawnGroupMaker> pawnGroupMakers;

		public SimpleCurve raidCommonalityFromPointsCurve;

		public bool autoFlee = true;

		public bool canSiege;

		public bool canStageAttacks;

		public bool canUseAvoidGrid = true;

		public float earliestRaidDays;

		public FloatRange allowedArrivalTemperatureRange = new FloatRange(-1000f, 1000f);

		public PawnKindDef basicMemberKind;

		[NoTranslate]
		public List<string> startingResearchTags;

		[NoTranslate]
		public List<string> recipePrerequisiteTags;

		public bool rescueesCanJoin;

		[MustTranslate]
		public string pawnsPlural = "members";

		public string leaderTitle = "leader";

		public float maxPawnOptionCostFactor = 1f;

		public int requiredCountAtGameStart;

		public int maxCountAtGameStart = 9999;

		public bool canMakeRandomly;

		public float baseSelectionWeight;

		public RulePackDef pawnNameMaker;

		public TechLevel techLevel;

		public string backstoryCategory;

		public List<string> hairTags = new List<string>();

		public ThingFilter apparelStuffFilter;

		public List<TraderKindDef> caravanTraderKinds = new List<TraderKindDef>();

		public List<TraderKindDef> visitorTraderKinds = new List<TraderKindDef>();

		public List<TraderKindDef> baseTraderKinds = new List<TraderKindDef>();

		public float geneticVariance = 1f;

		public FloatRange startingGoodwill = FloatRange.Zero;

		public bool mustStartOneEnemy;

		public FloatRange naturalColonyGoodwill = FloatRange.Zero;

		public float goodwillDailyGain = 2f;

		public float goodwillDailyFall = 2f;

		public bool appreciative = true;

		public string homeIconPath;

		public string expandingIconTexture;

		public List<Color> colorSpectrum;

		[Unsaved]
		private Texture2D expandingIconTextureInt;

		public bool CanEverBeNonHostile
		{
			get
			{
				if (this.startingGoodwill.max < 0.0 && !this.appreciative)
				{
					return false;
				}
				return true;
			}
		}

		public Texture2D ExpandingIconTexture
		{
			get
			{
				if ((Object)this.expandingIconTextureInt == (Object)null)
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

		public float MinPointsToGenerateNormalPawnGroup()
		{
			if (this.pawnGroupMakers == null)
			{
				return 9999999f;
			}
			IEnumerable<PawnGroupMaker> source = from x in this.pawnGroupMakers
			where x.kindDef == PawnGroupKindDefOf.Normal
			select x;
			if (!source.Any())
			{
				return 9999999f;
			}
			return source.Min((PawnGroupMaker pgm) => pgm.MinPointsToGenerateAnything);
		}

		public bool CanUseStuffForApparel(ThingDef stuffDef)
		{
			if (this.apparelStuffFilter == null)
			{
				return true;
			}
			return this.apparelStuffFilter.Allows(stuffDef);
		}

		public float RaidCommonalityFromPoints(float points)
		{
			if (!(points < 0.0) && this.raidCommonalityFromPointsCurve != null)
			{
				return this.raidCommonalityFromPointsCurve.Evaluate(points);
			}
			return 1f;
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
			using (IEnumerator<string> enumerator = base.ConfigErrors().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string error = enumerator.Current;
					yield return error;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (!this.isPlayer && this.factionNameMaker == null && this.fixedName == null)
			{
				yield return "FactionTypeDef " + base.defName + " lacks a factionNameMaker and a fixedName.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.techLevel == TechLevel.Undefined)
			{
				yield return base.defName + " has no tech level.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.humanlikeFaction)
			{
				if (this.backstoryCategory == null)
				{
					yield return base.defName + " is humanlikeFaction but has no backstory category.";
					/*Error: Unable to find new state assignment for yield return*/;
				}
				if (this.hairTags.Count == 0)
				{
					yield return base.defName + " is humanlikeFaction but has no hairTags.";
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (!this.isPlayer)
				yield break;
			if (this.baseNameMakerPlayer == null)
			{
				yield return "isPlayer is true but baseNameMakerPlayer is null";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.factionNameMakerPlayer != null)
				yield break;
			yield return "isPlayer is true but factionNameMakerPlayer is null";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0275:
			/*Error near IL_0276: Unexpected return in MoveNext()*/;
		}

		public static FactionDef Named(string defName)
		{
			return DefDatabase<FactionDef>.GetNamed(defName, true);
		}
	}
}
