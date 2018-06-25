using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000316 RID: 790
	public class GameCondition_ClimateCycle : GameCondition
	{
		// Token: 0x04000892 RID: 2194
		private int ticksOffset = 0;

		// Token: 0x04000893 RID: 2195
		private const float PeriodYears = 4f;

		// Token: 0x04000894 RID: 2196
		private const float MaxTempOffset = 20f;

		// Token: 0x06000D60 RID: 3424 RVA: 0x00073460 File Offset: 0x00071860
		public override void Init()
		{
			this.ticksOffset = ((Rand.Value >= 0.5f) ? 7200000 : 0);
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x00073483 File Offset: 0x00071883
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksOffset, "ticksOffset", 0, false);
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x000734A0 File Offset: 0x000718A0
		public override float TemperatureOffset()
		{
			return Mathf.Sin(GenDate.YearsPassedFloat / 4f * 3.14159274f * 2f) * 20f;
		}
	}
}
