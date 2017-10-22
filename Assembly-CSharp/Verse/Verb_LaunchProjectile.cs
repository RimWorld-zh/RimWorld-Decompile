using RimWorld;
using UnityEngine;

namespace Verse
{
	public class Verb_LaunchProjectile : Verb
	{
		public virtual ThingDef Projectile
		{
			get
			{
				ThingDef result;
				if (base.ownerEquipment != null)
				{
					CompChangeableProjectile comp = base.ownerEquipment.GetComp<CompChangeableProjectile>();
					if (comp != null && comp.Loaded)
					{
						result = comp.Projectile;
						goto IL_0048;
					}
				}
				result = base.verbProps.defaultProjectile;
				goto IL_0048;
				IL_0048:
				return result;
			}
		}

		protected override bool TryCastShot()
		{
			bool result;
			if (base.currentTarget.HasThing && base.currentTarget.Thing.Map != base.caster.Map)
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
					ShootLine shootLine = default(ShootLine);
					bool flag = base.TryFindShootLineFromTo(base.caster.Position, base.currentTarget, out shootLine);
					if (base.verbProps.stopBurstWithoutLos && !flag)
					{
						result = false;
					}
					else
					{
						if (base.ownerEquipment != null)
						{
							CompChangeableProjectile comp = base.ownerEquipment.GetComp<CompChangeableProjectile>();
							if (comp != null)
							{
								comp.Notify_ProjectileLaunched();
							}
						}
						Thing thing = base.caster;
						Thing thing2 = base.ownerEquipment;
						CompMannable compMannable = base.caster.TryGetComp<CompMannable>();
						if (compMannable != null && compMannable.ManningPawn != null)
						{
							thing = compMannable.ManningPawn;
							thing2 = base.caster;
						}
						Find.BattleLog.Add(new BattleLogEntry_RangedFire(thing as Pawn, (!base.currentTarget.HasThing) ? null : base.currentTarget.Thing, thing2.def, projectile));
						Vector3 drawPos = base.caster.DrawPos;
						Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectile, shootLine.Source, base.caster.Map);
						projectile2.FreeIntercept = (base.canFreeInterceptNow && !projectile2.def.projectile.flyOverhead);
						if (base.verbProps.forcedMissRadius > 0.5)
						{
							float num = (float)(base.currentTarget.Cell - base.caster.Position).LengthHorizontalSquared;
							float num2 = (float)((!(num < 9.0)) ? ((!(num < 25.0)) ? ((!(num < 49.0)) ? (base.verbProps.forcedMissRadius * 1.0) : (base.verbProps.forcedMissRadius * 0.800000011920929)) : (base.verbProps.forcedMissRadius * 0.5)) : 0.0);
							if (num2 > 0.5)
							{
								int max = GenRadial.NumCellsInRadius(base.verbProps.forcedMissRadius);
								int num3 = Rand.Range(0, max);
								if (num3 > 0)
								{
									if (DebugViewSettings.drawShooting)
									{
										MoteMaker.ThrowText(base.caster.DrawPos, base.caster.Map, "ToForRad", -1f);
									}
									IntVec3 c = base.currentTarget.Cell + GenRadial.RadialPattern[num3];
									if (base.currentTarget.HasThing)
									{
										projectile2.ThingToNeverIntercept = base.currentTarget.Thing;
									}
									if (!projectile2.def.projectile.flyOverhead)
									{
										projectile2.InterceptWalls = true;
									}
									projectile2.Launch(thing, drawPos, c, thing2, null);
									result = true;
									goto IL_0550;
								}
							}
						}
						ShotReport shotReport = ShotReport.HitReportFor(base.caster, this, base.currentTarget);
						if (Rand.Value > shotReport.ChanceToNotGoWild_IgnoringPosture)
						{
							if (DebugViewSettings.drawShooting)
							{
								MoteMaker.ThrowText(base.caster.DrawPos, base.caster.Map, "ToWild", -1f);
							}
							shootLine.ChangeDestToMissWild();
							if (base.currentTarget.HasThing)
							{
								projectile2.ThingToNeverIntercept = base.currentTarget.Thing;
							}
							if (!projectile2.def.projectile.flyOverhead)
							{
								projectile2.InterceptWalls = true;
							}
							projectile2.Launch(thing, drawPos, shootLine.Dest, thing2, base.currentTarget.Thing);
							result = true;
						}
						else
						{
							if (Rand.Value > shotReport.ChanceToNotHitCover)
							{
								if (DebugViewSettings.drawShooting)
								{
									MoteMaker.ThrowText(base.caster.DrawPos, base.caster.Map, "ToCover", -1f);
								}
								if (base.currentTarget.Thing != null && base.currentTarget.Thing.def.category == ThingCategory.Pawn)
								{
									Thing randomCoverToMissInto = shotReport.GetRandomCoverToMissInto();
									if (!projectile2.def.projectile.flyOverhead)
									{
										projectile2.InterceptWalls = true;
									}
									projectile2.Launch(thing, drawPos, randomCoverToMissInto, thing2, null);
									result = true;
									goto IL_0550;
								}
							}
							if (DebugViewSettings.drawShooting)
							{
								MoteMaker.ThrowText(base.caster.DrawPos, base.caster.Map, "ToHit", -1f);
							}
							if (!projectile2.def.projectile.flyOverhead)
							{
								projectile2.InterceptWalls = (!base.currentTarget.HasThing || base.currentTarget.Thing.def.Fillage == FillCategory.Full);
							}
							if (base.currentTarget.Thing != null)
							{
								projectile2.Launch(thing, drawPos, base.currentTarget, thing2, null);
							}
							else
							{
								projectile2.Launch(thing, drawPos, shootLine.Dest, thing2, null);
							}
							result = true;
						}
					}
				}
			}
			goto IL_0550;
			IL_0550:
			return result;
		}

		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = true;
			ThingDef projectile = this.Projectile;
			return (float)((projectile != null) ? projectile.projectile.explosionRadius : 0.0);
		}
	}
}
