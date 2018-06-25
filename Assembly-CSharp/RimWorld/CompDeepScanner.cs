using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200070B RID: 1803
	public class CompDeepScanner : ThingComp
	{
		// Token: 0x040015CE RID: 5582
		private CompPowerTrader powerComp;

		// Token: 0x06002782 RID: 10114 RVA: 0x00152F6F File Offset: 0x0015136F
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x00152F8A File Offset: 0x0015138A
		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.powerComp.PowerOn)
			{
				this.parent.Map.deepResourceGrid.MarkForDraw();
			}
		}
	}
}
