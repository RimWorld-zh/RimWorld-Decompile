using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200042E RID: 1070
	public class RetainedCaravanData : IExposable
	{
		// Token: 0x04000B6A RID: 2922
		private Map map;

		// Token: 0x04000B6B RID: 2923
		private bool shouldPassStoryState;

		// Token: 0x04000B6C RID: 2924
		private int nextTile = -1;

		// Token: 0x04000B6D RID: 2925
		private float nextTileCostLeftPct;

		// Token: 0x04000B6E RID: 2926
		private bool paused;

		// Token: 0x04000B6F RID: 2927
		private int destinationTile = -1;

		// Token: 0x04000B70 RID: 2928
		private CaravanArrivalAction arrivalAction;

		// Token: 0x060012B8 RID: 4792 RVA: 0x000A2426 File Offset: 0x000A0826
		public RetainedCaravanData(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060012B9 RID: 4793 RVA: 0x000A2444 File Offset: 0x000A0844
		public bool HasDestinationTile
		{
			get
			{
				return this.destinationTile != -1;
			}
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x000A2468 File Offset: 0x000A0868
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.shouldPassStoryState, "shouldPassStoryState", false, false);
			Scribe_Values.Look<int>(ref this.nextTile, "nextTile", -1, false);
			Scribe_Values.Look<float>(ref this.nextTileCostLeftPct, "nextTileCostLeftPct", -1f, false);
			Scribe_Values.Look<bool>(ref this.paused, "paused", false, false);
			Scribe_Values.Look<int>(ref this.destinationTile, "destinationTile", 0, false);
			Scribe_Deep.Look<CaravanArrivalAction>(ref this.arrivalAction, "arrivalAction", new object[0]);
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x000A24EA File Offset: 0x000A08EA
		public void Notify_GeneratedTempIncidentMapFor(Caravan caravan)
		{
			if (this.map.Parent.def.isTempIncidentMapOwner)
			{
				this.Set(caravan);
			}
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x000A2514 File Offset: 0x000A0914
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

		// Token: 0x060012BD RID: 4797 RVA: 0x000A2610 File Offset: 0x000A0A10
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
