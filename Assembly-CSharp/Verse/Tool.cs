using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E09 RID: 3593
	public class Tool
	{
		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x06005184 RID: 20868 RVA: 0x0029C2E0 File Offset: 0x0029A6E0
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

		// Token: 0x06005185 RID: 20869 RVA: 0x0029C318 File Offset: 0x0029A718
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

		// Token: 0x06005186 RID: 20870 RVA: 0x0029C378 File Offset: 0x0029A778
		public float AdjustedCooldown(Thing ownerEquipment)
		{
			return this.cooldownTime * ((ownerEquipment != null) ? ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_CooldownMultiplier, true) : 1f);
		}

		// Token: 0x06005187 RID: 20871 RVA: 0x0029C3B0 File Offset: 0x0029A7B0
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x06005188 RID: 20872 RVA: 0x0029C3CB File Offset: 0x0029A7CB
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x04003557 RID: 13655
		[NoTranslate]
		public string id;

		// Token: 0x04003558 RID: 13656
		[MustTranslate]
		public string label;

		// Token: 0x04003559 RID: 13657
		[Unsaved]
		[TranslationHandle]
		public string untranslatedLabel = null;

		// Token: 0x0400355A RID: 13658
		public bool labelUsedInLogging = true;

		// Token: 0x0400355B RID: 13659
		public List<ToolCapacityDef> capacities;

		// Token: 0x0400355C RID: 13660
		public float power;

		// Token: 0x0400355D RID: 13661
		public float cooldownTime;

		// Token: 0x0400355E RID: 13662
		public SurpriseAttackProps surpriseAttack;

		// Token: 0x0400355F RID: 13663
		public HediffDef hediff;

		// Token: 0x04003560 RID: 13664
		public float commonality = 1f;

		// Token: 0x04003561 RID: 13665
		public bool alwaysTreatAsWeapon = false;

		// Token: 0x04003562 RID: 13666
		public BodyPartGroupDef linkedBodyPartsGroup;

		// Token: 0x04003563 RID: 13667
		public bool ensureLinkedBodyPartsGroupAlwaysUsable;
	}
}
