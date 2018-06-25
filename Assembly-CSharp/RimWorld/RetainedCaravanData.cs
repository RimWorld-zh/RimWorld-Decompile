using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200042E RID: 1070
	public class RetainedCaravanData : IExposable
	{
		// Token: 0x04000B6D RID: 2925
		private Map map;

		// Token: 0x04000B6E RID: 2926
		private bool shouldPassStoryState;

		// Token: 0x04000B6F RID: 2927
		private int nextTile = -1;

		// Token: 0x04000B70 RID: 2928
		private float nextTileCostLeftPct;

		// Token: 0x04000B71 RID: 2929
		private bool paused;

		// Token: 0x04000B72 RID: 2930
		private int destinationTile = -1;

		// Token: 0x04000B73 RID: 2931
		private CaravanArrivalAction arrivalAction;

		// Token: 0x060012B7 RID: 4791 RVA: 0x000A2626 File Offset: 0x000A0A26
		public RetainedCaravanData(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060012B8 RID: 4792 RVA: 0x000A2644 File Offset: 0x000A0A44
		public bool HasDestinationTile
		{
			get
			{
				return this.destinationTile != -1;
			}
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x000A2668 File Offset: 0x000A0A68
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.shouldPassStoryState, "shouldPassStoryState", false, false);
			Scribe_Values.Look<int>(ref this.nextTile, "nextTile", -1, false);
			Scribe_Values.Look<float>(ref this.nextTileCostLeftPct, "nextTileCostLeftPct", -1f, false);
			Scribe_Values.Look<bool>(ref this.paused, "paused", false, false);
			Scribe_Values.Look<int>(ref this.destinationTile, "destinationTile", 0, false);
			Scribe_Deep.Look<CaravanArrivalAction>(ref this.arrivalAction, "arrivalAction", new object[0]);
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x000A26EA File Offset: 0x000A0AEA
		public void Notify_GeneratedTempIncidentMapFor(Caravan caravan)
		{
			if (this.map.Parent.def.isTempIncidentMapOwner)
			{
				this.Set(caravan);
			}
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x000A2714 File Offset: 0x000A0B14
		public void Notify_CaravanFormed(Caravan caravan)
		{
			if (this.shouldPassStoryState)
			{
				this.shouldPassStoryState = false;
				this.map.StoryState.CopyTo(caravan.StoryState);
			}
			if (this.nextTile != -1 && this.nextTile != caravan.Tile && caravan.CanReach(this.nextTile))
			{
				caravan.pather.StartPath(this.nextTile, null, true, true);
				caravan.pather.nextTileCostLeft = caravan.pather.nextTileCostTotal * this.nextTileCostLeftPct;
				caravan.pather.Paused = this.paused;
				caravan.tweener.ResetTweenedPosToRoot();
			}
			if (this.HasDestinationTile && this.destinationTile != caravan.Tile)
			{
				caravan.pather.StartPath(this.destinationTile, this.arrivalAction, true, true);
				this.destinationTile = -1;
				this.arrivalAction = null;
			}
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x000A2810 File Offset: 0x000A0C10
		private void Set(Caravan caravan)
		{
			caravan.StoryState.CopyTo(this.map.StoryState);
			this.shouldPassStoryState = true;
			if (caravan.pather.Moving)
			{
				this.nextTile = caravan.pather.nextTile;
				this.nextTileCostLeftPct = caravan.pather.nextTileCostLeft / caravan.pather.nextTileCostTotal;
				this.paused = caravan.pather.Paused;
				this.destinationTile = caravan.pather.Destination;
				this.arrivalAction = caravan.pather.ArrivalAction;
			}
			else
			{
				this.nextTile = -1;
				this.nextTileCostLeftPct = 0f;
				this.paused = false;
				this.destinationTile = -1;
				this.arrivalAction = null;
			}
		}
	}
}
