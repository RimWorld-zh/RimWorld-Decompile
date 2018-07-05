using System;
using Verse;

namespace RimWorld
{
	public static class DiplomacyTuning
	{
		public const int MaxGoodwill = 100;

		public const int MinGoodwill = -100;

		public const int BecomeHostileThreshold = -75;

		public const int BecomeNeutralThreshold = 0;

		public const int BecomeAllyThreshold = 75;

		public const int InitialHostileThreshold = -10;

		public const int InitialAllyThreshold = 75;

		public static readonly IntRange ForcedStartingEnemyGoodwillRange = new IntRange(-100, -40);

		public const int MinGoodwillToRequestAICoreQuest = 40;

		public const int RequestAICoreQuestSilverCost = 1500;

		public static readonly FloatRange RansomFeeMarketValueFactorRange = new FloatRange(1.2f, 2.2f);

		public const int Goodwill_NaturalChangeStep = 10;

		public const float Goodwill_PerDirectDamageToPawn = -1.3f;

		public const int Goodwill_MemberCrushed_Humanlike = -25;

		public const int Goodwill_MemberCrushed_Animal = -15;

		public const int Goodwill_MemberNeutrallyDied_Humanlike = -5;

		public const int Goodwill_MemberNeutrallyDied_Animal = -3;

		public const int Goodwill_BodyPartRemovalViolation = -15;

		public const int Goodwill_AttackedSettlement = -50;

		public const int Goodwill_MilitaryAidRequested = -20;

		public const int Goodwill_TraderRequested = -15;

		public static readonly SimpleCurve Goodwill_PerQuadrumFromSettlementProximity = new SimpleCurve
		{
			{
				new CurvePoint(2f, -30f),
				true
			},
			{
				new CurvePoint(3f, -20f),
				true
			},
			{
				new CurvePoint(4f, -10f),
				true
			},
			{
				new CurvePoint(5f, 0f),
				true
			}
		};

		public const float Goodwill_GiftSilverForOneGoodwill = 25f;

		public const float Goodwill_GiftPrisonerOfTheirFactionValueFactor = 2f;

		public const float Goodwill_TradedMarketValueforOneGoodwill = 400f;

		public const int Goodwill_DestroyedMutualEnemyBase = 15;

		public const int Goodwill_MemberExitedMapHealthy = 15;

		public const int Goodwill_MemberExitedMapHealthy_LeaderBonus = 40;

		public const float Goodwill_PerTend = 1f;

		public const int Goodwill_MaxTimesTendedTo = 10;

		public const int Goodwill_QuestBanditCampCompleted = 10;

		public const int Goodwill_QuestTradeRequestCompleted = 5;

		public static readonly IntRange Goodwill_PeaceTalksDisasterRange = new IntRange(-50, -40);

		public static readonly IntRange Goodwill_PeaceTalksBackfireRange = new IntRange(-20, -10);

		public static readonly IntRange Goodwill_PeaceTalksSuccessRange = new IntRange(60, 70);

		public static readonly IntRange Goodwill_PeaceTalksTriumphRange = new IntRange(100, 110);

		// Note: this type is marked as 'beforefieldinit'.
		static DiplomacyTuning()
		{
		}
	}
}
