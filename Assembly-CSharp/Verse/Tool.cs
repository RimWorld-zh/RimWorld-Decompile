using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E0D RID: 3597
	public class Tool
	{
		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x06005172 RID: 20850 RVA: 0x0029AD20 File Offset: 0x00299120
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
					result = this.englishLabel;
				}
				return result;
			}
		}

		// Token: 0x06005173 RID: 20851 RVA: 0x0029AD58 File Offset: 0x00299158
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

		// Token: 0x06005174 RID: 20852 RVA: 0x0029ADB8 File Offset: 0x002991B8
		public float AdjustedCooldown(Thing ownerEquipment)
		{
			return this.cooldownTime * ((ownerEquipment != null) ? ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_CooldownMultiplier, true) : 1f);
		}

		// Token: 0x06005175 RID: 20853 RVA: 0x0029ADF0 File Offset: 0x002991F0
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x06005176 RID: 20854 RVA: 0x0029AE0B File Offset: 0x0029920B
		public void PostLoad()
		{
			this.englishLabel = this.label;
		}

		// Token: 0x04003552 RID: 13650
		[NoTranslate]
		public string id;

		// Token: 0x04003553 RID: 13651
		[MustTranslate]
		public string label;

		// Token: 0x04003554 RID: 13652
		public bool labelUsedInLogging = true;

		// Token: 0x04003555 RID: 13653
		public List<ToolCapacityDef> capacities;

		// Token: 0x04003556 RID: 13654
		public float power;

		// Token: 0x04003557 RID: 13655
		public float cooldownTime;

		// Token: 0x04003558 RID: 13656
		public SurpriseAttackProps surpriseAttack;

		// Token: 0x04003559 RID: 13657
		public HediffDef hediff;

		// Token: 0x0400355A RID: 13658
		public float commonality = 1f;

		// Token: 0x0400355B RID: 13659
		public bool alwaysTreatAsWeapon = false;

		// Token: 0x0400355C RID: 13660
		public BodyPartGroupDef linkedBodyPartsGroup;

		// Token: 0x0400355D RID: 13661
		public bool ensureLinkedBodyPartsGroupAlwaysUsable;

		// Token: 0x0400355E RID: 13662
		[Unsaved]
		private string englishLabel;
	}
}
