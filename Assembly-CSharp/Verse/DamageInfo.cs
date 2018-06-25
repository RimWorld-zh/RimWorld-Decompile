using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EE8 RID: 3816
	public struct DamageInfo
	{
		// Token: 0x04003C8E RID: 15502
		private DamageDef defInt;

		// Token: 0x04003C8F RID: 15503
		private float amountInt;

		// Token: 0x04003C90 RID: 15504
		private float angleInt;

		// Token: 0x04003C91 RID: 15505
		private Thing instigatorInt;

		// Token: 0x04003C92 RID: 15506
		private DamageInfo.SourceCategory categoryInt;

		// Token: 0x04003C93 RID: 15507
		public Thing intendedTargetInt;

		// Token: 0x04003C94 RID: 15508
		private BodyPartRecord hitPartInt;

		// Token: 0x04003C95 RID: 15509
		private BodyPartHeight heightInt;

		// Token: 0x04003C96 RID: 15510
		private BodyPartDepth depthInt;

		// Token: 0x04003C97 RID: 15511
		private ThingDef weaponInt;

		// Token: 0x04003C98 RID: 15512
		private BodyPartGroupDef weaponBodyPartGroupInt;

		// Token: 0x04003C99 RID: 15513
		private HediffDef weaponHediffInt;

		// Token: 0x04003C9A RID: 15514
		private bool instantPermanentInjuryInt;

		// Token: 0x04003C9B RID: 15515
		private bool allowDamagePropagationInt;

		// Token: 0x06005A85 RID: 23173 RVA: 0x002E74FC File Offset: 0x002E58FC
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

		// Token: 0x06005A86 RID: 23174 RVA: 0x002E7594 File Offset: 0x002E5994
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

		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x06005A87 RID: 23175 RVA: 0x002E7658 File Offset: 0x002E5A58
		// (set) Token: 0x06005A88 RID: 23176 RVA: 0x002E7673 File Offset: 0x002E5A73
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

		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x06005A89 RID: 23177 RVA: 0x002E7680 File Offset: 0x002E5A80
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

		// Token: 0x17000E45 RID: 3653
		// (get) Token: 0x06005A8A RID: 23178 RVA: 0x002E76B0 File Offset: 0x002E5AB0
		public Thing Instigator
		{
			get
			{
				return this.instigatorInt;
			}
		}

		// Token: 0x17000E46 RID: 3654
		// (get) Token: 0x06005A8B RID: 23179 RVA: 0x002E76CC File Offset: 0x002E5ACC
		public DamageInfo.SourceCategory Category
		{
			get
			{
				return this.categoryInt;
			}
		}

		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x06005A8C RID: 23180 RVA: 0x002E76E8 File Offset: 0x002E5AE8
		public Thing IntendedTarget
		{
			get
			{
				return this.intendedTargetInt;
			}
		}

		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x06005A8D RID: 23181 RVA: 0x002E7704 File Offset: 0x002E5B04
		public float Angle
		{
			get
			{
				return this.angleInt;
			}
		}

		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x06005A8E RID: 23182 RVA: 0x002E7720 File Offset: 0x002E5B20
		public BodyPartRecord HitPart
		{
			get
			{
				return this.hitPartInt;
			}
		}

		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x06005A8F RID: 23183 RVA: 0x002E773C File Offset: 0x002E5B3C
		public BodyPartHeight Height
		{
			get
			{
				return this.heightInt;
			}
		}

		// Token: 0x17000E4B RID: 3659
		// (get) Token: 0x06005A90 RID: 23184 RVA: 0x002E7758 File Offset: 0x002E5B58
		public BodyPartDepth Depth
		{
			get
			{
				return this.depthInt;
			}
		}

		// Token: 0x17000E4C RID: 3660
		// (get) Token: 0x06005A91 RID: 23185 RVA: 0x002E7774 File Offset: 0x002E5B74
		public ThingDef Weapon
		{
			get
			{
				return this.weaponInt;
			}
		}

		// Token: 0x17000E4D RID: 3661
		// (get) Token: 0x06005A92 RID: 23186 RVA: 0x002E7790 File Offset: 0x002E5B90
		public BodyPartGroupDef WeaponBodyPartGroup
		{
			get
			{
				return this.weaponBodyPartGroupInt;
			}
		}

		// Token: 0x17000E4E RID: 3662
		// (get) Token: 0x06005A93 RID: 23187 RVA: 0x002E77AC File Offset: 0x002E5BAC
		public HediffDef WeaponLinkedHediff
		{
			get
			{
				return this.weaponHediffInt;
			}
		}

		// Token: 0x17000E4F RID: 3663
		// (get) Token: 0x06005A94 RID: 23188 RVA: 0x002E77C8 File Offset: 0x002E5BC8
		public bool InstantPermanentInjury
		{
			get
			{
				return this.instantPermanentInjuryInt;
			}
		}

		// Token: 0x17000E50 RID: 3664
		// (get) Token: 0x06005A95 RID: 23189 RVA: 0x002E77E4 File Offset: 0x002E5BE4
		public bool AllowDamagePropagation
		{
			get
			{
				return !this.InstantPermanentInjury && this.allowDamagePropagationInt;
			}
		}

		// Token: 0x06005A96 RID: 23190 RVA: 0x002E7811 File Offset: 0x002E5C11
		public void SetAmount(float newAmount)
		{
			this.amountInt = newAmount;
		}

		// Token: 0x06005A97 RID: 23191 RVA: 0x002E781B File Offset: 0x002E5C1B
		public void SetBodyRegion(BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined)
		{
			this.heightInt = height;
			this.depthInt = depth;
		}

		// Token: 0x06005A98 RID: 23192 RVA: 0x002E782C File Offset: 0x002E5C2C
		public void SetHitPart(BodyPartRecord forceHitPart)
		{
			this.hitPartInt = forceHitPart;
		}

		// Token: 0x06005A99 RID: 23193 RVA: 0x002E7836 File Offset: 0x002E5C36
		public void SetInstantPermanentInjury(bool val)
		{
			this.instantPermanentInjuryInt = val;
		}

		// Token: 0x06005A9A RID: 23194 RVA: 0x002E7840 File Offset: 0x002E5C40
		public void SetWeaponBodyPartGroup(BodyPartGroupDef gr)
		{
			this.weaponBodyPartGroupInt = gr;
		}

		// Token: 0x06005A9B RID: 23195 RVA: 0x002E784A File Offset: 0x002E5C4A
		public void SetWeaponHediff(HediffDef hd)
		{
			this.weaponHediffInt = hd;
		}

		// Token: 0x06005A9C RID: 23196 RVA: 0x002E7854 File Offset: 0x002E5C54
		public void SetAllowDamagePropagation(bool val)
		{
			this.allowDamagePropagationInt = val;
		}

		// Token: 0x06005A9D RID: 23197 RVA: 0x002E7860 File Offset: 0x002E5C60
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

		// Token: 0x06005A9E RID: 23198 RVA: 0x002E78C4 File Offset: 0x002E5CC4
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

		// Token: 0x02000EE9 RID: 3817
		public enum SourceCategory
		{
			// Token: 0x04003C9D RID: 15517
			ThingOrUnknown,
			// Token: 0x04003C9E RID: 15518
			Collapse
		}
	}
}
