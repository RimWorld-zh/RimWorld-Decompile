using System;
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

		public string fixedName;

		public bool humanlikeFaction = true;

		public bool hidden;

		public List<PawnGroupMaker> pawnGroupMakers;

		public float raidCommonality;

		public bool autoFlee = true;

		public bool canSiege;

		public bool canStageAttacks;

		public bool canUseAvoidGrid = true;

		public float earliestRaidDays;

		public FloatRange allowedArrivalTemperatureRange = new FloatRange(-1000f, 1000f);

		public PawnKindDef basicMemberKind;

		public List<string> startingResearchTags;

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
			if (this.pawnGroupMakers == null)
			{
				return 2.14748365E+09f;
			}
			IEnumerable<PawnGroupMaker> source = from x in this.pawnGroupMakers
			where x.kindDef == PawnGroupKindDefOf.Normal
			select x;
			if (!source.Any())
			{
				return 2.14748365E+09f;
			}
			return source.Min((Func<PawnGroupMaker, float>)((PawnGroupMaker pgm) => pgm.MinPointsToGenerateAnything));
		}

		public bool CanUseStuffForApparel(ThingDef stuffDef)
		{
			if (this.apparelStuffFilter == null)
			{
				return true;
			}
			return this.apparelStuffFilter.Allows(stuffDef);
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
			foreach (string item in base.ConfigErrors())
			{
				yield return item;
			}
			if (!this.isPlayer && this.factionNameMaker == null && this.fixedName == null)
			{
				yield return "FactionTypeDef " + base.defName + " lacks a factionNameMaker and a fixedName.";
			}
			if (this.techLevel == TechLevel.Undefined)
			{
				yield return base.defName + " has no tech level.";
			}
			if (this.humanlikeFaction)
			{
				if (this.backstoryCategory == null)
				{
					yield return base.defName + " is humanlikeFaction but has no backstory category.";
				}
				if (this.hairTags.Count == 0)
				{
					yield return base.defName + " is humanlikeFaction but has no hairTags.";
				}
			}
		}

		public static FactionDef Named(string defName)
		{
			return DefDatabase<FactionDef>.GetNamed(defName, true);
		}
	}
}
