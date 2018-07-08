using System;

namespace Verse
{
	public class HealthTuning
	{
		public const int StandardInterval = 60;

		public const float SmallPawnFragmentedDamageHealthScaleThreshold = 0.5f;

		public const int SmallPawnFragmentedDamageMinimumDamageAmount = 10;

		public static float ChanceToAdditionallyDamageInnerSolidPart = 0.2f;

		public const float MinBleedingRateToBleed = 0.1f;

		public const float BleedSeverityRecoveryPerInterval = 0.00033333333f;

		public const float BloodFilthDropChanceFactorStanding = 0.004f;

		public const float BloodFilthDropChanceFactorLaying = 0.0004f;

		public const int BaseTicksAfterInjuryToStopBleeding = 90000;

		public const int TicksAfterMissingBodyPartToStopBeingFresh = 90000;

		public const float DefaultPainShockThreshold = 0.8f;

		public const int InjuryHealInterval = 600;

		public const float InjuryHealPerDay_Base = 8f;

		public const float InjuryHealPerDayOffset_Laying = 4f;

		public const float InjuryHealPerDayOffset_Tended = 16f;

		public const int InjurySeverityTendedPerMedicine = 20;

		public const float BaseTotalDamageLethalThreshold = 150f;

		public const int MinDamageSeverityForPermanent = 7;

		public const float MinDamagePartPctForPermanent = 0.25f;

		public const float MinDamagePartPctForInfection = 0.2f;

		public static readonly IntRange InfectionDelayRange = new IntRange(15000, 45000);

		public const float AnimalsInfectionChanceFactor = 0.1f;

		public const float HypothermiaGrowthPerDegreeUnder = 6.45E-05f;

		public const float HeatstrokeGrowthPerDegreeOver = 6.45E-05f;

		public const float MinHeatstrokeProgressPerInterval = 0.000375f;

		public const float MinHypothermiaProgress = 0.00075f;

		public const float HarmfulTemperatureOffset = 10f;

		public const float MinTempOverComfyMaxForBurn = 150f;

		public const float BurnDamagePerTempOverage = 0.06f;

		public const int MinBurnDamage = 3;

		public const float ImpossibleToFallSickIfAboveThisImmunityLevel = 0.6f;

		public const int HediffGiverUpdateInterval = 60;

		public const int VomitCheckInterval = 600;

		public const int DeathCheckInterval = 200;

		public const int ForgetRandomMemoryThoughtCheckInterval = 400;

		public const float PawnBaseHealthForSummary = 75f;

		public const float BaseBecomePermanentChance = 0.1f;

		public const float DeathOnDownedChance_NonColonyAnimal = 0.5f;

		public const float DeathOnDownedChance_NonColonyMechanoid = 1f;

		public static readonly SimpleCurve DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(-1f, 0.87f),
				true
			},
			{
				new CurvePoint(0f, 0.77f),
				true
			},
			{
				new CurvePoint(1f, 0.67f),
				true
			},
			{
				new CurvePoint(2f, 0.61f),
				true
			}
		};

		public const float TendPriority_LifeThreateningDisease = 1f;

		public const float TendPriority_PerBleedRate = 1.5f;

		public const float TendPriority_DiseaseSeverityDecreasesWhenTended = 0.025f;

		public HealthTuning()
		{
		}

		// Note: this type is marked as 'beforefieldinit'.
		static HealthTuning()
		{
		}
	}
}
