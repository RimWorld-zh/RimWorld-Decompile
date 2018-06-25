using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D0E RID: 3342
	public class HediffComp_GetsPermanent : HediffComp
	{
		// Token: 0x040031FF RID: 12799
		public float permanentDamageThreshold = 9999f;

		// Token: 0x04003200 RID: 12800
		public bool isPermanentInt = false;

		// Token: 0x04003201 RID: 12801
		public float painFactor = 1f;

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x060049BA RID: 18874 RVA: 0x00269E6C File Offset: 0x0026826C
		public HediffCompProperties_GetsPermanent Props
		{
			get
			{
				return (HediffCompProperties_GetsPermanent)this.props;
			}
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x060049BB RID: 18875 RVA: 0x00269E8C File Offset: 0x0026828C
		// (set) Token: 0x060049BC RID: 18876 RVA: 0x00269EA7 File Offset: 0x002682A7
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
		// (get) Token: 0x060049BD RID: 18877 RVA: 0x00269ED8 File Offset: 0x002682D8
		private bool Active
		{
			get
			{
				return this.permanentDamageThreshold < 9000f;
			}
		}

		// Token: 0x060049BE RID: 18878 RVA: 0x00269EFC File Offset: 0x002682FC
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

		// Token: 0x060049BF RID: 18879 RVA: 0x00269F5C File Offset: 0x0026835C
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

		// Token: 0x060049C0 RID: 18880 RVA: 0x0026A02C File Offset: 0x0026842C
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
