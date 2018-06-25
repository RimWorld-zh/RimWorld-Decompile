using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006EB RID: 1771
	public class SmokepopBelt : Apparel
	{
		// Token: 0x0400157E RID: 5502
		private float ApparelScorePerBeltRadius = 0.046f;

		// Token: 0x06002693 RID: 9875 RVA: 0x0014A8B8 File Offset: 0x00148CB8
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

		// Token: 0x06002694 RID: 9876 RVA: 0x0014A984 File Offset: 0x00148D84
		public override float GetSpecialApparelScoreOffset()
		{
			return this.GetStatValue(StatDefOf.SmokepopBeltRadius, true) * this.ApparelScorePerBeltRadius;
		}
	}
}
