using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Verse
{
	public class RaceProperties
	{
		public Intelligence intelligence;

		private FleshTypeDef fleshType;

		private ThingDef bloodDef;

		public bool hasGenders = true;

		public bool needsRest = true;

		public ThinkTreeDef thinkTreeMain;

		public ThinkTreeDef thinkTreeConstant;

		public PawnNameCategory nameCategory;

		public FoodTypeFlags foodType;

		public BodyDef body;

		public Type deathActionWorkerClass;

		public List<AnimalBiomeRecord> wildBiomes;

		public SimpleCurve ageGenerationCurve;

		public bool makesFootprints;

		public int executionRange = 2;

		public float lifeExpectancy = 10f;

		public List<HediffGiverSetDef> hediffGiverSets;

		public float petness;

		public bool packAnimal;

		public bool herdAnimal;

		public bool predator;

		public float maxPreyBodySize = 99999f;

		public float wildness = -1f;

		public float nuzzleMtbHours = -1f;

		public float manhunterOnDamageChance;

		public float manhunterOnTameFailChance;

		public bool canBePredatorPrey = true;

		public float gestationPeriodDays = 10f;

		public SimpleCurve litterSizeCurve;

		public float mateMtbHours = 12f;

		public List<string> untrainableTags;

		public List<string> trainableTags;

		private TrainableIntelligenceDef trainableIntelligence;

		private RulePackDef nameGenerator;

		private RulePackDef nameGeneratorFemale;

		public float nameOnTameChance;

		public float nameOnNuzzleChance;

		public float baseBodySize = 1f;

		public float baseHealthScale = 1f;

		public float baseHungerRate = 1f;

		public List<LifeStageAge> lifeStageAges = new List<LifeStageAge>();

		public Color leatherColor = ColorLibrary.Leather;

		public string leatherLabel;

		public float leatherCommonalityFactor = 1f;

		public float leatherInsulation = 1.1f;

		public List<StatModifier> leatherStatFactors;

		public float leatherMarketValueFactor = 1f;

		public string meatLabel;

		public Color meatColor;

		public ThingDef useMeatFrom;

		public ThingDef useLeatherFrom;

		public ShadowData specialShadowData;

		public IntRange soundCallIntervalRange;

		public SoundDef soundMeleeHitPawn;

		public SoundDef soundMeleeHitBuilding;

		public SoundDef soundMeleeMiss;

		[Unsaved]
		private DeathActionWorker deathActionWorkerInt;

		[Unsaved]
		public ThingDef meatDef;

		[Unsaved]
		public ThingDef leatherDef;

		[Unsaved]
		public ThingDef corpseDef;

		public bool Humanlike
		{
			get
			{
				return this.intelligence >= Intelligence.Humanlike;
			}
		}

		public bool ToolUser
		{
			get
			{
				return this.intelligence >= Intelligence.ToolUser;
			}
		}

		public bool Animal
		{
			get
			{
				return !this.ToolUser && this.IsFlesh;
			}
		}

		public bool EatsFood
		{
			get
			{
				return this.foodType != FoodTypeFlags.None;
			}
		}

		public float FoodLevelPercentageWantEat
		{
			get
			{
				switch (this.ResolvedDietCategory)
				{
				case DietCategory.NeverEats:
					return 0.3f;
				case DietCategory.Herbivorous:
					return 0.45f;
				case DietCategory.Dendrovorous:
					return 0.45f;
				case DietCategory.Ovivorous:
					return 0.4f;
				case DietCategory.Omnivorous:
					return 0.3f;
				case DietCategory.Carnivorous:
					return 0.3f;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		public DietCategory ResolvedDietCategory
		{
			get
			{
				if (!this.EatsFood)
				{
					return DietCategory.NeverEats;
				}
				if (this.Eats(FoodTypeFlags.Tree))
				{
					return DietCategory.Dendrovorous;
				}
				if (this.Eats(FoodTypeFlags.Meat))
				{
					if (this.Eats(FoodTypeFlags.VegetableOrFruit) || this.Eats(FoodTypeFlags.Plant))
					{
						return DietCategory.Omnivorous;
					}
					return DietCategory.Carnivorous;
				}
				else
				{
					if (this.Eats(FoodTypeFlags.AnimalProduct))
					{
						return DietCategory.Ovivorous;
					}
					return DietCategory.Herbivorous;
				}
			}
		}

		public DeathActionWorker DeathActionWorker
		{
			get
			{
				if (this.deathActionWorkerInt == null && this.deathActionWorkerClass != null)
				{
					this.deathActionWorkerInt = (DeathActionWorker)Activator.CreateInstance(this.deathActionWorkerClass);
				}
				return this.deathActionWorkerInt;
			}
		}

		public FleshTypeDef FleshType
		{
			get
			{
				if (this.fleshType != null)
				{
					return this.fleshType;
				}
				return FleshTypeDefOf.Normal;
			}
		}

		public bool IsMechanoid
		{
			get
			{
				return this.FleshType == FleshTypeDefOf.Mechanoid;
			}
		}

		public bool IsFlesh
		{
			get
			{
				return this.FleshType != FleshTypeDefOf.Mechanoid;
			}
		}

		public ThingDef BloodDef
		{
			get
			{
				if (this.bloodDef != null)
				{
					return this.bloodDef;
				}
				if (this.IsFlesh)
				{
					return ThingDefOf.FilthBlood;
				}
				return null;
			}
		}

		public TrainableIntelligenceDef TrainableIntelligence
		{
			get
			{
				if (this.trainableIntelligence == null)
				{
					return TrainableIntelligenceDefOf.Intermediate;
				}
				return this.trainableIntelligence;
			}
		}

		public RaceProperties()
		{
			ColorInt colorInt = new ColorInt(141, 56, 52);
			this.meatColor = colorInt.ToColor;
			this.soundCallIntervalRange = new IntRange(2000, 4000);
			base..ctor();
		}

		public RulePackDef GetNameGenerator(Gender gender)
		{
			if (gender == Gender.Female && this.nameGeneratorFemale != null)
			{
				return this.nameGeneratorFemale;
			}
			return this.nameGenerator;
		}

		public bool WillAutomaticallyEat(Thing t)
		{
			return t.def.ingestible != null && this.CanEverEat(t);
		}

		public bool CanEverEat(Thing t)
		{
			return this.CanEverEat(t.def);
		}

		public bool CanEverEat(ThingDef t)
		{
			return this.EatsFood && t.ingestible != null && t.ingestible.preferability != FoodPreferability.Undefined && this.Eats(t.ingestible.foodType);
		}

		public bool Eats(FoodTypeFlags food)
		{
			return this.EatsFood && (this.foodType & food) != FoodTypeFlags.None;
		}

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

		[DebuggerHidden]
		public IEnumerable<string> ConfigErrors()
		{
			RaceProperties.<ConfigErrors>c__Iterator1C9 <ConfigErrors>c__Iterator1C = new RaceProperties.<ConfigErrors>c__Iterator1C9();
			<ConfigErrors>c__Iterator1C.<>f__this = this;
			RaceProperties.<ConfigErrors>c__Iterator1C9 expr_0E = <ConfigErrors>c__Iterator1C;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		[DebuggerHidden]
		internal IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			RaceProperties.<SpecialDisplayStats>c__Iterator1CA <SpecialDisplayStats>c__Iterator1CA = new RaceProperties.<SpecialDisplayStats>c__Iterator1CA();
			<SpecialDisplayStats>c__Iterator1CA.parentDef = parentDef;
			<SpecialDisplayStats>c__Iterator1CA.<$>parentDef = parentDef;
			<SpecialDisplayStats>c__Iterator1CA.<>f__this = this;
			RaceProperties.<SpecialDisplayStats>c__Iterator1CA expr_1C = <SpecialDisplayStats>c__Iterator1CA;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
