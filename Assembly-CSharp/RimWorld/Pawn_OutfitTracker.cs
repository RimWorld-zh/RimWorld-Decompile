using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200050F RID: 1295
	public class Pawn_OutfitTracker : IExposable
	{
		// Token: 0x06001741 RID: 5953 RVA: 0x000CC3E7 File Offset: 0x000CA7E7
		public Pawn_OutfitTracker()
		{
		}

		// Token: 0x06001742 RID: 5954 RVA: 0x000CC3FB File Offset: 0x000CA7FB
		public Pawn_OutfitTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06001743 RID: 5955 RVA: 0x000CC418 File Offset: 0x000CA818
		// (set) Token: 0x06001744 RID: 5956 RVA: 0x000CC453 File Offset: 0x000CA853
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

		// Token: 0x06001745 RID: 5957 RVA: 0x000CC48E File Offset: 0x000CA88E
		public void ExposeData()
		{
			Scribe_References.Look<Outfit>(ref this.curOutfit, "curOutfit", false);
			Scribe_Deep.Look<OutfitForcedHandler>(ref this.forcedHandler, "overrideHandler", new object[0]);
		}

		// Token: 0x04000DCF RID: 3535
		public Pawn pawn;

		// Token: 0x04000DD0 RID: 3536
		private Outfit curOutfit;

		// Token: 0x04000DD1 RID: 3537
		public OutfitForcedHandler forcedHandler = new OutfitForcedHandler();
	}
}
