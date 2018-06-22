using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FD4 RID: 4052
	public class Verb_LaunchProjectile : Verb
	{
		// Token: 0x17000FEA RID: 4074
		// (get) Token: 0x0600622B RID: 25131 RVA: 0x001E2E50 File Offset: 0x001E1250
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

		// Token: 0x0600622C RID: 25132 RVA: 0x001E2EA8 File Offset: 0x001E12A8
		public override void WarmupComplete()
		{
			base.WarmupComplete();
			Find.BattleLog.Add(new BattleLogEntry_RangedFire(this.caster, (!this.currentTarget.HasThing) ? null : this.currentTarget.Thing, (this.ownerEquipment == null) ? null : this.ownerEquipment.def, this.Projectile, this.ShotsPerBurst > 1));
		}

		// Token: 0x0600622D RID: 25133 RVA: 0x001E2F20 File Offset: 0x001E1320
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
								int max = GenRadial.NumCellsInRadius(num);
								int num2 = Rand.Range(0, max);
								if (num2 > 0)
								{
									IntVec3 c = this.currentTarget.Cell + GenRadial.RadialPattern[num2];
									this.ThrowDebugText("ToRadius");
									this.ThrowDebugText("Rad\nDest", c);
									ProjectileHitFlags projectileHitFlags = ProjectileHitFlags.NonTargetWorld;
									if (Rand.Chance(0.5f))
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
							shootLine.ChangeDestToMissWild();
							this.ThrowDebugText("ToWild" + ((!this.canHitNonTargetPawnsNow) ? "" : "\nchntp"));
							this.ThrowDebugText("Wild\nDest", shootLine.Dest);
							ProjectileHitFlags projectileHitFlags2 = ProjectileHitFlags.NonTargetWorld;
							if (Rand.Chance(0.5f) && this.canHitNonTargetPawnsNow)
							{
								projectileHitFlags2 |= ProjectileHitFlags.NonTargetPawns;
							}
							projectile2.Launch(launcher, drawPos, shootLine.Dest, this.currentTarget, projectileHitFlags2, equipment, targetCoverDef);
							result = true;
						}
						else if (this.currentTarget.Thing != null && this.currentTarget.Thing.def.category == ThingCategory.Pawn && !Rand.Chance(shotReport.ChanceToNotHitCover))
						{
							this.ThrowDebugText("ToCover" + ((!this.canHitNonTargetPawnsNow) ? "" : "\nchntp"));
							this.ThrowDebugText("Cover\nDest", randomCoverToMissInto.Position);
							ProjectileHitFlags projectileHitFlags3 = ProjectileHitFlags.NonTargetWorld;
							if (this.canHitNonTargetPawnsNow)
							{
								projectileHitFlags3 |= ProjectileHitFlags.NonTargetPawns;
							}
							projectile2.Launch(launcher, drawPos, randomCoverToMissInto, this.currentTarget, projectileHitFlags3, equipment, targetCoverDef);
							result = true;
						}
						else
						{
							ProjectileHitFlags projectileHitFlags4 = ProjectileHitFlags.IntendedTarget;
							if (this.canHitNonTargetPawnsNow)
							{
								projectileHitFlags4 |= ProjectileHitFlags.NonTargetPawns;
							}
							if (!this.currentTarget.HasThing || this.currentTarget.Thing.def.Fillage == FillCategory.Full)
							{
								projectileHitFlags4 |= ProjectileHitFlags.NonTargetWorld;
							}
							this.ThrowDebugText("ToHit" + ((!this.canHitNonTargetPawnsNow) ? "" : "\nchntp"));
							if (this.currentTarget.Thing != null)
							{
								projectile2.Launch(launcher, drawPos, this.currentTarget, this.currentTarget, projectileHitFlags4, equipment, targetCoverDef);
								this.ThrowDebugText("Hit\nDest", this.currentTarget.Cell);
							}
							else
							{
								projectile2.Launch(launcher, drawPos, shootLine.Dest, this.currentTarget, projectileHitFlags4, equipment, targetCoverDef);
								this.ThrowDebugText("Hit\nDest", shootLine.Dest);
							}
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600622E RID: 25134 RVA: 0x001E33BF File Offset: 0x001E17BF
		private void ThrowDebugText(string text)
		{
			if (DebugViewSettings.drawShooting)
			{
				MoteMaker.ThrowText(this.caster.DrawPos, this.caster.Map, text, -1f);
			}
		}

		// Token: 0x0600622F RID: 25135 RVA: 0x001E33ED File Offset: 0x001E17ED
		private void ThrowDebugText(string text, IntVec3 c)
		{
			if (DebugViewSettings.drawShooting)
			{
				MoteMaker.ThrowText(c.ToVector3Shifted(), this.caster.Map, text, -1f);
			}
		}

		// Token: 0x06006230 RID: 25136 RVA: 0x001E3418 File Offset: 0x001E1818
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

		// Token: 0x06006231 RID: 25137 RVA: 0x001E3454 File Offset: 0x001E1854
		public override bool Available()
		{
			return base.Available() && this.Projectile != null;
		}
	}
}
