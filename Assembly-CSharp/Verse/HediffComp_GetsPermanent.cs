using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D0B RID: 3339
	public class HediffComp_GetsPermanent : HediffComp
	{
		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x060049B7 RID: 18871 RVA: 0x00269AB0 File Offset: 0x00267EB0
		public HediffCompProperties_GetsPermanent Props
		{
			get
			{
				return (HediffCompProperties_GetsPermanent)this.props;
			}
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x060049B8 RID: 18872 RVA: 0x00269AD0 File Offset: 0x00267ED0
		// (set) Token: 0x060049B9 RID: 18873 RVA: 0x00269AEB File Offset: 0x00267EEB
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

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x060049BA RID: 18874 RVA: 0x00269B1C File Offset: 0x00267F1C
		private bool Active
		{
			get
			{
				return this.permanentDamageThreshold < 9000f;
			}
		}

		// Token: 0x060049BB RID: 18875 RVA: 0x00269B40 File Offset: 0x00267F40
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

		// Token: 0x060049BC RID: 18876 RVA: 0x00269BA0 File Offset: 0x00267FA0
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

		// Token: 0x060049BD RID: 18877 RVA: 0x00269C70 File Offset: 0x00268070
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

		// Token: 0x040031F8 RID: 12792
		public float permanentDamageThreshold = 9999f;

		// Token: 0x040031F9 RID: 12793
		public bool isPermanentInt = false;

		// Token: 0x040031FA RID: 12794
		public float painFactor = 1f;
	}
}
