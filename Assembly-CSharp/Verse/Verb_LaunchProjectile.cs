using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public class Verb_LaunchProjectile : Verb
	{
		public Verb_LaunchProjectile()
		{
		}

		public virtual ThingDef Projectile
		{
			get
			{
				if (base.EquipmentSource != null)
				{
					CompChangeableProjectile comp = base.EquipmentSource.GetComp<CompChangeableProjectile>();
					if (comp != null && comp.Loaded)
					{
						return comp.Projectile;
					}
				}
				return this.verbProps.defaultProjectile;
			}
		}

		public override void WarmupComplete()
		{
			base.WarmupComplete();
			Find.BattleLog.Add(new BattleLogEntry_RangedFire(this.caster, (!this.currentTarget.HasThing) ? null : this.currentTarget.Thing, (base.EquipmentSource == null) ? null : base.EquipmentSource.def, this.Projectile, this.ShotsPerBurst > 1));
		}

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
						if (base.EquipmentSource != null)
						{
							CompChangeableProjectile comp = base.EquipmentSource.GetComp<CompChangeableProjectile>();
							if (comp != null)
							{
								comp.Notify_ProjectileLaunched();
							}
						}
						Thing launcher = this.caster;
						Thing equipment = base.EquipmentSource;
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
						if (!Rand.Chance(shotReport.AimOnTargetChance_IgnoringPosture))
						{
							shootLine.ChangeDestToMissWild(shotReport.AimOnTargetChance_StandardTarget);
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
						else if (this.currentTarget.Thing != null && this.currentTarget.Thing.def.category == ThingCategory.Pawn && !Rand.Chance(shotReport.PassCoverChance))
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

		private void ThrowDebugText(string text)
		{
			if (DebugViewSettings.drawShooting)
			{
				MoteMaker.ThrowText(this.caster.DrawPos, this.caster.Map, text, -1f);
			}
		}

		private void ThrowDebugText(string text, IntVec3 c)
		{
			if (DebugViewSettings.drawShooting)
			{
				MoteMaker.ThrowText(c.ToVector3Shifted(), this.caster.Map, text, -1f);
			}
		}

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

		public override bool Available()
		{
			return base.Available() && this.Projectile != null;
		}
	}
}
