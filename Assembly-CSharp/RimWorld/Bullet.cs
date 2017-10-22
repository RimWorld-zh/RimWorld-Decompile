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
			if (hitThing != null)
			{
				int damageAmountBase = base.def.projectile.damageAmountBase;
				ThingDef equipmentDef = base.equipmentDef;
				DamageDef damageDef = base.def.projectile.damageDef;
				int amount = damageAmountBase;
				Vector3 eulerAngles = this.ExactRotation.eulerAngles;
				DamageInfo dinfo = new DamageInfo(damageDef, amount, eulerAngles.y, base.launcher, null, equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown);
				hitThing.TakeDamage(dinfo);
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
