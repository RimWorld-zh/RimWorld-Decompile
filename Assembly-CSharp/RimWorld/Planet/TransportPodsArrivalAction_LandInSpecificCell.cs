using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000615 RID: 1557
	public class TransportPodsArrivalAction_LandInSpecificCell : TransportPodsArrivalAction
	{
		// Token: 0x0400124D RID: 4685
		private MapParent mapParent;

		// Token: 0x0400124E RID: 4686
		private IntVec3 cell;

		// Token: 0x06001F5D RID: 8029 RVA: 0x00110583 File Offset: 0x0010E983
		public TransportPodsArrivalAction_LandInSpecificCell()
		{
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x0011058C File Offset: 0x0010E98C
		public TransportPodsArrivalAction_LandInSpecificCell(MapParent mapParent, IntVec3 cell)
		{
			this.mapParent = mapParent;
			this.cell = cell;
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x001105A4 File Offset: 0x0010E9A4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x001105E4 File Offset: 0x0010E9E4
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			FloatMenuAcceptanceReport result;
			if (!floatMenuAcceptanceReport)
			{
				result = floatMenuAcceptanceReport;
			}
			else if (this.mapParent != null && this.mapParent.Tile != destinationTile)
			{
				result = false;
			}
			else
			{
				result = TransportPodsArrivalAction_LandInSpecificCell.CanLandInSpecificCell(pods, this.mapParent);
			}
			return result;
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x00110650 File Offset: 0x0010EA50
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			Thing lookTarget = TransportPodsArrivalActionUtility.GetLookTarget(pods);
			TransportPodsArrivalActionUtility.DropTravelingTransportPods(pods, this.cell, this.mapParent.Map);
			Messages.Message("MessageTransportPodsArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x00110698 File Offset: 0x0010EA98
		public static bool CanLandInSpecificCell(IEnumerable<IThingHolder> pods, MapParent mapParent)
		{
			return mapParent != null && mapParent.Spawned && mapParent.HasMap && (!mapParent.EnterCooldownBlocksEntering() || FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(new object[]
			{
				mapParent.EnterCooldownDaysLeft().ToString("0.#")
			})));
		}
	}
}
