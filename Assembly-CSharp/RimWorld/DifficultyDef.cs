using System;
using Verse;

namespace RimWorld
{
	public sealed class DifficultyDef : Def
	{
		public int difficulty = -1;

		public float threatScale = 1f;

		public bool allowBigThreats = true;

		public bool allowIntroThreats = true;

		public bool allowCaveHives = true;

		public bool peacefulTemples = false;

		public bool predatorsHuntHumanlikes = true;

		public float colonistMoodOffset = 0f;

		public float tradePriceFactorLoss = 0f;

		public float cropYieldFactor = 1f;

		public float mineYieldFactor = 1f;

		public float diseaseIntervalFactor = 1f;

		public float enemyReproductionRateFactor = 1f;

		public float playerPawnInfectionChanceFactor = 1f;

		public float manhunterChanceOnDamageFactor = 1f;

		public float deepDrillInfestationChanceFactor = 1f;

		public float foodPoisonChanceFactor = 1f;

		public float raidBeaconThreatMtbFactor = 1f;

		public float maintenanceCostFactor = 1f;

		public float enemyDeathOnDownedChanceFactor = 1f;

		public DifficultyDef()
		{
		}
	}
}
