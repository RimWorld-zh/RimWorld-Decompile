using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000697 RID: 1687
	public class Building_TempControl : Building
	{
		// Token: 0x040013FB RID: 5115
		public CompTempControl compTempControl;

		// Token: 0x040013FC RID: 5116
		public CompPowerTrader compPowerTrader;

		// Token: 0x060023C6 RID: 9158 RVA: 0x00132A10 File Offset: 0x00130E10
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.compTempControl = base.GetComp<CompTempControl>();
			this.compPowerTrader = base.GetComp<CompPowerTrader>();
		}
	}
}
