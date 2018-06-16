using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B26 RID: 2854
	public class RaceProperties
	{
		// Token: 0x06003ED0 RID: 16080 RVA: 0x00210FA4 File Offset: 0x0020F3A4
		public RaceProperties()
		{
			ColorInt colorInt = new ColorInt(141, 56, 52);
			this.meatColor = colorInt.ToColor;
			this.useMeatFrom = null;
			this.useLeatherFrom = null;
			this.specialShadowData = null;
			this.soundCallIntervalRange = new IntRange(2000, 4000);
			this.soundMeleeHitPawn = null;
			this.soundMeleeHitBuilding = null;
			this.soundMeleeMiss = null;
			this.deathActionWorkerInt = null;
			this.meatDef = null;
			this.leatherDef = null;
			this.corpseDef = null;
			base..ctor();
		}

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x06003ED1 RID: 16081 RVA: 0x00211178 File Offset: 0x0020F578
		public bool Humanlike
		{
			get
			{
				return this.intelligence >= Intelligence.Humanlike;
			}
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06003ED2 RID: 16082 RVA: 0x0021119C File Offset: 0x0020F59C
		public bool ToolUser
		{
			get
			{
				return this.intelligence >= Intelligence.ToolUser;
			}
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06003ED3 RID: 16083 RVA: 0x002111C0 File Offset: 0x0020F5C0
		public bool Animal
		{
			get
			{
				return !this.ToolUser && this.IsFlesh;
			}
		}

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06003ED4 RID: 16084 RVA: 0x002111EC File Offset: 0x0020F5EC
		public bool EatsFood
		{
			get
			{
				return this.foodType != FoodTypeFlags.None;
			}
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06003ED5 RID: 16085 RVA: 0x00211210 File Offset: 0x0020F610
		public float FoodLevelPercentageWantEat
		{
			get
			{
				float result;
				switch (this.ResolvedDietCategory)
				{
				case DietCategory.NeverEats:
					result = 0.3f;
					break;
				case DietCategory.Herbivorous:
					result = 0.45f;
					break;
				case DietCategory.Dendrovorous:
					result = 0.45f;
					break;
				case DietCategory.Ovivorous:
					result = 0.4f;
					break;
				case DietCategory.Omnivorous:
					result = 0.3f;
					break;
				case DietCategory.Carnivorous:
					result = 0.3f;
					break;
				default:
					throw new InvalidOperationException();
				}
				return result;
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x06003ED6 RID: 16086 RVA: 0x00211294 File Offset: 0x0020F694
		public DietCategory ResolvedDietCategory
		{
			get
			{
				DietCategory result;
				if (!this.EatsFood)
				{
					result = DietCategory.NeverEats;
				}
				else if (this.Eats(FoodTypeFlags.Tree))
				{
					result = DietCategory.Dendrovorous;
				}
				else if (this.Eats(FoodTypeFlags.Meat))
				{
					if (this.Eats(FoodTypeFlags.VegetableOrFruit) || this.Eats(FoodTypeFlags.Plant))
					{
						result = DietCategory.Omnivorous;
					}
					else
					{
						result = DietCategory.Carnivorous;
					}
				}
				else if (this.Eats(FoodTypeFlags.AnimalProduct))
				{
					result = DietCategory.Ovivorous;
				}
				else
				{
					result = DietCategory.Herbivorous;
				}
				return result;
			}
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x06003ED7 RID: 16087 RVA: 0x0021131C File Offset: 0x0020F71C
		public DeathActionWorker DeathActionWorker
		{
			get
			{
				if (this.deathActionWorkerInt == null)
				{
					if (this.deathActionWorkerClass != null)
					{
						this.deathActionWorkerInt = (DeathActionWorker)Activator.CreateInstance(this.deathActionWorkerClass);
					}
					else
					{
						this.deathActionWorkerInt = new DeathActionWorker_Simple();
					}
				}
				return this.deathActionWorkerInt;
			}
		}

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x06003ED8 RID: 16088 RVA: 0x00211378 File Offset: 0x0020F778
		public FleshTypeDef FleshType
		{
			get
			{
				FleshTypeDef normal;
				if (this.fleshType != null)
				{
					normal = this.fleshType;
				}
				else
				{
					normal = FleshTypeDefOf.Normal;
				}
				return normal;
			}
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x06003ED9 RID: 16089 RVA: 0x002113AC File Offset: 0x0020F7AC
		public bool IsMechanoid
		{
			get
			{
				return this.FleshType == FleshTypeDefOf.Mechanoid;
			}
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06003EDA RID: 16090 RVA: 0x002113D0 File Offset: 0x0020F7D0
		public bool IsFlesh
		{
			get
			{
				return this.FleshType != FleshTypeDefOf.Mechanoid;
			}
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06003EDB RID: 16091 RVA: 0x002113F8 File Offset: 0x0020F7F8
		public ThingDef BloodDef
		{
			get
			{
				ThingDef result;
				if (this.bloodDef != null)
				{
					result = this.bloodDef;
				}
				else if (this.IsFlesh)
				{
					result = ThingDefOf.Filth_Blood;
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x06003EDC RID: 16092 RVA: 0x0021143C File Offset: 0x0020F83C
		public bool CanDoHerdMigration
		{
			get
			{
				return this.Animal && this.herdMigrationAllowed;
			}
		}

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06003EDD RID: 16093 RVA: 0x00211468 File Offset: 0x0020F868
		public PawnKindDef AnyPawnKind
		{
			get
			{
				if (this.cachedAnyPawnKind == null)
				{
					List<PawnKindDef> allDefsListForReading = DefDatabase<PawnKindDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].race.race == this)
						{
							this.cachedAnyPawnKind = allDefsListForReading[i];
							break;
						}
					}
				}
				return this.cachedAnyPawnKind;
			}
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x002114DC File Offset: 0x0020F8DC
		public RulePackDef GetNameGenerator(Gender gender)
		{
			RulePackDef result;
			if (gender == Gender.Female && this.nameGeneratorFemale != null)
			{
				result = this.nameGeneratorFemale;
			}
			else
			{
				result = this.nameGenerator;
			}
			return result;
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x00211518 File Offset: 0x0020F918
		public bool WillAutomaticallyEat(Thing t)
		{
			return t.def.ingestible != null && this.CanEverEat(t);
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x00211558 File Offset: 0x0020F958
		public bool CanEverEat(Thing t)
		{
			return this.CanEverEat(t.def);
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x0021157C File Offset: 0x0020F97C
		public bool CanEverEat(ThingDef t)
		{
			return this.EatsFood && t.ingestible != null && t.ingestible.preferability != FoodPreferability.Undefined && this.Eats(t.ingestible.foodType);
		}

		// Token: 0x06003EE2 RID: 16098 RVA: 0x002115E0 File Offset: 0x0020F9E0
		public bool Eats(FoodTypeFlags food)
		{
			return this.EatsFood && (this.foodType & food) != FoodTypeFlags.None;
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x00211618 File Offset: 0x0020FA18
		public void ResolveReferencesSpecial()
		{
			if (this.useMeatFrom != null)
			{
				this.meatDef = this.useMeatFrom.race.meatDef;
			}
			if (this.useLeatherFrom != null)
			{
				this.leatherDef = this.useLeatherFrom.race.leatherDef;
			}
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x00211668 File Offset: 0x0020FA68
		public IEnumerable<string> ConfigErrors()
		{
			if (this.soundMeleeHitPawn == null)
			{
				yield return "soundMeleeHitPawn is null";
			}
			if (this.soundMeleeHitBuilding == null)
			{
				yield return "soundMeleeHitBuilding is null";
			}
			if (this.soundMeleeMiss == null)
			{
				yield return "soundMeleeMiss is null";
			}
			if (this.predator && !this.Eats(FoodTypeFlags.Meat))
			{
				yield return "predator but doesn't eat meat";
			}
			for (int i = 0; i < this.lifeStageAges.Count; i++)
			{
				for (int j = 0; j < i; j++)
				{
					if (this.lifeStageAges[j].minAge > this.lifeStageAges[i].minAge)
					{
						yield return "lifeStages minAges are not in ascending order";
					}
				}
			}
			if (this.litterSizeCurve != null)
			{
				foreach (string e in this.litterSizeCurve.ConfigErrors("litterSizeCurve"))
				{
					yield return e;
				}
			}
			if (this.nameOnTameChance > 0f && this.nameGenerator == null)
			{
				yield return "can be named, but has no nameGenerator";
			}
			if (this.Animal && this.wildness < 0f)
			{
				yield return "is animal but wildness is not defined";
			}
			if (this.useMeatFrom != null && this.useMeatFrom.category != ThingCategory.Pawn)
			{
				yield return "tries to use meat from non-pawn " + this.useMeatFrom;
			}
			if (this.useMeatFrom != null && this.useMeatFrom.race.useMeatFrom != null)
			{
				yield return string.Concat(new object[]
				{
					"tries to use meat from ",
					this.useMeatFrom,
					" which uses meat from ",
					this.useMeatFrom.race.useMeatFrom
				});
			}
			if (this.useLeatherFrom != null && this.useLeatherFrom.category != ThingCategory.Pawn)
			{
				yield return "tries to use leather from non-pawn " + this.useLeatherFrom;
			}
			if (this.useLeatherFrom != null && this.useLeatherFrom.race.useLeatherFrom != null)
			{
				yield return string.Concat(new object[]
				{
					"tries to use leather from ",
					this.useLeatherFrom,
					" which uses leather from ",
					this.useLeatherFrom.race.useLeatherFrom
				});
			}
			if (this.Animal && this.trainability == null)
			{
				yield return "animal has trainability = null";
			}
			yield break;
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x00211694 File Offset: 0x0020FA94
		public IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Race".Translate(), parentDef.LabelCap, 2000, "");
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Diet".Translate(), this.foodType.ToHumanString().CapitalizeFirst(), 0, "");
			if (parentDef.race.leatherDef != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "LeatherType".Translate(), parentDef.race.leatherDef.LabelCap, 0, "");
			}
			if (parentDef.race.Animal || this.wildness > 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Wildness".Translate(), this.wildness.ToStringPercent(), 0, "")
				{
					overrideReportText = TrainableUtility.GetWildnessExplanation(parentDef)
				};
			}
			if (PawnUtility.GetManhunterOnDamageChance(parentDef.race) > 0f || parentDef.race.manhunterOnTameFailChance > 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "HarmedRevengeChance".Translate(), PawnUtility.GetManhunterOnDamageChance(parentDef.race).ToStringPercent(), 0, "")
				{
					overrideReportText = "HarmedRevengeChanceExplanation".Translate()
				};
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "TameFailedRevengeChance".Translate(), parentDef.race.manhunterOnTameFailChance.ToStringPercent(), 0, "");
			}
			if (this.intelligence < Intelligence.Humanlike && this.trainability != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Trainability".Translate(), this.trainability.LabelCap, 0, "");
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "StatsReport_LifeExpectancy".Translate(), this.lifeExpectancy.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Absolute), 0, "");
			if (this.intelligence < Intelligence.Humanlike)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "AnimalFilthRate".Translate(), (PawnUtility.AnimalFilthChancePerCell(parentDef, parentDef.race.baseBodySize) * 1000f).ToString("F2"), 0, "")
				{
					overrideReportText = "AnimalFilthRateExplanation".Translate(new object[]
					{
						1000.ToString()
					})
				};
			}
			if (this.packAnimal)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "PackAnimal".Translate(), "Yes".Translate(), 0, "")
				{
					overrideReportText = "PackAnimalExplanation".Translate()
				};
			}
			if (parentDef.race.nuzzleMtbHours > 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.PawnSocial, "NuzzleInterval".Translate(), Mathf.RoundToInt(parentDef.race.nuzzleMtbHours * 2500f).ToStringTicksToPeriod(), 0, "")
				{
					overrideReportText = "NuzzleIntervalExplanation".Translate()
				};
			}
			yield break;
		}

		// Token: 0x04002881 RID: 10369
		public Intelligence intelligence = Intelligence.Animal;

		// Token: 0x04002882 RID: 10370
		private FleshTypeDef fleshType = null;

		// Token: 0x04002883 RID: 10371
		private ThingDef bloodDef = null;

		// Token: 0x04002884 RID: 10372
		public bool hasGenders = true;

		// Token: 0x04002885 RID: 10373
		public bool needsRest = true;

		// Token: 0x04002886 RID: 10374
		public ThinkTreeDef thinkTreeMain;

		// Token: 0x04002887 RID: 10375
		public ThinkTreeDef thinkTreeConstant;

		// Token: 0x04002888 RID: 10376
		public PawnNameCategory nameCategory = PawnNameCategory.NoName;

		// Token: 0x04002889 RID: 10377
		public FoodTypeFlags foodType = FoodTypeFlags.None;

		// Token: 0x0400288A RID: 10378
		public BodyDef body = null;

		// Token: 0x0400288B RID: 10379
		public Type deathActionWorkerClass;

		// Token: 0x0400288C RID: 10380
		public List<AnimalBiomeRecord> wildBiomes = null;

		// Token: 0x0400288D RID: 10381
		public SimpleCurve ageGenerationCurve = null;

		// Token: 0x0400288E RID: 10382
		public bool makesFootprints = false;

		// Token: 0x0400288F RID: 10383
		public int executionRange = 2;

		// Token: 0x04002890 RID: 10384
		public float lifeExpectancy = 10f;

		// Token: 0x04002891 RID: 10385
		public List<HediffGiverSetDef> hediffGiverSets = null;

		// Token: 0x04002892 RID: 10386
		public bool herdAnimal = false;

		// Token: 0x04002893 RID: 10387
		public bool packAnimal = false;

		// Token: 0x04002894 RID: 10388
		public bool predator = false;

		// Token: 0x04002895 RID: 10389
		public float maxPreyBodySize = 99999f;

		// Token: 0x04002896 RID: 10390
		public float wildness = 0f;

		// Token: 0x04002897 RID: 10391
		public float petness = 0f;

		// Token: 0x04002898 RID: 10392
		public float nuzzleMtbHours = -1f;

		// Token: 0x04002899 RID: 10393
		public float manhunterOnDamageChance = 0f;

		// Token: 0x0400289A RID: 10394
		public float manhunterOnTameFailChance = 0f;

		// Token: 0x0400289B RID: 10395
		public bool canBePredatorPrey = true;

		// Token: 0x0400289C RID: 10396
		public bool herdMigrationAllowed = true;

		// Token: 0x0400289D RID: 10397
		public float gestationPeriodDays = 10f;

		// Token: 0x0400289E RID: 10398
		public SimpleCurve litterSizeCurve = null;

		// Token: 0x0400289F RID: 10399
		public float mateMtbHours = 12f;

		// Token: 0x040028A0 RID: 10400
		[NoTranslate]
		public List<string> untrainableTags = null;

		// Token: 0x040028A1 RID: 10401
		[NoTranslate]
		public List<string> trainableTags = null;

		// Token: 0x040028A2 RID: 10402
		public TrainabilityDef trainability = null;

		// Token: 0x040028A3 RID: 10403
		private RulePackDef nameGenerator;

		// Token: 0x040028A4 RID: 10404
		private RulePackDef nameGeneratorFemale;

		// Token: 0x040028A5 RID: 10405
		public float nameOnTameChance = 0f;

		// Token: 0x040028A6 RID: 10406
		public float nameOnNuzzleChance = 0f;

		// Token: 0x040028A7 RID: 10407
		public float baseBodySize = 1f;

		// Token: 0x040028A8 RID: 10408
		public float baseHealthScale = 1f;

		// Token: 0x040028A9 RID: 10409
		public float baseHungerRate = 1f;

		// Token: 0x040028AA RID: 10410
		public List<LifeStageAge> lifeStageAges = new List<LifeStageAge>();

		// Token: 0x040028AB RID: 10411
		[MustTranslate]
		public string meatLabel = null;

		// Token: 0x040028AC RID: 10412
		public Color meatColor;

		// Token: 0x040028AD RID: 10413
		public ThingDef useMeatFrom;

		// Token: 0x040028AE RID: 10414
		public ThingDef useLeatherFrom;

		// Token: 0x040028AF RID: 10415
		public ShadowData specialShadowData;

		// Token: 0x040028B0 RID: 10416
		public IntRange soundCallIntervalRange;

		// Token: 0x040028B1 RID: 10417
		public SoundDef soundMeleeHitPawn;

		// Token: 0x040028B2 RID: 10418
		public SoundDef soundMeleeHitBuilding;

		// Token: 0x040028B3 RID: 10419
		public SoundDef soundMeleeMiss;

		// Token: 0x040028B4 RID: 10420
		[Unsaved]
		private DeathActionWorker deathActionWorkerInt;

		// Token: 0x040028B5 RID: 10421
		[Unsaved]
		public ThingDef meatDef;

		// Token: 0x040028B6 RID: 10422
		[Unsaved]
		public ThingDef leatherDef;

		// Token: 0x040028B7 RID: 10423
		[Unsaved]
		public ThingDef corpseDef;

		// Token: 0x040028B8 RID: 10424
		[Unsaved]
		private PawnKindDef cachedAnyPawnKind;
	}
}
