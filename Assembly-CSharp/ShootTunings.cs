using System;

// Token: 0x02000FB4 RID: 4020
public static class ShootTunings
{
	// Token: 0x04003F91 RID: 16273
	public const float CanInterceptPawnsChanceOnWildOrForcedMissRadius = 0.5f;

	// Token: 0x04003F92 RID: 16274
	public const float Intercept_Pawn_HitChancePerBodySize = 0.4f;

	// Token: 0x04003F93 RID: 16275
	public const float Intercept_Pawn_HitChanceFactor_LayingDown = 0.1f;

	// Token: 0x04003F94 RID: 16276
	public const float Intercept_Pawn_HitChanceFactor_NonWildNonEnemy = 0.4f;

	// Token: 0x04003F95 RID: 16277
	public const float Intercept_Object_HitChancePerFillPercent = 0.15f;

	// Token: 0x04003F96 RID: 16278
	public const float Intercept_Object_AdjToTarget_HitChancePerFillPercent = 1f;

	// Token: 0x04003F97 RID: 16279
	public const float Intercept_OpenDoor_HitChance = 0.05f;

	// Token: 0x04003F98 RID: 16280
	public const float ImpactCell_Pawn_HitChancePerBodySize = 0.5f;

	// Token: 0x04003F99 RID: 16281
	public const float ImpactCell_Object_HitChancePerFillPercent = 1.5f;

	// Token: 0x04003F9A RID: 16282
	public const float BodySizeClampMin = 0.1f;

	// Token: 0x04003F9B RID: 16283
	public const float BodySizeClampMax = 2f;
}
