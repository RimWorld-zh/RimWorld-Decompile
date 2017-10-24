using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class RaceProperties
	{
		public Intelligence intelligence = Intelligence.Animal;

		private FleshTypeDef fleshType = null;

		private ThingDef bloodDef = null;

		public bool hasGenders = true;

		public bool needsRest = true;

		public ThinkTreeDef thinkTreeMain;

		public ThinkTreeDef thinkTreeConstant;

		public PawnNameCategory nameCategory = PawnNameCategory.NoName;

		public FoodTypeFlags foodType = FoodTypeFlags.None;

		public BodyDef body = null;

		public Type deathActionWorkerClass;

		public List<AnimalBiomeRecord> wildBiomes = null;

		public SimpleCurve ageGenerationCurve = null;

		public bool makesFootprints = false;

		public int executionRange = 2;

		public float lifeExpectancy = 10f;

		public List<HediffGiverSetDef> hediffGiverSets = null;

		public float petness = 0f;

		public bool packAnimal = false;

		public bool herdAnimal = false;

		public bool predator = false;

		public float maxPreyBodySize = 99999f;

		public float wildness = -1f;

		public float nuzzleMtbHours = -1f;

		public float manhunterOnDamageChance = 0f;

		public float manhunterOnTameFailChance = 0f;

		public bool canBePredatorPrey = true;

		public float gestationPeriodDays = 10f;

		public SimpleCurve litterSizeCurve = null;

		public float mateMtbHours = 12f;

		public List<string> untrainableTags = null;

		public List<string> trainableTags = null;

		private TrainableIntelligenceDef trainableIntelligence = null;

		private RulePackDef nameGenerator;

		private RulePackDef nameGeneratorFemale;

		public float nameOnTameChance = 0f;

		public float nameOnNuzzleChance = 0f;

		public float baseBodySize = 1f;

		public float baseHealthScale = 1f;

		public float baseHungerRate = 1f;

		public List<LifeStageAge> lifeStageAges = new List<LifeStageAge>();

		public Color leatherColor = ColorLibrary.Leather;

		public string leatherLabel = (string)null;

		public float leatherCommonalityFactor = 1f;

		public float leatherInsulation = 1.1f;

		public List<StatModifier> leatherStatFactors = null;

		public float leatherMarketValueFactor = 1f;

		public string meatLabel = (string)null;

		public Color meatColor = new ColorInt(141, 56, 52).ToColor;

		public ThingDef useMeatFrom = null;

		public ThingDef useLeatherFrom = null;

		public ShadowData specialShadowData = null;

		public IntRange soundCallIntervalRange = new IntRange(2000, 4000);

		public SoundDef soundMeleeHitPawn = null;

		public SoundDef soundMeleeHitBuilding = null;

		public SoundDef soundMeleeMiss = null;

		[Unsaved]
		private DeathActionWorker deathActionWorkerInt = null;

		[Unsaved]
		public ThingDef meatDef = null;

		[Unsaved]
		public ThingDef leatherDef = null;

		[Unsaved]
		public ThingDef corpseDef = null;

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
				float result;
				switch (this.ResolvedDietCategory)
				{
				case DietCategory.NeverEats:
				{
					result = 0.3f;
					break;
				}
				case DietCategory.Omnivorous:
				{
					result = 0.3f;
					break;
				}
				case DietCategory.Carnivorous:
				{
					result = 0.3f;
					break;
				}
				case DietCategory.Ovivorous:
				{
					result = 0.4f;
					break;
				}
				case DietCategory.Herbivorous:
				{
					result = 0.45f;
					break;
				}
				case DietCategory.Dendrovorous:
				{
					result = 0.45f;
					break;
				}
				default:
				{
					throw new InvalidOperationException();
				}
				}
				return result;
			}
		}

		public DietCategory ResolvedDietCategory
		{
			get
			{
				return (DietCategory)(this.EatsFood ? ((!this.Eats(FoodTypeFlags.Tree)) ? ((!this.Eats(FoodTypeFlags.Meat)) ? ((!this.Eats(FoodTypeFlags.AnimalProduct)) ? 1 : 3) : ((!this.Eats(FoodTypeFlags.VegetableOrFruit) && !this.Eats(FoodTypeFlags.Plant)) ? 5 : 4)) : 2) : 0);
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
				return (this.fleshType == null) ? FleshTypeDefOf.Normal : this.fleshType;
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
				return (this.bloodDef == null) ? ((!this.IsFlesh) ? null : ThingDefOf.FilthBlood) : this.bloodDef;
			}
		}

		public TrainableIntelligenceDef TrainableIntelligence
		{
			get
			{
				return (this.trainableIntelligence != null) ? this.trainableIntelligence : TrainableIntelligenceDefOf.Intermediate;
			}
		}

		public RulePackDef GetNameGenerator(Gender gender)
		{
			return (gender != Gender.Female || this.nameGeneratorFemale == null) ? this.nameGenerator : this.nameGeneratorFemale;
		}

		public bool WillAutomaticallyEat(Thing t)
		{
			return (byte)((t.def.ingestible != null) ? (this.CanEverEat(t) ? 1 : 0) : 0) != 0;
		}

		public bool CanEverEat(Thing t)
		{
			return this.CanEverEat(t.def);
		}

		public bool CanEverEat(ThingDef t)
		{
			return this.EatsFood && t.ingestible != null && t.ingestible.preferability != 0 && this.Eats(t.ingestible.foodType);
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
			IL_04d0:
			/*Error near IL_04d1: Unexpected return in MoveNext()*/;
		}

		internal IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Race".Translate(), parentDef.LabelCap, 2000, "");
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
