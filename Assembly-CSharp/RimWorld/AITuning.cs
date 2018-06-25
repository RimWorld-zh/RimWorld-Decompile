using System;
using Verse;

namespace RimWorld
{
	public static class AITuning
	{
		public const int ConstantThinkTreeJobCheckIntervalTicks = 30;

		[TweakValue("AI", 0f, 10f)]
		public static float OpportunisticJobMinDistPawnToDest = 3f;

		[TweakValue("AI", 5f, 50f)]
		public static float OpportunisticJobMaxDistPawnToItem = 30f;

		[TweakValue("AI", 0f, 2f)]
		public static float OpportunisticJobMaxPickupDistanceFactor = 0.5f;

		[TweakValue("AI", 1f, 3f)]
		public static float OpportunisticJobMaxRatioOppHaulDistanceToDestDistance = 1.5f;

		[TweakValue("AI", 5f, 50f)]
		public static float OpportunisticJobMaxDistDestToDropoff = 40f;

		[TweakValue("AI", 0f, 2f)]
		public static float OpportunisticJobMaxDistDestToDropoffFactor = 0.5f;

		[TweakValue("AI", 1f, 50f)]
		public static int OpportunisticJobMaxPickupRegions = 25;

		[TweakValue("AI", 1f, 50f)]
		public static int OpportunisticJobMaxDropoffRegions = 25;

		// Note: this type is marked as 'beforefieldinit'.
		static AITuning()
		{
		}
	}
}
