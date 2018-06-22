using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000709 RID: 1801
	public class CompDeepScanner : ThingComp
	{
		// Token: 0x0600277E RID: 10110 RVA: 0x00152E1F File Offset: 0x0015121F
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x00152E3A File Offset: 0x0015123A
		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.powerComp.PowerOn)
			{
				this.parent.Map.deepResourceGrid.MarkForDraw();
			}
		}

		// Token: 0x040015CE RID: 5582
		private CompPowerTrader powerComp;
	}
}
