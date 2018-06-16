using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B6 RID: 1718
	public class Building_WorkTable_HeatPush : Building_WorkTable
	{
		// Token: 0x060024E7 RID: 9447 RVA: 0x0013BA01 File Offset: 0x00139E01
		public override void UsedThisTick()
		{
			base.UsedThisTick();
			if (Find.TickManager.TicksGame % 30 == 4)
			{
				GenTemperature.PushHeat(this, this.def.building.heatPerTickWhileWorking * 30f);
			}
		}

		// Token: 0x04001456 RID: 5206
		private const int HeatPushInterval = 30;
	}
}
