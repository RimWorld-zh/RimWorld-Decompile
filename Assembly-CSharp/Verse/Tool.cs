using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E0C RID: 3596
	public class Tool
	{
		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x06005170 RID: 20848 RVA: 0x0029AD00 File Offset: 0x00299100
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

		// Token: 0x06005171 RID: 20849 RVA: 0x0029AD38 File Offset: 0x00299138
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

		// Token: 0x06005172 RID: 20850 RVA: 0x0029AD98 File Offset: 0x00299198
		public float AdjustedCooldown(Thing ownerEquipment)
		{
			return this.cooldownTime * ((ownerEquipment != null) ? ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_CooldownMultiplier, true) : 1f);
		}

		// Token: 0x06005173 RID: 20851 RVA: 0x0029ADD0 File Offset: 0x002991D0
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x06005174 RID: 20852 RVA: 0x0029ADEB File Offset: 0x002991EB
		public void PostLoad()
		{
			this.englishLabel = this.label;
		}

		// Token: 0x04003550 RID: 13648
		[NoTranslate]
		public string id;

		// Token: 0x04003551 RID: 13649
		[MustTranslate]
		public string label;

		// Token: 0x04003552 RID: 13650
		public bool labelUsedInLogging = true;

		// Token: 0x04003553 RID: 13651
		public List<ToolCapacityDef> capacities;

		// Token: 0x04003554 RID: 13652
		public float power;

		// Token: 0x04003555 RID: 13653
		public float cooldownTime;

		// Token: 0x04003556 RID: 13654
		public SurpriseAttackProps surpriseAttack;

		// Token: 0x04003557 RID: 13655
		public HediffDef hediff;

		// Token: 0x04003558 RID: 13656
		public float commonality = 1f;

		// Token: 0x04003559 RID: 13657
		public bool alwaysTreatAsWeapon = false;

		// Token: 0x0400355A RID: 13658
		public BodyPartGroupDef linkedBodyPartsGroup;

		// Token: 0x0400355B RID: 13659
		public bool ensureLinkedBodyPartsGroupAlwaysUsable;

		// Token: 0x0400355C RID: 13660
		[Unsaved]
		private string englishLabel;
	}
}
