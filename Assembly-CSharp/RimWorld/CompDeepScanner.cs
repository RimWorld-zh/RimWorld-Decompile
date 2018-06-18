using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200070D RID: 1805
	public class CompDeepScanner : ThingComp
	{
		// Token: 0x06002786 RID: 10118 RVA: 0x00152C7B File Offset: 0x0015107B
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
		}

		// Token: 0x06002787 RID: 10119 RVA: 0x00152C96 File Offset: 0x00151096
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
