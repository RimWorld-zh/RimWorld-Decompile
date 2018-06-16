using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000314 RID: 788
	public class GameCondition_ClimateCycle : GameCondition
	{
		// Token: 0x06000D5C RID: 3420 RVA: 0x0007325C File Offset: 0x0007165C
		public override void Init()
		{
			this.ticksOffset = ((Rand.Value >= 0.5f) ? 7200000 : 0);
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x0007327F File Offset: 0x0007167F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksOffset, "ticksOffset", 0, false);
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x0007329C File Offset: 0x0007169C
		public override float TemperatureOffset()
		{
			return Mathf.Sin(GenDate.YearsPassedFloat / 4f * 3.14159274f * 2f) * 20f;
		}

		// Token: 0x04000890 RID: 2192
		private int ticksOffset = 0;

		// Token: 0x04000891 RID: 2193
		private const float PeriodYears = 4f;

		// Token: 0x04000892 RID: 2194
		private const float MaxTempOffset = 20f;
	}
}
