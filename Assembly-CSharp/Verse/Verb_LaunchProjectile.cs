using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FD4 RID: 4052
	public class Verb_LaunchProjectile : Verb
	{
		// Token: 0x17000FE7 RID: 4071
		// (get) Token: 0x06006204 RID: 25092 RVA: 0x001E2BA4 File Offset: 0x001E0FA4
		public virtual ThingDef Projectile
		{
			get
			{
				if (this.ownerEquipment != null)
				{
					CompChangeableProjectile comp = this.ownerEquipment.GetComp<CompChangeableProjectile>();
					if (comp != null && comp.Loaded)
					{
						return comp.Projectile;
					}
				}
				return this.verbProps.defaultProjectile;
			}
		}

		// Token: 0x06006205 RID: 25093 RVA: 0x001E2BFC File Offset: 0x001E0FFC
		public override void WarmupComplete()
		{
			base.WarmupComplete();
			Find.BattleLog.Add(new BattleLogEntry_RangedFire(this.caster, (!this.currentTarget.HasThing) ? null : this.currentTarget.Thing, (this.ownerEquipment == null) ? null : this.ownerEquipment.def, this.Projectile, this.ShotsPerBurst > 1));
		}

		// Token: 0x06006206 RID: 25094 RVA: 0x001E2C74 File Offset: 0x001E1074
		protected override bool TryCastShot()
		{
			bool result;
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				result = false;
			}
			else
			{
				ThingDef projectile = this.Projectile;
				if (projectile == null)
				{
					result = false;
				}
				else
				{
					ShootLine shootLine;
					bool flag = base.TryFindShootLineFromTo(this.caster.Position, this.currentTarget, out shootLine);
					if (this.verbProps.stopBurstWithoutLos && !flag)
					{
						result = false;
					}
					else
					{
						if (this.ownerEquipment != null)
						{
							CompChangeableProjectile comp = this.ownerEquipment.GetComp<CompChangeableProjectile>();
							if (comp != null)
							{
								comp.Notify_ProjectileLaunched();
							}
						}
						Thing launcher = this.caster;
						Thing equipment = this.ownerEquipment;
						CompMannable compMannable = this.caster.TryGetComp<CompMannable>();
						if (compMannable != null && compMannable.ManningPawn != null)
						{
							launcher = compMannable.ManningPawn;
							equipment = this.caster;
						}
						Vector3 drawPos = this.caster.DrawPos;
						Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectile, shootLine.Source, this.caster.Map, WipeMode.Vanish);
						if (this.verbProps.forcedMissRadius > 0.5f)
						{
							float num = VerbUtility.CalculateAdjustedForcedMiss(this.verbProps.forcedMissRadius, this.currentTarget.Cell - this.caster.Position);
							if (num > 0.5f)
							{
								int max = GenRadial.NumCellsInRadius(this.verbProps.forcedMissRadius);
								int num2 = Rand.Range(0, max);
								if (num2 > 0)
								{
									if (DebugViewSettings.drawShooting)
									{
										MoteMaker.ThrowText(this.caster.DrawPos, this.caster.Map, "ToForRad", -1f);
									}
									IntVec3 c = this.currentTarget.Cell + GenRadial.RadialPattern[num2];
									ProjectileHitFlags projectileHitFlags;
									if (Rand.Chance(0.5f))
									{
										projectileHitFlags = ProjectileHitFlags.NonTargetWorld;
									}
									else
									{
										projectileHitFlags = ProjectileHitFlags.All;
									}
									if (!this.canHitNonTargetPawnsNow)
									{
										projectileHitFlags &= ~ProjectileHitFlags.NonTargetPawns;
									}
									projectile2.Launch(launcher, drawPos, c, this.currentTarget, projectileHitFlags, equipment, null);
									return true;
								}
							}
						}
						ShotReport shotReport = ShotReport.HitReportFor(this.caster, this, this.currentTarget);
						Thing randomCoverToMissInto = shotReport.GetRandomCoverToMissInto();
						ThingDef targetCoverDef = (randomCoverToMissInto == null) ? null : randomCoverToMissInto.def;
						if (!Rand.Chance(shotReport.ChanceToNotGoWild_IgnoringPosture))
						{
							if (DebugViewSettings.drawShooting)
							{
								MoteMaker.ThrowText(this.caster.DrawPos, this.caster.Map, "ToWild", -1f);
							}
							shootLine.ChangeDestToMissWild();
							ProjectileHitFlags projectileHitFlags2;
							if (Rand.Chance(0.5f))
							{
								projectileHitFlags2 = ProjectileHitFlags.NonTargetWorld;
							}
							else
							{
								projectileHitFlags2 = ProjectileHitFlags.NonTargetWorld;
								if (this.canHitNonTargetPawnsNow)
								{
									projectileHitFlags2 |= ProjectileHitFlags.NonTargetPawns;
								}
							}
							projectile2.Launch(launcher, drawPos, shootLine.Dest, this.currentTarget, projectileHitFlags2, equipment, targetCoverDef);
							result = true;
						}
						else
						{
							if (!Rand.Chance(shotReport.ChanceToNotHitCover))
							{
								if (DebugViewSettings.drawShooting)
								{
									MoteMaker.ThrowText(this.caster.DrawPos, this.caster.Map, "ToCover", -1f);
								}
								if (this.currentTarget.Thing != null && this.currentTarget.Thing.def.category == ThingCategory.Pawn)
								{
									ProjectileHitFlags projectileHitFlags3 = ProjectileHitFlags.NonTargetWorld;
									if (this.canHitNonTargetPawnsNow)
									{
										projectileHitFlags3 |= ProjectileHitFlags.NonTargetPawns;
									}
									projectile2.Launch(launcher, drawPos, randomCoverToMissInto, this.currentTarget, projectileHitFlags3, equipment, targetCoverDef);
									return true;
								}
							}
							if (DebugViewSettings.drawShooting)
							{
								MoteMaker.ThrowText(this.caster.DrawPos, this.caster.Map, "ToHit", -1f);
							}
							ProjectileHitFlags projectileHitFlags4 = ProjectileHitFlags.IntendedTarget;
							if (this.canHitNonTargetPawnsNow)
							{
								projectileHitFlags4 |= ProjectileHitFlags.NonTargetPawns;
							}
							if (!this.currentTarget.HasThing || this.currentTarget.Thing.def.Fillage == FillCategory.Full)
							{
								projectileHitFlags4 |= ProjectileHitFlags.NonTargetWorld;
							}
							if (this.currentTarget.Thing != null)
							{
								projectile2.Launch(launcher, drawPos, this.currentTarget, this.currentTarget, projectileHitFlags4, equipment, targetCoverDef);
							}
							else
							{
								projectile2.Launch(launcher, drawPos, shootLine.Dest, this.currentTarget, projectileHitFlags4, equipment, targetCoverDef);
							}
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06006207 RID: 25095 RVA: 0x001E3104 File Offset: 0x001E1504
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = true;
			ThingDef projectile = this.Projectile;
			float result;
			if (projectile == null)
			{
				result = 0f;
			}
			else
			{
				result = projectile.projectile.explosionRadius;
			}
			return result;
		}

		// Token: 0x06006208 RID: 25096 RVA: 0x001E3140 File Offset: 0x001E1540
		public override bool Available()
		{
			return base.Available() && this.Projectile != null;
		}

		// Token: 0x04004007 RID: 16391
		private const float NoInterceptChanceOnGoWild = 0.5f;
	}
}
