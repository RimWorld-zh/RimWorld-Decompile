using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000699 RID: 1689
	public class Building_TempControl : Building
	{
		// Token: 0x040013FF RID: 5119
		public CompTempControl compTempControl;

		// Token: 0x04001400 RID: 5120
		public CompPowerTrader compPowerTrader;

		// Token: 0x060023C9 RID: 9161 RVA: 0x00132DC8 File Offset: 0x001311C8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.compTempControl = base.GetComp<CompTempControl>();
			this.compPowerTrader = base.GetComp<CompPowerTrader>();
		}
	}
}
