using RimWorld;
using System;
using System.Collections.Generic;
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

		public bool herdMigrationAllowed = true;

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

		public Color meatColor = new ColorInt(141, 56, 52).ToColor;

		public ThingDef useMeatFrom;

		public ThingDef useLeatherFrom;

		public ShadowData specialShadowData;

		public IntRange soundCallIntervalRange = new IntRange(2000, 4000);

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
				return (int)this.intelligence >= 2;
			}
		}

		public bool ToolUser
		{
			get
			{
				return (int)this.intelligence >= 1;
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
				case DietCategory.Omnivorous:
					return 0.3f;
				case DietCategory.Carnivorous:
					return 0.3f;
				case DietCategory.Ovivorous:
					return 0.4f;
				case DietCategory.Herbivorous:
					return 0.45f;
				case DietCategory.Dendrovorous:
					return 0.45f;
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
					if (!this.Eats(FoodTypeFlags.VegetableOrFruit) && !this.Eats(FoodTypeFlags.Plant))
					{
						return DietCategory.Carnivorous;
					}
					return DietCategory.Omnivorous;
				}
				if (this.Eats(FoodTypeFlags.AnimalProduct))
				{
					return DietCategory.Ovivorous;
				}
				return DietCategory.Herbivorous;
			}
		}

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

		public bool CanDoHerdMigration
		{
			get
			{
				return this.Animal && this.herdAnimal && this.herdMigrationAllowed;
			}
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
			if (t.def.ingestible == null)
			{
				return false;
			}
			if (!this.CanEverEat(t))
			{
				return false;
			}
			return true;
		}

		public bool CanEverEat(Thing t)
		{
			return this.CanEverEat(t.def);
		}

		public bool CanEverEat(ThingDef t)
		{
			if (!this.EatsFood)
			{
				return false;
			}
			if (t.ingestible == null)
			{
				return false;
			}
			if (t.ingestible.preferability == FoodPreferability.Undefined)
			{
				return false;
			}
			return this.Eats(t.ingestible.foodType);
		}

		public bool Eats(FoodTypeFlags food)
		{
			if (!this.EatsFood)
			{
				return false;
			}
			return (this.foodType & food) != FoodTypeFlags.None;
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

		public IEnumerable<string> ConfigErrors()
		{
			if (this.soundMeleeHitPawn == null)
			{
				yield return "soundMeleeHitPawn is null";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.soundMeleeHitBuilding == null)
			{
				yield return "soundMeleeHitBuilding is null";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.soundMeleeMiss == null)
			{
				yield return "soundMeleeMiss is null";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.predator && !this.Eats(FoodTypeFlags.Meat))
			{
				yield return "predator but doesn't eat meat";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			for (int j = 0; j < this.lifeStageAges.Count; j++)
			{
				for (int i = 0; i < j; i++)
				{
					if (this.lifeStageAges[i].minAge > this.lifeStageAges[j].minAge)
					{
						yield return "lifeStages minAges are not in ascending order";
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (this.litterSizeCurve != null)
			{
				using (IEnumerator<string> enumerator = this.litterSizeCurve.ConfigErrors("litterSizeCurve").GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						string e = enumerator.Current;
						yield return e;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (this.nameOnTameChance > 0.0 && this.nameGenerator == null)
			{
				yield return "can be named, but has no nameGenerator";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.Animal && this.wildness < 0.0)
			{
				yield return "is animal but wildness is not defined";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.useMeatFrom != null && this.useMeatFrom.category != ThingCategory.Pawn)
			{
				yield return "tries to use meat from non-pawn " + this.useMeatFrom;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.useMeatFrom != null && this.useMeatFrom.race.useMeatFrom != null)
			{
				yield return "tries to use meat from " + this.useMeatFrom + " which uses meat from " + this.useMeatFrom.race.useMeatFrom;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.useLeatherFrom != null && this.useLeatherFrom.category != ThingCategory.Pawn)
			{
				yield return "tries to use leather from non-pawn " + this.useLeatherFrom;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.useLeatherFrom == null)
				yield break;
			if (this.useLeatherFrom.race.useLeatherFrom == null)
				yield break;
			yield return "tries to use leather from " + this.useLeatherFrom + " which uses leather from " + this.useLeatherFrom.race.useLeatherFrom;
			/*Error: Unable to find new state assignment for yield return*/;
			IL_04c6:
			/*Error near IL_04c7: Unexpected return in MoveNext()*/;
		}

		internal IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Race".Translate(), parentDef.LabelCap, 2000, string.Empty);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
