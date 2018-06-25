using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009E0 RID: 2528
	public static class AITuning
	{
		// Token: 0x04002435 RID: 9269
		public const int ConstantThinkTreeJobCheckIntervalTicks = 30;

		// Token: 0x04002436 RID: 9270
		[TweakValue("AI", 0f, 10f)]
		public static float OpportunisticJobMinDistPawnToDest = 3f;

		// Token: 0x04002437 RID: 9271
		[TweakValue("AI", 5f, 50f)]
		public static float OpportunisticJobMaxDistPawnToItem = 30f;

		// Token: 0x04002438 RID: 9272
		[TweakValue("AI", 0f, 2f)]
		public static float OpportunisticJobMaxPickupDistanceFactor = 0.5f;

		// Token: 0x04002439 RID: 9273
		[TweakValue("AI", 1f, 3f)]
		public static float OpportunisticJobMaxRatioOppHaulDistanceToDestDistance = 1.5f;

		// Token: 0x0400243A RID: 9274
		[TweakValue("AI", 5f, 50f)]
		public static float OpportunisticJobMaxDistDestToDropoff = 40f;

		// Token: 0x0400243B RID: 9275
		[TweakValue("AI", 0f, 2f)]
		public static float OpportunisticJobMaxDistDestToDropoffFactor = 0.5f;

		// Token: 0x0400243C RID: 9276
		[TweakValue("AI", 1f, 50f)]
		public static int OpportunisticJobMaxPickupRegions = 25;

		// Token: 0x0400243D RID: 9277
		[TweakValue("AI", 1f, 50f)]
		public static int OpportunisticJobMaxDropoffRegions = 25;
	}
}
