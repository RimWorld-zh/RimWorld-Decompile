using RimWorld;
using System.Collections.Generic;

namespace Verse
{
	public class Tool
	{
		public string id;

		public string label;

		public List<ToolCapacityDef> capacities;

		public float power;

		public float cooldownTime;

		public SurpriseAttackProps surpriseAttack;

		public float commonality = 1f;

		public BodyPartGroupDef linkedBodyPartsGroup;

		public string Id
		{
			get
			{
				return this.id.NullOrEmpty() ? this.label : this.id;
			}
		}

		public float AdjustedMeleeDamageAmount(Thing ownerEquipment, DamageDef damageDef)
		{
			float num = this.power;
			if (ownerEquipment != null)
			{
				num *= ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_DamageMultiplier, true);
				if (ownerEquipment.Stuff != null && damageDef != null)
				{
					num *= ownerEquipment.Stuff.GetStatValueAbstract(damageDef.armorCategory.multStat, null);
				}
			}
			return num;
		}

		public float AdjustedCooldown(Thing ownerEquipment)
		{
			return (float)(this.cooldownTime * ((ownerEquipment != null) ? ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_CooldownMultiplier, true) : 1.0));
		}

		public override string ToString()
		{
			return this.label;
		}
	}
}
