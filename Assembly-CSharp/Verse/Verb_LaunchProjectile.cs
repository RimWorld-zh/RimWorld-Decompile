using RimWorld;
using UnityEngine;

namespace Verse
{
	public class Verb_LaunchProjectile : Verb
	{
		protected override bool TryCastShot()
		{
			if (base.currentTarget.HasThing && base.currentTarget.Thing.Map != base.caster.Map)
			{
				return false;
			}
			ShootLine shootLine = default(ShootLine);
			bool flag = base.TryFindShootLineFromTo(base.caster.Position, base.currentTarget, out shootLine);
			if (base.verbProps.stopBurstWithoutLos && !flag)
			{
				return false;
			}
			Vector3 drawPos = base.caster.DrawPos;
			Projectile projectile = (Projectile)GenSpawn.Spawn(base.verbProps.projectileDef, shootLine.Source, base.caster.Map);
			projectile.FreeIntercept = (base.canFreeInterceptNow && !projectile.def.projectile.flyOverhead);
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
							projectile.ThingToNeverIntercept = base.currentTarget.Thing;
						}
						if (!projectile.def.projectile.flyOverhead)
						{
							projectile.InterceptWalls = true;
						}
						projectile.Launch(base.caster, drawPos, c, base.ownerEquipment);
						return true;
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
					projectile.ThingToNeverIntercept = base.currentTarget.Thing;
				}
				if (!projectile.def.projectile.flyOverhead)
				{
					projectile.InterceptWalls = true;
				}
				projectile.Launch(base.caster, drawPos, shootLine.Dest, base.ownerEquipment);
				return true;
			}
			if (Rand.Value > shotReport.ChanceToNotHitCover)
			{
				if (DebugViewSettings.drawShooting)
				{
					MoteMaker.ThrowText(base.caster.DrawPos, base.caster.Map, "ToCover", -1f);
				}
				if (base.currentTarget.Thing != null && base.currentTarget.Thing.def.category == ThingCategory.Pawn)
				{
					Thing randomCoverToMissInto = shotReport.GetRandomCoverToMissInto();
					if (!projectile.def.projectile.flyOverhead)
					{
						projectile.InterceptWalls = true;
					}
					projectile.Launch(base.caster, drawPos, randomCoverToMissInto, base.ownerEquipment);
					return true;
				}
			}
			if (DebugViewSettings.drawShooting)
			{
				MoteMaker.ThrowText(base.caster.DrawPos, base.caster.Map, "ToHit", -1f);
			}
			if (!projectile.def.projectile.flyOverhead)
			{
				projectile.InterceptWalls = (!base.currentTarget.HasThing || base.currentTarget.Thing.def.Fillage == FillCategory.Full);
			}
			if (base.currentTarget.Thing != null)
			{
				projectile.Launch(base.caster, drawPos, base.currentTarget, base.ownerEquipment);
			}
			else
			{
				projectile.Launch(base.caster, drawPos, shootLine.Dest, base.ownerEquipment);
			}
			return true;
		}

		public override float HighlightFieldRadiusAroundTarget()
		{
			return base.verbProps.projectileDef.projectile.explosionRadius;
		}
	}
}
