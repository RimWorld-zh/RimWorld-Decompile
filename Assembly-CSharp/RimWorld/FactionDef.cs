using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class FactionDef : Def
	{
		public bool isPlayer = false;

		public RulePackDef factionNameMaker;

		public RulePackDef baseNameMaker;

		public RulePackDef factionNameMakerPlayer;

		public RulePackDef baseNameMakerPlayer;

		public string fixedName = (string)null;

		public bool humanlikeFaction = true;

		public bool hidden = false;

		public List<PawnGroupMaker> pawnGroupMakers = null;

		public SimpleCurve raidCommonalityFromPointsCurve = null;

		public bool autoFlee = true;

		public bool canSiege = false;

		public bool canStageAttacks = false;

		public bool canUseAvoidGrid = true;

		public float earliestRaidDays = 0f;

		public FloatRange allowedArrivalTemperatureRange = new FloatRange(-1000f, 1000f);

		public PawnKindDef basicMemberKind;

		[NoTranslate]
		public List<string> startingResearchTags = null;

		[NoTranslate]
		public List<string> recipePrerequisiteTags = null;

		public bool rescueesCanJoin = false;

		[MustTranslate]
		public string pawnsPlural = "members";

		public string leaderTitle = "leader";

		public float maxPawnOptionCostFactor = 1f;

		public int requiredCountAtGameStart = 0;

		public int maxCountAtGameStart = 9999;

		public bool canMakeRandomly = false;

		public float baseSelectionWeight = 0f;

		public RulePackDef pawnNameMaker;

		public TechLevel techLevel = TechLevel.Undefined;

		public string backstoryCategory = (string)null;

		public List<string> hairTags = new List<string>();

		public ThingFilter apparelStuffFilter = null;

		public List<TraderKindDef> caravanTraderKinds = new List<TraderKindDef>();

		public List<TraderKindDef> visitorTraderKinds = new List<TraderKindDef>();

		public List<TraderKindDef> baseTraderKinds = new List<TraderKindDef>();

		public float geneticVariance = 1f;

		public FloatRange startingGoodwill = FloatRange.Zero;

		public bool mustStartOneEnemy = false;

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
				return (byte)((!(this.startingGoodwill.max < 0.0) || this.appreciative) ? 1 : 0) != 0;
			}
		}

		public Texture2D ExpandingIconTexture
		{
			get
			{
				if ((UnityEngine.Object)this.expandingIconTextureInt == (UnityEngine.Object)null)
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
			float result;
			if (this.pawnGroupMakers == null)
			{
				result = 9999999f;
			}
			else
			{
				IEnumerable<PawnGroupMaker> source = from x in this.pawnGroupMakers
				where x.kindDef == PawnGroupKindDefOf.Normal
				select x;
				result = (float)(source.Any() ? source.Min((Func<PawnGroupMaker, float>)((PawnGroupMaker pgm) => pgm.MinPointsToGenerateAnything)) : 9999999.0);
			}
			return result;
		}

		public bool CanUseStuffForApparel(ThingDef stuffDef)
		{
			return this.apparelStuffFilter == null || this.apparelStuffFilter.Allows(stuffDef);
		}

		public float RaidCommonalityFromPoints(float points)
		{
			return (float)((!(points < 0.0) && this.raidCommonalityFromPointsCurve != null) ? this.raidCommonalityFromPointsCurve.Evaluate(points) : 1.0);
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
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
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
			IL_027b:
			/*Error near IL_027c: Unexpected return in MoveNext()*/;
		}

		public static FactionDef Named(string defName)
		{
			return DefDatabase<FactionDef>.GetNamed(defName, true);
		}
	}
}
