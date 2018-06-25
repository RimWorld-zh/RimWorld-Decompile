using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B4 RID: 1716
	public class Building_WorkTable_HeatPush : Building_WorkTable
	{
		// Token: 0x04001454 RID: 5204
		private const int HeatPushInterval = 30;

		// Token: 0x060024E5 RID: 9445 RVA: 0x0013BD11 File Offset: 0x0013A111
		public override void UsedThisTick()
		{
			base.UsedThisTick();
			if (Find.TickManager.TicksGame % 30 == 4)
			{
				GenTemperature.PushHeat(this, this.def.building.heatPerTickWhileWorking * 30f);
			}
		}
	}
}
