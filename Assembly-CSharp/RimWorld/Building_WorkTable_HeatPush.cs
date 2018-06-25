using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B4 RID: 1716
	public class Building_WorkTable_HeatPush : Building_WorkTable
	{
		// Token: 0x04001458 RID: 5208
		private const int HeatPushInterval = 30;

		// Token: 0x060024E4 RID: 9444 RVA: 0x0013BF79 File Offset: 0x0013A379
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
