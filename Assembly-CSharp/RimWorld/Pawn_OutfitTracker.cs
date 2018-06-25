using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200050D RID: 1293
	public class Pawn_OutfitTracker : IExposable
	{
		// Token: 0x04000DCF RID: 3535
		public Pawn pawn;

		// Token: 0x04000DD0 RID: 3536
		private Outfit curOutfit;

		// Token: 0x04000DD1 RID: 3537
		public OutfitForcedHandler forcedHandler = new OutfitForcedHandler();

		// Token: 0x0600173C RID: 5948 RVA: 0x000CC783 File Offset: 0x000CAB83
		public Pawn_OutfitTracker()
		{
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x000CC797 File Offset: 0x000CAB97
		public Pawn_OutfitTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x0600173E RID: 5950 RVA: 0x000CC7B4 File Offset: 0x000CABB4
		// (set) Token: 0x0600173F RID: 5951 RVA: 0x000CC7EF File Offset: 0x000CABEF
		public Outfit CurrentOutfit
		{
			get
			{
				if (this.curOutfit == null)
				{
					this.curOutfit = Current.Game.outfitDatabase.DefaultOutfit();
				}
				return this.curOutfit;
			}
			set
			{
				if (this.curOutfit != value)
				{
					this.curOutfit = value;
					if (this.pawn.mindState != null)
					{
						this.pawn.mindState.Notify_OutfitChanged();
					}
				}
			}
		}

		// Token: 0x06001740 RID: 5952 RVA: 0x000CC82A File Offset: 0x000CAC2A
		public void ExposeData()
		{
			Scribe_References.Look<Outfit>(ref this.curOutfit, "curOutfit", false);
			Scribe_Deep.Look<OutfitForcedHandler>(ref this.forcedHandler, "overrideHandler", new object[0]);
		}
	}
}
