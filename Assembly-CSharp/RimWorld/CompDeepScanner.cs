using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200070D RID: 1805
	public class CompDeepScanner : ThingComp
	{
		// Token: 0x06002784 RID: 10116 RVA: 0x00152C03 File Offset: 0x00151003
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
		}

		// Token: 0x06002785 RID: 10117 RVA: 0x00152C1E File Offset: 0x0015101E
		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.powerComp.PowerOn)
			{
				this.parent.Map.deepResourceGrid.MarkForDraw();
			}
		}

		// Token: 0x040015D0 RID: 5584
		private CompPowerTrader powerComp;
	}
}
