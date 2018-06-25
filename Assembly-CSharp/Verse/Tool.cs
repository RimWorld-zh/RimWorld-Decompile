using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E0C RID: 3596
	public class Tool
	{
		// Token: 0x0400355E RID: 13662
		[NoTranslate]
		public string id;

		// Token: 0x0400355F RID: 13663
		[MustTranslate]
		public string label;

		// Token: 0x04003560 RID: 13664
		[Unsaved]
		[TranslationHandle]
		public string untranslatedLabel = null;

		// Token: 0x04003561 RID: 13665
		public bool labelUsedInLogging = true;

		// Token: 0x04003562 RID: 13666
		public List<ToolCapacityDef> capacities;

		// Token: 0x04003563 RID: 13667
		public float power;

		// Token: 0x04003564 RID: 13668
		public float cooldownTime;

		// Token: 0x04003565 RID: 13669
		public SurpriseAttackProps surpriseAttack;

		// Token: 0x04003566 RID: 13670
		public HediffDef hediff;

		// Token: 0x04003567 RID: 13671
		public float commonality = 1f;

		// Token: 0x04003568 RID: 13672
		public bool alwaysTreatAsWeapon = false;

		// Token: 0x04003569 RID: 13673
		public BodyPartGroupDef linkedBodyPartsGroup;

		// Token: 0x0400356A RID: 13674
		public bool ensureLinkedBodyPartsGroupAlwaysUsable;

		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x06005188 RID: 20872 RVA: 0x0029C6EC File Offset: 0x0029AAEC
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

		// Token: 0x06005189 RID: 20873 RVA: 0x0029C724 File Offset: 0x0029AB24
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

		// Token: 0x0600518A RID: 20874 RVA: 0x0029C784 File Offset: 0x0029AB84
		public float AdjustedCooldown(Thing ownerEquipment)
		{
			return this.cooldownTime * ((ownerEquipment != null) ? ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_CooldownMultiplier, true) : 1f);
		}

		// Token: 0x0600518B RID: 20875 RVA: 0x0029C7BC File Offset: 0x0029ABBC
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x0600518C RID: 20876 RVA: 0x0029C7D7 File Offset: 0x0029ABD7
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}
	}
}
