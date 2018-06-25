using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000316 RID: 790
	public class GameCondition_ClimateCycle : GameCondition
	{
		// Token: 0x04000895 RID: 2197
		private int ticksOffset = 0;

		// Token: 0x04000896 RID: 2198
		private const float PeriodYears = 4f;

		// Token: 0x04000897 RID: 2199
		private const float MaxTempOffset = 20f;

		// Token: 0x06000D5F RID: 3423 RVA: 0x00073468 File Offset: 0x00071868
		public override void Init()
		{
			this.ticksOffset = ((Rand.Value >= 0.5f) ? 7200000 : 0);
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x0007348B File Offset: 0x0007188B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksOffset, "ticksOffset", 0, false);
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x000734A8 File Offset: 0x000718A8
		public override float TemperatureOffset()
		{
			return Mathf.Sin(GenDate.YearsPassedFloat / 4f * 3.14159274f * 2f) * 20f;
		}
	}
}
