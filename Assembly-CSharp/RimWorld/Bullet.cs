using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class Bullet : Projectile
	{
		protected override void Impact(Thing hitThing)
		{
			Map map = base.Map;
			base.Impact(hitThing);
			BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(base.launcher, hitThing, base.intendedTarget, base.equipmentDef, base.def);
			Find.BattleLog.Add(battleLogEntry_RangedImpact);
			if (hitThing != null)
			{
				int damageAmountBase = base.def.projectile.damageAmountBase;
				DamageDef damageDef = base.def.projectile.damageDef;
				int amount = damageAmountBase;
				Vector3 eulerAngles = this.ExactRotation.eulerAngles;
				float y = eulerAngles.y;
				Thing launcher = base.launcher;
				ThingDef equipmentDef = base.equipmentDef;
				DamageInfo dinfo = new DamageInfo(damageDef, amount, y, launcher, null, equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown);
				hitThing.TakeDamage(dinfo).InsertIntoLog(battleLogEntry_RangedImpact);
			}
			else
			{
				SoundDefOf.BulletImpactGround.PlayOneShot(new TargetInfo(base.Position, map, false));
				MoteMaker.MakeStaticMote(this.ExactPosition, map, ThingDefOf.Mote_ShotHit_Dirt, 1f);
				if (base.Position.GetTerrain(map).takeSplashes)
				{
					MoteMaker.MakeWaterSplash(this.ExactPosition, map, (float)(Mathf.Sqrt((float)base.def.projectile.damageAmountBase) * 1.0), 4f);
				}
			}
		}
	}
}
