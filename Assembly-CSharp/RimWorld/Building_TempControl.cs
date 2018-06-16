using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200069B RID: 1691
	public class Building_TempControl : Building
	{
		// Token: 0x060023CC RID: 9164 RVA: 0x00132850 File Offset: 0x00130C50
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.compTempControl = base.GetComp<CompTempControl>();
			this.compPowerTrader = base.GetComp<CompPowerTrader>();
		}

		// Token: 0x040013FD RID: 5117
		public CompTempControl compTempControl;

		// Token: 0x040013FE RID: 5118
		public CompPowerTrader compPowerTrader;
	}
}
