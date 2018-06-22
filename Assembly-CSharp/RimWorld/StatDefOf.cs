using System;

namespace RimWorld
{
	// Token: 0x0200093C RID: 2364
	[DefOf]
	public static class StatDefOf
	{
		// Token: 0x06003644 RID: 13892 RVA: 0x001D0B85 File Offset: 0x001CEF85
		static StatDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StatDefOf));
		}

		// Token: 0x040020F8 RID: 8440
		public static StatDef MaxHitPoints;

		// Token: 0x040020F9 RID: 8441
		public static StatDef MarketValue;

		// Token: 0x040020FA RID: 8442
		public static StatDef SellPriceFactor;

		// Token: 0x040020FB RID: 8443
		public static StatDef Beauty;

		// Token: 0x040020FC RID: 8444
		public static StatDef Cleanliness;

		// Token: 0x040020FD RID: 8445
		public static StatDef Flammability;

		// Token: 0x040020FE RID: 8446
		public static StatDef DeteriorationRate;

		// Token: 0x040020FF RID: 8447
		public static StatDef WorkToMake;

		// Token: 0x04002100 RID: 8448
		public static StatDef WorkToBuild;

		// Token: 0x04002101 RID: 8449
		public static StatDef Mass;

		// Token: 0x04002102 RID: 8450
		public static StatDef ConstructionSpeedFactor;

		// Token: 0x04002103 RID: 8451
		public static StatDef Nutrition;

		// Token: 0x04002104 RID: 8452
		public static StatDef FoodPoisonChanceFixedHuman;

		// Token: 0x04002105 RID: 8453
		public static StatDef MoveSpeed;

		// Token: 0x04002106 RID: 8454
		public static StatDef GlobalLearningFactor;

		// Token: 0x04002107 RID: 8455
		public static StatDef HungerRateMultiplier;

		// Token: 0x04002108 RID: 8456
		public static StatDef RestRateMultiplier;

		// Token: 0x04002109 RID: 8457
		public static StatDef PsychicSensitivity;

		// Token: 0x0400210A RID: 8458
		public static StatDef ToxicSensitivity;

		// Token: 0x0400210B RID: 8459
		public static StatDef MentalBreakThreshold;

		// Token: 0x0400210C RID: 8460
		public static StatDef EatingSpeed;

		// Token: 0x0400210D RID: 8461
		public static StatDef ComfyTemperatureMin;

		// Token: 0x0400210E RID: 8462
		public static StatDef ComfyTemperatureMax;

		// Token: 0x0400210F RID: 8463
		public static StatDef Comfort;

		// Token: 0x04002110 RID: 8464
		public static StatDef MeatAmount;

		// Token: 0x04002111 RID: 8465
		public static StatDef LeatherAmount;

		// Token: 0x04002112 RID: 8466
		public static StatDef MinimumHandlingSkill;

		// Token: 0x04002113 RID: 8467
		public static StatDef MeleeDPS;

		// Token: 0x04002114 RID: 8468
		public static StatDef PainShockThreshold;

		// Token: 0x04002115 RID: 8469
		public static StatDef ForagedNutritionPerDay;

		// Token: 0x04002116 RID: 8470
		public static StatDef WorkSpeedGlobal;

		// Token: 0x04002117 RID: 8471
		public static StatDef MiningSpeed;

		// Token: 0x04002118 RID: 8472
		public static StatDef MiningYield;

		// Token: 0x04002119 RID: 8473
		public static StatDef ResearchSpeed;

		// Token: 0x0400211A RID: 8474
		public static StatDef ConstructionSpeed;

		// Token: 0x0400211B RID: 8475
		public static StatDef DiplomacyPower;

		// Token: 0x0400211C RID: 8476
		public static StatDef TradePriceImprovement;

		// Token: 0x0400211D RID: 8477
		public static StatDef PlantWorkSpeed;

		// Token: 0x0400211E RID: 8478
		public static StatDef SmoothingSpeed;

		// Token: 0x0400211F RID: 8479
		public static StatDef FoodPoisonChance;

		// Token: 0x04002120 RID: 8480
		public static StatDef CarryingCapacity;

		// Token: 0x04002121 RID: 8481
		public static StatDef PlantHarvestYield;

		// Token: 0x04002122 RID: 8482
		public static StatDef FixBrokenDownBuildingSuccessChance;

		// Token: 0x04002123 RID: 8483
		public static StatDef ConstructSuccessChance;

		// Token: 0x04002124 RID: 8484
		public static StatDef MedicalTendSpeed;

		// Token: 0x04002125 RID: 8485
		public static StatDef MedicalTendQuality;

		// Token: 0x04002126 RID: 8486
		public static StatDef MedicalSurgerySuccessChance;

		// Token: 0x04002127 RID: 8487
		public static StatDef SocialImpact;

		// Token: 0x04002128 RID: 8488
		public static StatDef RecruitPrisonerChance;

		// Token: 0x04002129 RID: 8489
		public static StatDef AnimalGatherSpeed;

		// Token: 0x0400212A RID: 8490
		public static StatDef AnimalGatherYield;

		// Token: 0x0400212B RID: 8491
		public static StatDef TameAnimalChance;

		// Token: 0x0400212C RID: 8492
		public static StatDef TrainAnimalChance;

		// Token: 0x0400212D RID: 8493
		public static StatDef ShootingAccuracy;

		// Token: 0x0400212E RID: 8494
		public static StatDef AimingDelayFactor;

		// Token: 0x0400212F RID: 8495
		public static StatDef MeleeHitChance;

		// Token: 0x04002130 RID: 8496
		public static StatDef MeleeDodgeChance;

		// Token: 0x04002131 RID: 8497
		public static StatDef Weapon_Bulk;

		// Token: 0x04002132 RID: 8498
		public static StatDef MeleeWeapon_AverageDPS;

		// Token: 0x04002133 RID: 8499
		public static StatDef MeleeWeapon_DamageMultiplier;

		// Token: 0x04002134 RID: 8500
		public static StatDef MeleeWeapon_CooldownMultiplier;

		// Token: 0x04002135 RID: 8501
		public static StatDef SharpDamageMultiplier;

		// Token: 0x04002136 RID: 8502
		public static StatDef BluntDamageMultiplier;

		// Token: 0x04002137 RID: 8503
		public static StatDef StuffPower_Armor_Sharp;

		// Token: 0x04002138 RID: 8504
		public static StatDef StuffPower_Armor_Blunt;

		// Token: 0x04002139 RID: 8505
		public static StatDef StuffPower_Armor_Heat;

		// Token: 0x0400213A RID: 8506
		public static StatDef StuffPower_Insulation_Cold;

		// Token: 0x0400213B RID: 8507
		public static StatDef StuffPower_Insulation_Heat;

		// Token: 0x0400213C RID: 8508
		public static StatDef RangedWeapon_Cooldown;

		// Token: 0x0400213D RID: 8509
		public static StatDef AccuracyTouch;

		// Token: 0x0400213E RID: 8510
		public static StatDef AccuracyShort;

		// Token: 0x0400213F RID: 8511
		public static StatDef AccuracyMedium;

		// Token: 0x04002140 RID: 8512
		public static StatDef AccuracyLong;

		// Token: 0x04002141 RID: 8513
		public static StatDef StuffEffectMultiplierArmor;

		// Token: 0x04002142 RID: 8514
		public static StatDef StuffEffectMultiplierInsulation_Cold;

		// Token: 0x04002143 RID: 8515
		public static StatDef StuffEffectMultiplierInsulation_Heat;

		// Token: 0x04002144 RID: 8516
		public static StatDef ArmorRating_Sharp;

		// Token: 0x04002145 RID: 8517
		public static StatDef ArmorRating_Blunt;

		// Token: 0x04002146 RID: 8518
		public static StatDef ArmorRating_Heat;

		// Token: 0x04002147 RID: 8519
		public static StatDef Insulation_Cold;

		// Token: 0x04002148 RID: 8520
		public static StatDef Insulation_Heat;

		// Token: 0x04002149 RID: 8521
		public static StatDef EnergyShieldRechargeRate;

		// Token: 0x0400214A RID: 8522
		public static StatDef EnergyShieldEnergyMax;

		// Token: 0x0400214B RID: 8523
		public static StatDef SmokepopBeltRadius;

		// Token: 0x0400214C RID: 8524
		public static StatDef EquipDelay;

		// Token: 0x0400214D RID: 8525
		public static StatDef MedicalPotency;

		// Token: 0x0400214E RID: 8526
		public static StatDef MedicalQualityMax;

		// Token: 0x0400214F RID: 8527
		public static StatDef ImmunityGainSpeed;

		// Token: 0x04002150 RID: 8528
		public static StatDef ImmunityGainSpeedFactor;

		// Token: 0x04002151 RID: 8529
		public static StatDef DoorOpenSpeed;

		// Token: 0x04002152 RID: 8530
		public static StatDef BedRestEffectiveness;

		// Token: 0x04002153 RID: 8531
		public static StatDef TrapMeleeDamage;

		// Token: 0x04002154 RID: 8532
		public static StatDef TrapSpringChance;

		// Token: 0x04002155 RID: 8533
		public static StatDef ResearchSpeedFactor;

		// Token: 0x04002156 RID: 8534
		public static StatDef MedicalTendQualityOffset;

		// Token: 0x04002157 RID: 8535
		public static StatDef WorkTableWorkSpeedFactor;

		// Token: 0x04002158 RID: 8536
		public static StatDef WorkTableEfficiencyFactor;

		// Token: 0x04002159 RID: 8537
		public static StatDef JoyGainFactor;

		// Token: 0x0400215A RID: 8538
		public static StatDef SurgerySuccessChanceFactor;
	}
}
