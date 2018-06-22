using System;

namespace Verse
{
	// Token: 0x02000CFC RID: 3324
	public class HealthTunings
	{
		// Token: 0x04003198 RID: 12696
		public const int StandardInterval = 60;

		// Token: 0x04003199 RID: 12697
		public const float SmallPawnFragmentedDamageHealthScaleThreshold = 0.5f;

		// Token: 0x0400319A RID: 12698
		public const int SmallPawnFragmentedDamageMinimumDamageAmount = 10;

		// Token: 0x0400319B RID: 12699
		public static float ChanceToAdditionallyDamageInnerSolidPart = 0.2f;

		// Token: 0x0400319C RID: 12700
		public const float MinBleedingRateToBleed = 0.1f;

		// Token: 0x0400319D RID: 12701
		public const float BleedSeverityRecoveryPerInterval = 0.00033333333f;

		// Token: 0x0400319E RID: 12702
		public const float BloodFilthDropChanceFactorStanding = 0.004f;

		// Token: 0x0400319F RID: 12703
		public const float BloodFilthDropChanceFactorLaying = 0.0004f;

		// Token: 0x040031A0 RID: 12704
		public const int BaseTicksAfterInjuryToStopBleeding = 90000;

		// Token: 0x040031A1 RID: 12705
		public const int TicksAfterMissingBodyPartToStopBeingFresh = 90000;

		// Token: 0x040031A2 RID: 12706
		public const float DefaultPainShockThreshold = 0.8f;

		// Token: 0x040031A3 RID: 12707
		public const int InjuryHealInterval = 600;

		// Token: 0x040031A4 RID: 12708
		public const float InjuryHealPerDay_Untended = 8f;

		// Token: 0x040031A5 RID: 12709
		public const float InjuryHealPerDay_Tended = 22f;

		// Token: 0x040031A6 RID: 12710
		public const float InjuryHealPerDay_Laying = 4f;

		// Token: 0x040031A7 RID: 12711
		public const int InjurySeverityTendedPerMedicine = 20;

		// Token: 0x040031A8 RID: 12712
		public const float BaseTotalDamageLethalThreshold = 150f;

		// Token: 0x040031A9 RID: 12713
		public const int MinDamageSeverityForPermanent = 7;

		// Token: 0x040031AA RID: 12714
		public const float MinDamagePartPctForPermanent = 0.25f;

		// Token: 0x040031AB RID: 12715
		public const float MinDamagePartPctForInfection = 0.2f;

		// Token: 0x040031AC RID: 12716
		public static readonly IntRange InfectionDelayRange = new IntRange(15000, 45000);

		// Token: 0x040031AD RID: 12717
		public const float AnimalsInfectionChanceFactor = 0.2f;

		// Token: 0x040031AE RID: 12718
		public const float HypothermiaGrowthPerDegreeUnder = 6.45E-05f;

		// Token: 0x040031AF RID: 12719
		public const float HeatstrokeGrowthPerDegreeOver = 6.45E-05f;

		// Token: 0x040031B0 RID: 12720
		public const float MinHeatstrokeProgressPerInterval = 0.000375f;

		// Token: 0x040031B1 RID: 12721
		public const float MinHypothermiaProgress = 0.00075f;

		// Token: 0x040031B2 RID: 12722
		public const float HarmfulTemperatureOffset = 10f;

		// Token: 0x040031B3 RID: 12723
		public const float MinTempOverComfyMaxForBurn = 150f;

		// Token: 0x040031B4 RID: 12724
		public const float BurnDamagePerTempOverage = 0.06f;

		// Token: 0x040031B5 RID: 12725
		public const int MinBurnDamage = 3;

		// Token: 0x040031B6 RID: 12726
		public const float ImpossibleToFallSickIfAboveThisImmunityLevel = 0.6f;

		// Token: 0x040031B7 RID: 12727
		public const int HediffGiverUpdateInterval = 60;

		// Token: 0x040031B8 RID: 12728
		public const int VomitCheckInterval = 600;

		// Token: 0x040031B9 RID: 12729
		public const int DeathCheckInterval = 200;

		// Token: 0x040031BA RID: 12730
		public const int ForgetRandomMemoryThoughtCheckInterval = 400;

		// Token: 0x040031BB RID: 12731
		public const float PawnBaseHealthForSummary = 75f;

		// Token: 0x040031BC RID: 12732
		public const float BaseBecomePermanentChance = 0.1f;

		// Token: 0x040031BD RID: 12733
		public const float DeathOnDownedChance_NonColonyHumanlike = 0.67f;

		// Token: 0x040031BE RID: 12734
		public const float DeathOnDownedChance_NonColonyAnimal = 0.47f;

		// Token: 0x040031BF RID: 12735
		public const float DeathOnDownedChance_NonColonyMechanoid = 1f;

		// Token: 0x040031C0 RID: 12736
		public const float TendPriority_LifeThreateningDisease = 1f;

		// Token: 0x040031C1 RID: 12737
		public const float TendPriority_PerBleedRate = 1.5f;

		// Token: 0x040031C2 RID: 12738
		public const float TendPriority_DiseaseSeverityDecreasesWhenTended = 0.025f;
	}
}
