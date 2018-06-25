using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class Bullet : Projectile
	{
		public Bullet()
		{
		}

		protected override void Impact(Thing hitThing)
		{
			Map map = base.Map;
			base.Impact(hitThing);
			BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(this.launcher, hitThing, this.intendedTarget.Thing, this.equipmentDef, this.def, this.targetCoverDef);
			Find.BattleLog.Add(battleLogEntry_RangedImpact);
			if (hitThing != null)
			{
				int damageAmount = this.def.projectile.DamageAmount;
				DamageDef damageDef = this.def.projectile.damageDef;
				float amount = (float)damageAmount;
				float y = this.ExactRotation.eulerAngles.y;
				Thing launcher = this.launcher;
				ThingDef equipmentDef = this.equipmentDef;
				DamageInfo dinfo = new DamageInfo(damageDef, amount, y, launcher, null, equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown, this.intendedTarget.Thing);
				hitThing.TakeDamage(dinfo).AssociateWithLog(battleLogEntry_RangedImpact);
				Pawn pawn = hitThing as Pawn;
				if (pawn != null && pawn.stances != null && pawn.BodySize <= this.def.projectile.stoppingPower + 0.001f)
				{
					pawn.stances.StaggerFor(95);
				}
			}
			else
			{
				SoundDefOf.BulletImpact_Ground.PlayOneShot(new TargetInfo(base.Position, map, false));
				MoteMaker.MakeStaticMote(this.ExactPosition, map, ThingDefOf.Mote_ShotHit_Dirt, 1f);
				if (base.Position.GetTerrain(map).takeSplashes)
				{
					MoteMaker.MakeWaterSplash(this.ExactPosition, map, Mathf.Sqrt((float)this.def.projectile.DamageAmount) * 1f, 4f);
				}
			}
		}
	}
}
