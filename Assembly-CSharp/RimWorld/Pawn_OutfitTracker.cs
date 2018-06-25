using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200050D RID: 1293
	public class Pawn_OutfitTracker : IExposable
	{
		// Token: 0x04000DCC RID: 3532
		public Pawn pawn;

		// Token: 0x04000DCD RID: 3533
		private Outfit curOutfit;

		// Token: 0x04000DCE RID: 3534
		public OutfitForcedHandler forcedHandler = new OutfitForcedHandler();

		// Token: 0x0600173D RID: 5949 RVA: 0x000CC583 File Offset: 0x000CA983
		public Pawn_OutfitTracker()
		{
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x000CC597 File Offset: 0x000CA997
		public Pawn_OutfitTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x0600173F RID: 5951 RVA: 0x000CC5B4 File Offset: 0x000CA9B4
		// (set) Token: 0x06001740 RID: 5952 RVA: 0x000CC5EF File Offset: 0x000CA9EF
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

		// Token: 0x06001741 RID: 5953 RVA: 0x000CC62A File Offset: 0x000CAA2A
		public void ExposeData()
		{
			Scribe_References.Look<Outfit>(ref this.curOutfit, "curOutfit", false);
			Scribe_Deep.Look<OutfitForcedHandler>(ref this.forcedHandler, "overrideHandler", new object[0]);
		}
	}
}
