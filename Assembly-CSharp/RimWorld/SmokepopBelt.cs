using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006E9 RID: 1769
	public class SmokepopBelt : Apparel
	{
		// Token: 0x0400157A RID: 5498
		private float ApparelScorePerBeltRadius = 0.046f;

		// Token: 0x06002690 RID: 9872 RVA: 0x0014A508 File Offset: 0x00148908
		public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
		{
			if (!dinfo.Def.isExplosive && dinfo.Def.harmsHealth && dinfo.Def.externalViolence && dinfo.Weapon != null && dinfo.Weapon.IsRangedWeapon)
			{
				IntVec3 position = base.Wearer.Position;
				Map map = base.Wearer.Map;
				float statValue = this.GetStatValue(StatDefOf.SmokepopBeltRadius, true);
				DamageDef smoke = DamageDefOf.Smoke;
				Thing instigator = null;
				ThingDef gas_Smoke = ThingDefOf.Gas_Smoke;
				GenExplosion.DoExplosion(position, map, statValue, smoke, instigator, -1, null, null, null, null, gas_Smoke, 1f, 1, false, null, 0f, 1, 0f, false);
				this.Destroy(DestroyMode.Vanish);
			}
			return false;
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x0014A5D4 File Offset: 0x001489D4
		public override float GetSpecialApparelScoreOffset()
		{
			return this.GetStatValue(StatDefOf.SmokepopBeltRadius, true) * this.ApparelScorePerBeltRadius;
		}
	}
}
