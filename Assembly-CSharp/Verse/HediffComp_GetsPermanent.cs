using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D0F RID: 3343
	public class HediffComp_GetsPermanent : HediffComp
	{
		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x060049A8 RID: 18856 RVA: 0x002686A4 File Offset: 0x00266AA4
		public HediffCompProperties_GetsPermanent Props
		{
			get
			{
				return (HediffCompProperties_GetsPermanent)this.props;
			}
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x060049A9 RID: 18857 RVA: 0x002686C4 File Offset: 0x00266AC4
		// (set) Token: 0x060049AA RID: 18858 RVA: 0x002686DF File Offset: 0x00266ADF
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

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x060049AB RID: 18859 RVA: 0x00268710 File Offset: 0x00266B10
		private bool Active
		{
			get
			{
				return this.permanentDamageThreshold < 9000f;
			}
		}

		// Token: 0x060049AC RID: 18860 RVA: 0x00268734 File Offset: 0x00266B34
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

		// Token: 0x060049AD RID: 18861 RVA: 0x00268794 File Offset: 0x00266B94
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

		// Token: 0x060049AE RID: 18862 RVA: 0x00268864 File Offset: 0x00266C64
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

		// Token: 0x040031EF RID: 12783
		public float permanentDamageThreshold = 9999f;

		// Token: 0x040031F0 RID: 12784
		public bool isPermanentInt = false;

		// Token: 0x040031F1 RID: 12785
		public float painFactor = 1f;
	}
}
