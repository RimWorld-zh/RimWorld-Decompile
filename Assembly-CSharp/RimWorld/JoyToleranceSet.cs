using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004F1 RID: 1265
	public class JoyToleranceSet : IExposable
	{
		// Token: 0x170002FC RID: 764
		public float this[JoyKindDef d]
		{
			get
			{
				return this.tolerances[d];
			}
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x000C8E95 File Offset: 0x000C7295
		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<JoyKindDef, float>>(ref this.tolerances, "tolerances", new object[0]);
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x000C8EAE File Offset: 0x000C72AE
		public void Notify_JoyGained(float amount, JoyKindDef joyKind)
		{
			this.tolerances[joyKind] = Mathf.Min(this.tolerances[joyKind] + amount * 0.65f, 1f);
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x000C8EDC File Offset: 0x000C72DC
		public float JoyFactorFromTolerance(JoyKindDef joyKind)
		{
			return 1f - this.tolerances[joyKind];
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x000C8F04 File Offset: 0x000C7304
		public void NeedInterval()
		{
			for (int i = 0; i < this.tolerances.Count; i++)
			{
				float num = this.tolerances[i];
				num -= 0.000208333338f;
				if (num < 0f)
				{
					num = 0f;
				}
				this.tolerances[i] = num;
			}
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x000C8F64 File Offset: 0x000C7364
		public string TolerancesString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<JoyKindDef> allDefsListForReading = DefDatabase<JoyKindDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				JoyKindDef joyKindDef = allDefsListForReading[i];
				float num = this.tolerances[joyKindDef];
				if (num > 0.01f)
				{
					if (stringBuilder.Length == 0)
					{
						stringBuilder.AppendLine("JoyTolerances".Translate() + ":");
					}
					stringBuilder.AppendLine("   -" + joyKindDef.LabelCap + ": " + num.ToStringPercent());
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x04000D3B RID: 3387
		private DefMap<JoyKindDef, float> tolerances = new DefMap<JoyKindDef, float>();
	}
}
