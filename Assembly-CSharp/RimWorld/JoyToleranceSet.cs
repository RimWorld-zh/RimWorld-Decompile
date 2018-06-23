using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004ED RID: 1261
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

		// Token: 0x060016A4 RID: 5796 RVA: 0x000C8E8D File Offset: 0x000C728D
		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<JoyKindDef, float>>(ref this.tolerances, "tolerances", new object[0]);
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x000C8EA6 File Offset: 0x000C72A6
		public void Notify_JoyGained(float amount, JoyKindDef joyKind)
		{
			this.tolerances[joyKind] = Mathf.Min(this.tolerances[joyKind] + amount * 0.65f, 1f);
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x000C8ED4 File Offset: 0x000C72D4
		public float JoyFactorFromTolerance(JoyKindDef joyKind)
		{
			return 1f - this.tolerances[joyKind];
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x000C8EFC File Offset: 0x000C72FC
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

		// Token: 0x060016A8 RID: 5800 RVA: 0x000C8F5C File Offset: 0x000C735C
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
