using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006ED RID: 1773
	public class SmokepopBelt : Apparel
	{
		// Token: 0x06002698 RID: 9880 RVA: 0x0014A364 File Offset: 0x00148764
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

		// Token: 0x06002699 RID: 9881 RVA: 0x0014A430 File Offset: 0x00148830
		public override float GetSpecialApparelScoreOffset()
		{
			return this.GetStatValue(StatDefOf.SmokepopBeltRadius, true) * this.ApparelScorePerBeltRadius;
		}

		// Token: 0x0400157C RID: 5500
		private float ApparelScorePerBeltRadius = 0.046f;
	}
}
