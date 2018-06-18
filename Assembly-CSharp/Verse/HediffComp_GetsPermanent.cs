using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D0E RID: 3342
	public class HediffComp_GetsPermanent : HediffComp
	{
		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x060049A6 RID: 18854 RVA: 0x0026867C File Offset: 0x00266A7C
		public HediffCompProperties_GetsPermanent Props
		{
			get
			{
				return (HediffCompProperties_GetsPermanent)this.props;
			}
		}

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x060049A7 RID: 18855 RVA: 0x0026869C File Offset: 0x00266A9C
		// (set) Token: 0x060049A8 RID: 18856 RVA: 0x002686B7 File Offset: 0x00266AB7
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

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x060049A9 RID: 18857 RVA: 0x002686E8 File Offset: 0x00266AE8
		private bool Active
		{
			get
			{
				return this.permanentDamageThreshold < 9000f;
			}
		}

		// Token: 0x060049AA RID: 18858 RVA: 0x0026870C File Offset: 0x00266B0C
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

		// Token: 0x060049AB RID: 18859 RVA: 0x0026876C File Offset: 0x00266B6C
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

		// Token: 0x060049AC RID: 18860 RVA: 0x0026883C File Offset: 0x00266C3C
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

		// Token: 0x040031ED RID: 12781
		public float permanentDamageThreshold = 9999f;

		// Token: 0x040031EE RID: 12782
		public bool isPermanentInt = false;

		// Token: 0x040031EF RID: 12783
		public float painFactor = 1f;
	}
}
