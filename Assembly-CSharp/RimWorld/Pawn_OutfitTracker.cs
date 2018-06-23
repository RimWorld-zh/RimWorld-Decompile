using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200050B RID: 1291
	public class Pawn_OutfitTracker : IExposable
	{
		// Token: 0x04000DCC RID: 3532
		public Pawn pawn;

		// Token: 0x04000DCD RID: 3533
		private Outfit curOutfit;

		// Token: 0x04000DCE RID: 3534
		public OutfitForcedHandler forcedHandler = new OutfitForcedHandler();

		// Token: 0x06001739 RID: 5945 RVA: 0x000CC433 File Offset: 0x000CA833
		public Pawn_OutfitTracker()
		{
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x000CC447 File Offset: 0x000CA847
		public Pawn_OutfitTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x0600173B RID: 5947 RVA: 0x000CC464 File Offset: 0x000CA864
		// (set) Token: 0x0600173C RID: 5948 RVA: 0x000CC49F File Offset: 0x000CA89F
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

		// Token: 0x0600173D RID: 5949 RVA: 0x000CC4DA File Offset: 0x000CA8DA
		public void ExposeData()
		{
			Scribe_References.Look<Outfit>(ref this.curOutfit, "curOutfit", false);
			Scribe_Deep.Look<OutfitForcedHandler>(ref this.forcedHandler, "overrideHandler", new object[0]);
		}
	}
}
