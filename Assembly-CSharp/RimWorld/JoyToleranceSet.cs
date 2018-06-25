using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004EF RID: 1263
	public class JoyToleranceSet : IExposable
	{
		// Token: 0x04000D38 RID: 3384
		private DefMap<JoyKindDef, float> tolerances = new DefMap<JoyKindDef, float>();

		// Token: 0x170002FC RID: 764
		public float this[JoyKindDef d]
		{
			get
			{
				return this.tolerances[d];
			}
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x000C8FDD File Offset: 0x000C73DD
		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<JoyKindDef, float>>(ref this.tolerances, "tolerances", new object[0]);
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x000C8FF6 File Offset: 0x000C73F6
		public void Notify_JoyGained(float amount, JoyKindDef joyKind)
		{
			this.tolerances[joyKind] = Mathf.Min(this.tolerances[joyKind] + amount * 0.65f, 1f);
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x000C9024 File Offset: 0x000C7424
		public float JoyFactorFromTolerance(JoyKindDef joyKind)
		{
			return 1f - this.tolerances[joyKind];
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x000C904C File Offset: 0x000C744C
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

		// Token: 0x060016AC RID: 5804 RVA: 0x000C90AC File Offset: 0x000C74AC
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
	}
}
