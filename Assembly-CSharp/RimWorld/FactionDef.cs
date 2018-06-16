using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000293 RID: 659
	public class FactionDef : Def
	{
		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000B1F RID: 2847 RVA: 0x00064E9C File Offset: 0x0006329C
		public bool CanEverBeNonHostile
		{
			get
			{
				return !this.permanentEnemy;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000B20 RID: 2848 RVA: 0x00064EBC File Offset: 0x000632BC
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

		// Token: 0x06000B21 RID: 2849 RVA: 0x00064F1C File Offset: 0x0006331C
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

		// Token: 0x06000B22 RID: 2850 RVA: 0x00064FA8 File Offset: 0x000633A8
		public bool CanUseStuffForApparel(ThingDef stuffDef)
		{
			return this.apparelStuffFilter == null || this.apparelStuffFilter.Allows(stuffDef);
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00064FDC File Offset: 0x000633DC
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

		// Token: 0x06000B24 RID: 2852 RVA: 0x0006501E File Offset: 0x0006341E
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.apparelStuffFilter != null)
			{
				this.apparelStuffFilter.ResolveReferences();
			}
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00065040 File Offset: 0x00063440
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

		// Token: 0x06000B26 RID: 2854 RVA: 0x0006506C File Offset: 0x0006346C
		public static FactionDef Named(string defName)
		{
			return DefDatabase<FactionDef>.GetNamed(defName, true);
		}

		// Token: 0x040005AA RID: 1450
		public bool isPlayer = false;

		// Token: 0x040005AB RID: 1451
		public RulePackDef factionNameMaker;

		// Token: 0x040005AC RID: 1452
		public RulePackDef settlementNameMaker;

		// Token: 0x040005AD RID: 1453
		public RulePackDef playerInitialSettlementNameMaker;

		// Token: 0x040005AE RID: 1454
		[MustTranslate]
		public string fixedName = null;

		// Token: 0x040005AF RID: 1455
		public bool humanlikeFaction = true;

		// Token: 0x040005B0 RID: 1456
		public bool hidden = false;

		// Token: 0x040005B1 RID: 1457
		public float listOrderPriority = 0f;

		// Token: 0x040005B2 RID: 1458
		public List<PawnGroupMaker> pawnGroupMakers = null;

		// Token: 0x040005B3 RID: 1459
		public SimpleCurve raidCommonalityFromPointsCurve = null;

		// Token: 0x040005B4 RID: 1460
		public bool autoFlee = true;

		// Token: 0x040005B5 RID: 1461
		public bool canSiege = false;

		// Token: 0x040005B6 RID: 1462
		public bool canStageAttacks = false;

		// Token: 0x040005B7 RID: 1463
		public bool canUseAvoidGrid = true;

		// Token: 0x040005B8 RID: 1464
		public float earliestRaidDays = 0f;

		// Token: 0x040005B9 RID: 1465
		public FloatRange allowedArrivalTemperatureRange = new FloatRange(-1000f, 1000f);

		// Token: 0x040005BA RID: 1466
		public PawnKindDef basicMemberKind;

		// Token: 0x040005BB RID: 1467
		public List<ResearchProjectTagDef> startingResearchTags = null;

		// Token: 0x040005BC RID: 1468
		[NoTranslate]
		public List<string> recipePrerequisiteTags = null;

		// Token: 0x040005BD RID: 1469
		public bool rescueesCanJoin = false;

		// Token: 0x040005BE RID: 1470
		[MustTranslate]
		public string pawnSingular = "member";

		// Token: 0x040005BF RID: 1471
		[MustTranslate]
		public string pawnsPlural = "members";

		// Token: 0x040005C0 RID: 1472
		public string leaderTitle = "leader";

		// Token: 0x040005C1 RID: 1473
		public float forageabilityFactor = 1f;

		// Token: 0x040005C2 RID: 1474
		public SimpleCurve maxPawnCostPerTotalPointsCurve = null;

		// Token: 0x040005C3 RID: 1475
		public int requiredCountAtGameStart = 0;

		// Token: 0x040005C4 RID: 1476
		public int maxCountAtGameStart = 9999;

		// Token: 0x040005C5 RID: 1477
		public bool canMakeRandomly = false;

		// Token: 0x040005C6 RID: 1478
		public float settlementGenerationWeight = 0f;

		// Token: 0x040005C7 RID: 1479
		public RulePackDef pawnNameMaker;

		// Token: 0x040005C8 RID: 1480
		public TechLevel techLevel = TechLevel.Undefined;

		// Token: 0x040005C9 RID: 1481
		[NoTranslate]
		public string backstoryCategory = null;

		// Token: 0x040005CA RID: 1482
		[NoTranslate]
		public List<string> hairTags = new List<string>();

		// Token: 0x040005CB RID: 1483
		public ThingFilter apparelStuffFilter = null;

		// Token: 0x040005CC RID: 1484
		public List<TraderKindDef> caravanTraderKinds = new List<TraderKindDef>();

		// Token: 0x040005CD RID: 1485
		public List<TraderKindDef> visitorTraderKinds = new List<TraderKindDef>();

		// Token: 0x040005CE RID: 1486
		public List<TraderKindDef> baseTraderKinds = new List<TraderKindDef>();

		// Token: 0x040005CF RID: 1487
		public float geneticVariance = 1f;

		// Token: 0x040005D0 RID: 1488
		public IntRange startingGoodwill = IntRange.zero;

		// Token: 0x040005D1 RID: 1489
		public bool mustStartOneEnemy = false;

		// Token: 0x040005D2 RID: 1490
		public IntRange naturalColonyGoodwill = IntRange.zero;

		// Token: 0x040005D3 RID: 1491
		public float goodwillDailyGain = 0f;

		// Token: 0x040005D4 RID: 1492
		public float goodwillDailyFall = 0f;

		// Token: 0x040005D5 RID: 1493
		public bool permanentEnemy = false;

		// Token: 0x040005D6 RID: 1494
		[NoTranslate]
		public string homeIconPath;

		// Token: 0x040005D7 RID: 1495
		[NoTranslate]
		public string expandingIconTexture;

		// Token: 0x040005D8 RID: 1496
		public List<Color> colorSpectrum;

		// Token: 0x040005D9 RID: 1497
		[Unsaved]
		private Texture2D expandingIconTextureInt;
	}
}
