using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	public class Tool
	{
		[NoTranslate]
		public string id;

		[MustTranslate]
		public string label;

		[TranslationHandle]
		[Unsaved]
		public string untranslatedLabel = null;

		public bool labelUsedInLogging = true;

		public List<ToolCapacityDef> capacities;

		public float power;

		public float cooldownTime;

		public SurpriseAttackProps surpriseAttack;

		public HediffDef hediff;

		public float commonality = 1f;

		public bool alwaysTreatAsWeapon = false;

		public BodyPartGroupDef linkedBodyPartsGroup;

		public bool ensureLinkedBodyPartsGroupAlwaysUsable;

		public Tool()
		{
		}

		public string Id
		{
			get
			{
				string result;
				if (!this.id.NullOrEmpty())
				{
					result = this.id;
				}
				else
				{
					result = this.untranslatedLabel;
				}
				return result;
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
			return this.cooldownTime * ((ownerEquipment != null) ? ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_CooldownMultiplier, true) : 1f);
		}

		public override string ToString()
		{
			return this.label;
		}

		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}
	}
}
