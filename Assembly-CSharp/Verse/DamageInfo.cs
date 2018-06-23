using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EE5 RID: 3813
	public struct DamageInfo
	{
		// Token: 0x04003C86 RID: 15494
		private DamageDef defInt;

		// Token: 0x04003C87 RID: 15495
		private float amountInt;

		// Token: 0x04003C88 RID: 15496
		private float angleInt;

		// Token: 0x04003C89 RID: 15497
		private Thing instigatorInt;

		// Token: 0x04003C8A RID: 15498
		private DamageInfo.SourceCategory categoryInt;

		// Token: 0x04003C8B RID: 15499
		public Thing intendedTargetInt;

		// Token: 0x04003C8C RID: 15500
		private BodyPartRecord hitPartInt;

		// Token: 0x04003C8D RID: 15501
		private BodyPartHeight heightInt;

		// Token: 0x04003C8E RID: 15502
		private BodyPartDepth depthInt;

		// Token: 0x04003C8F RID: 15503
		private ThingDef weaponInt;

		// Token: 0x04003C90 RID: 15504
		private BodyPartGroupDef weaponBodyPartGroupInt;

		// Token: 0x04003C91 RID: 15505
		private HediffDef weaponHediffInt;

		// Token: 0x04003C92 RID: 15506
		private bool instantPermanentInjuryInt;

		// Token: 0x04003C93 RID: 15507
		private bool allowDamagePropagationInt;

		// Token: 0x06005A82 RID: 23170 RVA: 0x002E71BC File Offset: 0x002E55BC
		public DamageInfo(DamageDef def, float amount, float angle = -1f, Thing instigator = null, BodyPartRecord hitPart = null, ThingDef weapon = null, DamageInfo.SourceCategory category = DamageInfo.SourceCategory.ThingOrUnknown, Thing intendedTarget = null)
		{
			this.defInt = def;
			this.amountInt = amount;
			if (angle < 0f)
			{
				this.angleInt = (float)Rand.RangeInclusive(0, 359);
			}
			else
			{
				this.angleInt = angle;
			}
			this.instigatorInt = instigator;
			this.categoryInt = category;
			this.hitPartInt = hitPart;
			this.heightInt = BodyPartHeight.Undefined;
			this.depthInt = BodyPartDepth.Undefined;
			this.weaponInt = weapon;
			this.weaponBodyPartGroupInt = null;
			this.weaponHediffInt = null;
			this.instantPermanentInjuryInt = false;
			this.allowDamagePropagationInt = true;
			this.intendedTargetInt = intendedTarget;
		}

		// Token: 0x06005A83 RID: 23171 RVA: 0x002E7254 File Offset: 0x002E5654
		public DamageInfo(DamageInfo cloneSource)
		{
			this.defInt = cloneSource.defInt;
			this.amountInt = cloneSource.amountInt;
			this.angleInt = cloneSource.angleInt;
			this.instigatorInt = cloneSource.instigatorInt;
			this.categoryInt = cloneSource.categoryInt;
			this.hitPartInt = cloneSource.hitPartInt;
			this.heightInt = cloneSource.heightInt;
			this.depthInt = cloneSource.depthInt;
			this.weaponInt = cloneSource.weaponInt;
			this.weaponBodyPartGroupInt = cloneSource.weaponBodyPartGroupInt;
			this.weaponHediffInt = cloneSource.weaponHediffInt;
			this.instantPermanentInjuryInt = cloneSource.instantPermanentInjuryInt;
			this.allowDamagePropagationInt = cloneSource.allowDamagePropagationInt;
			this.intendedTargetInt = cloneSource.intendedTargetInt;
		}

		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x06005A84 RID: 23172 RVA: 0x002E7318 File Offset: 0x002E5718
		// (set) Token: 0x06005A85 RID: 23173 RVA: 0x002E7333 File Offset: 0x002E5733
		public DamageDef Def
		{
			get
			{
				return this.defInt;
			}
			set
			{
				this.defInt = value;
			}
		}

		// Token: 0x17000E45 RID: 3653
		// (get) Token: 0x06005A86 RID: 23174 RVA: 0x002E7340 File Offset: 0x002E5740
		public float Amount
		{
			get
			{
				float result;
				if (!DebugSettings.enableDamage)
				{
					result = 0f;
				}
				else
				{
					result = this.amountInt;
				}
				return result;
			}
		}

		// Token: 0x17000E46 RID: 3654
		// (get) Token: 0x06005A87 RID: 23175 RVA: 0x002E7370 File Offset: 0x002E5770
		public Thing Instigator
		{
			get
			{
				return this.instigatorInt;
			}
		}

		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x06005A88 RID: 23176 RVA: 0x002E738C File Offset: 0x002E578C
		public DamageInfo.SourceCategory Category
		{
			get
			{
				return this.categoryInt;
			}
		}

		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x06005A89 RID: 23177 RVA: 0x002E73A8 File Offset: 0x002E57A8
		public Thing IntendedTarget
		{
			get
			{
				return this.intendedTargetInt;
			}
		}

		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x06005A8A RID: 23178 RVA: 0x002E73C4 File Offset: 0x002E57C4
		public float Angle
		{
			get
			{
				return this.angleInt;
			}
		}

		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x06005A8B RID: 23179 RVA: 0x002E73E0 File Offset: 0x002E57E0
		public BodyPartRecord HitPart
		{
			get
			{
				return this.hitPartInt;
			}
		}

		// Token: 0x17000E4B RID: 3659
		// (get) Token: 0x06005A8C RID: 23180 RVA: 0x002E73FC File Offset: 0x002E57FC
		public BodyPartHeight Height
		{
			get
			{
				return this.heightInt;
			}
		}

		// Token: 0x17000E4C RID: 3660
		// (get) Token: 0x06005A8D RID: 23181 RVA: 0x002E7418 File Offset: 0x002E5818
		public BodyPartDepth Depth
		{
			get
			{
				return this.depthInt;
			}
		}

		// Token: 0x17000E4D RID: 3661
		// (get) Token: 0x06005A8E RID: 23182 RVA: 0x002E7434 File Offset: 0x002E5834
		public ThingDef Weapon
		{
			get
			{
				return this.weaponInt;
			}
		}

		// Token: 0x17000E4E RID: 3662
		// (get) Token: 0x06005A8F RID: 23183 RVA: 0x002E7450 File Offset: 0x002E5850
		public BodyPartGroupDef WeaponBodyPartGroup
		{
			get
			{
				return this.weaponBodyPartGroupInt;
			}
		}

		// Token: 0x17000E4F RID: 3663
		// (get) Token: 0x06005A90 RID: 23184 RVA: 0x002E746C File Offset: 0x002E586C
		public HediffDef WeaponLinkedHediff
		{
			get
			{
				return this.weaponHediffInt;
			}
		}

		// Token: 0x17000E50 RID: 3664
		// (get) Token: 0x06005A91 RID: 23185 RVA: 0x002E7488 File Offset: 0x002E5888
		public bool InstantPermanentInjury
		{
			get
			{
				return this.instantPermanentInjuryInt;
			}
		}

		// Token: 0x17000E51 RID: 3665
		// (get) Token: 0x06005A92 RID: 23186 RVA: 0x002E74A4 File Offset: 0x002E58A4
		public bool AllowDamagePropagation
		{
			get
			{
				return !this.InstantPermanentInjury && this.allowDamagePropagationInt;
			}
		}

		// Token: 0x06005A93 RID: 23187 RVA: 0x002E74D1 File Offset: 0x002E58D1
		public void SetAmount(float newAmount)
		{
			this.amountInt = newAmount;
		}

		// Token: 0x06005A94 RID: 23188 RVA: 0x002E74DB File Offset: 0x002E58DB
		public void SetBodyRegion(BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined)
		{
			this.heightInt = height;
			this.depthInt = depth;
		}

		// Token: 0x06005A95 RID: 23189 RVA: 0x002E74EC File Offset: 0x002E58EC
		public void SetHitPart(BodyPartRecord forceHitPart)
		{
			this.hitPartInt = forceHitPart;
		}

		// Token: 0x06005A96 RID: 23190 RVA: 0x002E74F6 File Offset: 0x002E58F6
		public void SetInstantPermanentInjury(bool val)
		{
			this.instantPermanentInjuryInt = val;
		}

		// Token: 0x06005A97 RID: 23191 RVA: 0x002E7500 File Offset: 0x002E5900
		public void SetWeaponBodyPartGroup(BodyPartGroupDef gr)
		{
			this.weaponBodyPartGroupInt = gr;
		}

		// Token: 0x06005A98 RID: 23192 RVA: 0x002E750A File Offset: 0x002E590A
		public void SetWeaponHediff(HediffDef hd)
		{
			this.weaponHediffInt = hd;
		}

		// Token: 0x06005A99 RID: 23193 RVA: 0x002E7514 File Offset: 0x002E5914
		public void SetAllowDamagePropagation(bool val)
		{
			this.allowDamagePropagationInt = val;
		}

		// Token: 0x06005A9A RID: 23194 RVA: 0x002E7520 File Offset: 0x002E5920
		public void SetAngle(Vector3 vec)
		{
			if (vec.x != 0f || vec.z != 0f)
			{
				this.angleInt = Quaternion.LookRotation(vec).eulerAngles.y;
			}
			else
			{
				this.angleInt = (float)Rand.RangeInclusive(0, 359);
			}
		}

		// Token: 0x06005A9B RID: 23195 RVA: 0x002E7584 File Offset: 0x002E5984
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(def=",
				this.defInt,
				", amount= ",
				this.amountInt,
				", instigator=",
				(this.instigatorInt == null) ? this.categoryInt.ToString() : this.instigatorInt.ToString(),
				", angle=",
				this.angleInt.ToString("F1"),
				")"
			});
		}

		// Token: 0x02000EE6 RID: 3814
		public enum SourceCategory
		{
			// Token: 0x04003C95 RID: 15509
			ThingOrUnknown,
			// Token: 0x04003C96 RID: 15510
			Collapse
		}
	}
}
