using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000699 RID: 1689
	public class Building_TempControl : Building
	{
		// Token: 0x040013FB RID: 5115
		public CompTempControl compTempControl;

		// Token: 0x040013FC RID: 5116
		public CompPowerTrader compPowerTrader;

		// Token: 0x060023CA RID: 9162 RVA: 0x00132B60 File Offset: 0x00130F60
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.compTempControl = base.GetComp<CompTempControl>();
			this.compPowerTrader = base.GetComp<CompPowerTrader>();
		}
	}
}
