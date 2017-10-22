using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class JoyToleranceSet : IExposable
	{
		private const float ToleranceGainRate = 0.4f;

		private const float ToleranceDropPerDay = 0.0833333358f;

		private DefMap<JoyKindDef, float> tolerances = new DefMap<JoyKindDef, float>();

		public float this[JoyKindDef d]
		{
			get
			{
				return this.tolerances[d];
			}
		}

		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<JoyKindDef, float>>(ref this.tolerances, "tolerances", new object[0]);
		}

		public void Notify_JoyGained(float amount, JoyKindDef joyKind)
		{
			this.tolerances[joyKind] = Mathf.Min((float)(this.tolerances[joyKind] + amount * 0.40000000596046448), 1f);
		}

		public float JoyFactorFromTolerance(JoyKindDef joyKind)
		{
			return (float)(1.0 - this.tolerances[joyKind]);
		}

		public void NeedInterval()
		{
			for (int i = 0; i < this.tolerances.Count; i++)
			{
				float num = this.tolerances[i];
				num = (float)(num - 0.00020833333837799728);
				if (num < 0.0)
				{
					num = 0f;
				}
				this.tolerances[i] = num;
			}
		}

		public string TolerancesString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("\n" + "JoyTolerances".Translate() + ":");
			List<JoyKindDef> allDefsListForReading = DefDatabase<JoyKindDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				JoyKindDef joyKindDef = allDefsListForReading[i];
				float num = this.tolerances[joyKindDef];
				if (num > 0.0099999997764825821)
				{
					stringBuilder.AppendLine("   -" + joyKindDef.label + ": " + num.ToStringPercent());
				}
			}
			return stringBuilder.ToString();
		}
	}
}
