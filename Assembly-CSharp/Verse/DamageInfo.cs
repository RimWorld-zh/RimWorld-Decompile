using System;
using UnityEngine;

namespace Verse
{
	public struct DamageInfo
	{
		public enum SourceCategory
		{
			ThingOrUnknown = 0,
			Collapse = 1
		}

		private DamageDef defInt;

		private int amountInt;

		private float angleInt;

		private Thing instigatorInt;

		private SourceCategory categoryInt;

		private BodyPartRecord forceHitPartInt;

		private BodyPartHeight heightInt;

		private BodyPartDepth depthInt;

		private ThingDef weaponGearInt;

		private BodyPartGroupDef weaponBodyPartGroupInt;

		private HediffDef weaponHediffInt;

		private bool instantOldInjuryInt;

		private bool allowDamagePropagationInt;

		public DamageDef Def
		{
			get
			{
				return this.defInt;
			}
		}

		public int Amount
		{
			get
			{
				if (!DebugSettings.enableDamage)
				{
					return 0;
				}
				return this.amountInt;
			}
		}

		public Thing Instigator
		{
			get
			{
				return this.instigatorInt;
			}
		}

		public SourceCategory Category
		{
			get
			{
				return this.categoryInt;
			}
		}

		public float Angle
		{
			get
			{
				return this.angleInt;
			}
		}

		public BodyPartRecord ForceHitPart
		{
			get
			{
				return this.forceHitPartInt;
			}
		}

		public BodyPartHeight Height
		{
			get
			{
				return this.heightInt;
			}
		}

		public BodyPartDepth Depth
		{
			get
			{
				return this.depthInt;
			}
		}

		public ThingDef WeaponGear
		{
			get
			{
				return this.weaponGearInt;
			}
		}

		public BodyPartGroupDef WeaponBodyPartGroup
		{
			get
			{
				return this.weaponBodyPartGroupInt;
			}
		}

		public HediffDef WeaponLinkedHediff
		{
			get
			{
				return this.weaponHediffInt;
			}
		}

		public bool InstantOldInjury
		{
			get
			{
				return this.instantOldInjuryInt;
			}
		}

		public bool AllowDamagePropagation
		{
			get
			{
				if (this.InstantOldInjury)
				{
					return false;
				}
				return this.allowDamagePropagationInt;
			}
		}

		public DamageInfo(DamageDef def, int amount, float angle = -1, Thing instigator = null, BodyPartRecord forceHitPart = null, ThingDef weaponGear = null, SourceCategory category = SourceCategory.ThingOrUnknown)
		{
			this.defInt = def;
			this.amountInt = amount;
			if (angle < 0.0)
			{
				this.angleInt = (float)Rand.RangeInclusive(0, 359);
			}
			else
			{
				this.angleInt = angle;
			}
			this.instigatorInt = instigator;
			this.categoryInt = category;
			this.forceHitPartInt = forceHitPart;
			this.heightInt = BodyPartHeight.Undefined;
			this.depthInt = BodyPartDepth.Undefined;
			this.weaponGearInt = weaponGear;
			this.weaponBodyPartGroupInt = null;
			this.weaponHediffInt = null;
			this.instantOldInjuryInt = false;
			this.allowDamagePropagationInt = true;
		}

		public DamageInfo(DamageInfo cloneSource)
		{
			this.defInt = cloneSource.defInt;
			this.amountInt = cloneSource.amountInt;
			this.angleInt = cloneSource.angleInt;
			this.instigatorInt = cloneSource.instigatorInt;
			this.categoryInt = cloneSource.categoryInt;
			this.forceHitPartInt = cloneSource.forceHitPartInt;
			this.heightInt = cloneSource.heightInt;
			this.depthInt = cloneSource.depthInt;
			this.weaponGearInt = cloneSource.weaponGearInt;
			this.weaponBodyPartGroupInt = cloneSource.weaponBodyPartGroupInt;
			this.weaponHediffInt = cloneSource.weaponHediffInt;
			this.instantOldInjuryInt = cloneSource.instantOldInjuryInt;
			this.allowDamagePropagationInt = cloneSource.allowDamagePropagationInt;
		}

		public void SetAmount(int newAmount)
		{
			this.amountInt = newAmount;
		}

		public void SetBodyRegion(BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined)
		{
			this.heightInt = height;
			this.depthInt = depth;
		}

		public void SetForcedHitPart(BodyPartRecord forceHitPart)
		{
			this.forceHitPartInt = forceHitPart;
		}

		public void SetInstantOldInjury(bool val)
		{
			this.instantOldInjuryInt = val;
		}

		public void SetWeaponBodyPartGroup(BodyPartGroupDef gr)
		{
			this.weaponBodyPartGroupInt = gr;
		}

		public void SetWeaponHediff(HediffDef hd)
		{
			this.weaponHediffInt = hd;
		}

		public void SetAllowDamagePropagation(bool val)
		{
			this.allowDamagePropagationInt = val;
		}

		public void SetAngle(Vector3 vec)
		{
			if (vec.x != 0.0 || vec.z != 0.0)
			{
				Vector3 eulerAngles = Quaternion.LookRotation(vec).eulerAngles;
				this.angleInt = eulerAngles.y;
			}
			else
			{
				this.angleInt = (float)Rand.RangeInclusive(0, 359);
			}
		}

		public override string ToString()
		{
			return "(def=" + this.defInt + ", amount= " + this.amountInt + ", instigator=" + ((this.instigatorInt == null) ? ((Enum)(object)this.categoryInt).ToString() : this.instigatorInt.ToString()) + ", angle=" + this.angleInt.ToString("F1") + ")";
		}
	}
}
