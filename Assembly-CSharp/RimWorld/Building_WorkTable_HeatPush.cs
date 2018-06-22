using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B2 RID: 1714
	public class Building_WorkTable_HeatPush : Building_WorkTable
	{
		// Token: 0x060024E1 RID: 9441 RVA: 0x0013BBC1 File Offset: 0x00139FC1
		public override void UsedThisTick()
		{
			base.UsedThisTick();
			if (Find.TickManager.TicksGame % 30 == 4)
			{
				GenTemperature.PushHeat(this, this.def.building.heatPerTickWhileWorking * 30f);
			}
		}

		// Token: 0x04001454 RID: 5204
		private const int HeatPushInterval = 30;
	}
}
