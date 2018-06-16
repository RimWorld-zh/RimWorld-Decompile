using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000557 RID: 1367
	public static class DiplomacyTuning
	{
		// Token: 0x04000EDA RID: 3802
		public const int MaxGoodwill = 100;

		// Token: 0x04000EDB RID: 3803
		public const int MinGoodwill = -100;

		// Token: 0x04000EDC RID: 3804
		public const int BecomeHostileThreshold = -75;

		// Token: 0x04000EDD RID: 3805
		public const int BecomeNeutralThreshold = 0;

		// Token: 0x04000EDE RID: 3806
		public const int BecomeAllyThreshold = 75;

		// Token: 0x04000EDF RID: 3807
		public const int InitialHostileThreshold = -10;

		// Token: 0x04000EE0 RID: 3808
		public const int InitialAllyThreshold = 75;

		// Token: 0x04000EE1 RID: 3809
		public static readonly IntRange ForcedStartingEnemyGoodwillRange = new IntRange(-100, -40);

		// Token: 0x04000EE2 RID: 3810
		public const int MinGoodwillToRequestAICoreQuest = 40;

		// Token: 0x04000EE3 RID: 3811
		public const int RequestAICoreQuestSilverCost = 1500;

		// Token: 0x04000EE4 RID: 3812
		public static readonly FloatRange RansomFeeMarketValueFactorRange = new FloatRange(1.2f, 2.2f);

		// Token: 0x04000EE5 RID: 3813
		public const int Goodwill_NaturalChangeStep = 10;

		// Token: 0x04000EE6 RID: 3814
		public const float Goodwill_PerDirectDamageToPawn = -1.3f;

		// Token: 0x04000EE7 RID: 3815
		public const int Goodwill_MemberCrushed_Humanlike = -25;

		// Token: 0x04000EE8 RID: 3816
		public const int Goodwill_MemberCrushed_Animal = -15;

		// Token: 0x04000EE9 RID: 3817
		public const int Goodwill_MemberNeutrallyDied_Humanlike = -5;

		// Token: 0x04000EEA RID: 3818
		public const int Goodwill_MemberNeutrallyDied_Animal = -3;

		// Token: 0x04000EEB RID: 3819
		public const int Goodwill_BodyPartRemovalViolation = -15;

		// Token: 0x04000EEC RID: 3820
		public const int Goodwill_AttackedFactionBase = -50;

		// Token: 0x04000EED RID: 3821
		public const int Goodwill_MilitaryAidRequested = -20;

		// Token: 0x04000EEE RID: 3822
		public const int Goodwill_TraderRequested = -15;

		// Token: 0x04000EEF RID: 3823
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

		// Token: 0x04000EF0 RID: 3824
		public const float Goodwill_GiftSilverForOneGoodwill = 20f;

		// Token: 0x04000EF1 RID: 3825
		public const float Goodwill_GiftPrisonerOfTheirFactionValueFactor = 2f;

		// Token: 0x04000EF2 RID: 3826
		public const float Goodwill_TradedMarketValueforOneGoodwill = 400f;

		// Token: 0x04000EF3 RID: 3827
		public const int Goodwill_DestroyedMutualEnemyBase = 15;

		// Token: 0x04000EF4 RID: 3828
		public const int Goodwill_MemberExitedMapHealthy = 15;

		// Token: 0x04000EF5 RID: 3829
		public const int Goodwill_MemberExitedMapHealthy_LeaderBonus = 40;

		// Token: 0x04000EF6 RID: 3830
		public const float Goodwill_PerTend = 1f;

		// Token: 0x04000EF7 RID: 3831
		public const int Goodwill_MaxTimesTendedTo = 10;

		// Token: 0x04000EF8 RID: 3832
		public const int Goodwill_QuestBanditCampCompleted = 10;

		// Token: 0x04000EF9 RID: 3833
		public const int Goodwill_QuestTradeRequestCompleted = 5;

		// Token: 0x04000EFA RID: 3834
		public static readonly IntRange Goodwill_PeaceTalksDisasterRange = new IntRange(-50, -40);

		// Token: 0x04000EFB RID: 3835
		public static readonly IntRange Goodwill_PeaceTalksBackfireRange = new IntRange(-20, -10);

		// Token: 0x04000EFC RID: 3836
		public static readonly IntRange Goodwill_PeaceTalksSuccessRange = new IntRange(60, 70);

		// Token: 0x04000EFD RID: 3837
		public static readonly IntRange Goodwill_PeaceTalksTriumphRange = new IntRange(100, 110);
	}
}
