using UnityEngine;

namespace Verse
{
	public class HediffComp_GetsOld : HediffComp
	{
		public float oldDamageThreshold = 9999f;

		public bool isOldInt = false;

		public float painFactor = 1f;

		public HediffCompProperties_GetsOld Props
		{
			get
			{
				return (HediffCompProperties_GetsOld)base.props;
			}
		}

		public bool IsOld
		{
			get
			{
				return this.isOldInt;
			}
			set
			{
				if (value != this.isOldInt)
				{
					this.isOldInt = value;
					if (this.isOldInt)
					{
						this.painFactor = OldInjuryUtility.GetRandomPainFactor();
					}
				}
			}
		}

		private bool Active
		{
			get
			{
				return this.oldDamageThreshold < 9000.0;
			}
		}

		public override void CompExposeData()
		{
			Scribe_Values.Look<bool>(ref this.isOldInt, "isOld", false, false);
			Scribe_Values.Look<float>(ref this.oldDamageThreshold, "oldDamageThreshold", 9999f, false);
			Scribe_Values.Look<float>(ref this.painFactor, "painFactor", 1f, false);
		}

		public override void CompPostInjuryHeal(float amount)
		{
			if (this.Active && !this.IsOld && base.parent.Severity <= this.oldDamageThreshold && base.parent.Severity >= this.oldDamageThreshold - amount)
			{
				float num = 0.2f;
				HediffComp_TendDuration hediffComp_TendDuration = base.parent.TryGetComp<HediffComp_TendDuration>();
				if (hediffComp_TendDuration != null)
				{
					num *= Mathf.Clamp01((float)(1.0 - hediffComp_TendDuration.tendQuality));
				}
				if (Rand.Value < num)
				{
					base.parent.Severity = this.oldDamageThreshold;
					this.IsOld = true;
					base.Pawn.health.Notify_HediffChanged(base.parent);
				}
				else
				{
					this.oldDamageThreshold = 9999f;
				}
			}
		}

		public override string CompDebugString()
		{
			return "isOld: " + this.isOldInt + "\noldDamageThreshold: " + this.oldDamageThreshold + "\npainFactor: " + this.painFactor;
		}
	}
}
