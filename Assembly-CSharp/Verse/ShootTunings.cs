using System;

namespace Verse
{
	// Token: 0x02000FB8 RID: 4024
	public static class ShootTunings
	{
		// Token: 0x04003F96 RID: 16278
		public const float MinAccuracyFactorFromShooterAndDistance = 0.0201f;

		// Token: 0x04003F97 RID: 16279
		public const float LayingDownHitChanceFactorMinDistance = 4.5f;

		// Token: 0x04003F98 RID: 16280
		public const float HitChanceFactorIfLayingDown = 0.2f;

		// Token: 0x04003F99 RID: 16281
		public const float NonPawnShooterHitFactorPerDistance = 0.96f;

		// Token: 0x04003F9A RID: 16282
		public const float ExecutionMaxDistance = 3.9f;

		// Token: 0x04003F9B RID: 16283
		public const float ExecutionAccuracyFactor = 7.5f;

		// Token: 0x04003F9C RID: 16284
		public const float TargetSizeFactorFromFillPercentFactor = 1.7f;

		// Token: 0x04003F9D RID: 16285
		public const float TargetSizeFactorMin = 0.5f;

		// Token: 0x04003F9E RID: 16286
		public const float TargetSizeFactorMax = 2f;

		// Token: 0x04003F9F RID: 16287
		public const float MinAimOnChance_StandardTarget = 0.0201f;

		// Token: 0x04003FA0 RID: 16288
		public static readonly SimpleCurve2D MissDistanceFromAimOnChanceCurves = new SimpleCurve2D
		{
			new CurveColumn(0.02f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 10f),
					true
				}
			}),
			new CurveColumn(0.04f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 8f),
					true
				}
			}),
			new CurveColumn(0.07f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 6f),
					true
				}
			}),
			new CurveColumn(0.11f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 4f),
					true
				}
			}),
			new CurveColumn(0.22f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 2f),
					true
				}
			}),
			new CurveColumn(1f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 1f),
					true
				}
			})
		};

		// Token: 0x04003FA1 RID: 16289
		public const float CanInterceptPawnsChanceOnWildOrForcedMissRadius = 0.5f;

		// Token: 0x04003FA2 RID: 16290
		public const float Intercept_Pawn_HitChancePerBodySize = 0.4f;

		// Token: 0x04003FA3 RID: 16291
		public const float Intercept_Pawn_HitChanceFactor_LayingDown = 0.1f;

		// Token: 0x04003FA4 RID: 16292
		public const float Intercept_Pawn_HitChanceFactor_NonWildNonEnemy = 0.4f;

		// Token: 0x04003FA5 RID: 16293
		public const float Intercept_Object_HitChancePerFillPercent = 0.15f;

		// Token: 0x04003FA6 RID: 16294
		public const float Intercept_Object_AdjToTarget_HitChancePerFillPercent = 1f;

		// Token: 0x04003FA7 RID: 16295
		public const float Intercept_OpenDoor_HitChance = 0.05f;

		// Token: 0x04003FA8 RID: 16296
		public const float ImpactCell_Pawn_HitChancePerBodySize = 0.5f;

		// Token: 0x04003FA9 RID: 16297
		public const float ImpactCell_Object_HitChancePerFillPercent = 1.5f;

		// Token: 0x04003FAA RID: 16298
		public const float BodySizeClampMin = 0.1f;

		// Token: 0x04003FAB RID: 16299
		public const float BodySizeClampMax = 2f;
	}
}
