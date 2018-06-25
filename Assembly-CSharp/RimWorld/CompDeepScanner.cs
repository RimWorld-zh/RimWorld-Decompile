using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200070B RID: 1803
	public class CompDeepScanner : ThingComp
	{
		// Token: 0x040015D2 RID: 5586
		private CompPowerTrader powerComp;

		// Token: 0x06002781 RID: 10113 RVA: 0x001531CF File Offset: 0x001515CF
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x001531EA File Offset: 0x001515EA
		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.powerComp.PowerOn)
			{
				this.parent.Map.deepResourceGrid.MarkForDraw();
			}
		}
	}
}
