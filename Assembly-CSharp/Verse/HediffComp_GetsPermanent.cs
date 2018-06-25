using System;
using UnityEngine;

namespace Verse
{
	public class HediffComp_GetsPermanent : HediffComp
	{
		public float permanentDamageThreshold = 9999f;

		public bool isPermanentInt = false;

		public float painFactor = 1f;

		public HediffComp_GetsPermanent()
		{
		}

		public HediffCompProperties_GetsPermanent Props
		{
			get
			{
				return (HediffCompProperties_GetsPermanent)this.props;
			}
		}

		public bool IsPermanent
		{
			get
			{
				return this.isPermanentInt;
			}
			set
			{
				if (value != this.isPermanentInt)
				{
					this.isPermanentInt = value;
					if (this.isPermanentInt)
					{
						this.painFactor = PermanentInjuryUtility.GetRandomPainFactor();
					}
				}
			}
		}

		private bool Active
		{
			get
			{
				return this.permanentDamageThreshold < 9000f;
			}
		}

		public override void CompExposeData()
		{
			Scribe_Values.Look<bool>(ref this.isPermanentInt, "isPermanent", false, false);
			Scribe_Values.Look<float>(ref this.permanentDamageThreshold, "permanentDamageThreshold", 9999f, false);
			Scribe_Values.Look<float>(ref this.painFactor, "painFactor", 1f, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				BackCompatibility.HediffComp_GetsPermanentLoadingVars(this);
			}
		}

		public override void CompPostInjuryHeal(float amount)
		{
			if (this.Active && !this.IsPermanent)
			{
				if (this.parent.Severity <= this.permanentDamageThreshold && this.parent.Severity >= this.permanentDamageThreshold - amount)
				{
					float num = 0.1f;
					HediffComp_TendDuration hediffComp_TendDuration = this.parent.TryGetComp<HediffComp_TendDuration>();
					if (hediffComp_TendDuration != null)
					{
						num *= Mathf.Clamp01(1f - hediffComp_TendDuration.tendQuality);
					}
					if (Rand.Value < num)
					{
						this.parent.Severity = this.permanentDamageThreshold;
						this.IsPermanent = true;
						base.Pawn.health.Notify_HediffChanged(this.parent);
					}
					else
					{
						this.permanentDamageThreshold = 9999f;
					}
				}
			}
		}

		public override string CompDebugString()
		{
			return string.Concat(new object[]
			{
				"isPermanent: ",
				this.isPermanentInt,
				"\npermanentDamageThreshold: ",
				this.permanentDamageThreshold,
				"\npainFactor: ",
				this.painFactor
			});
		}
	}
}
