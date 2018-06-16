using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000617 RID: 1559
	public class TransportPodsArrivalAction_LandInSpecificCell : TransportPodsArrivalAction
	{
		// Token: 0x06001F61 RID: 8033 RVA: 0x001100FF File Offset: 0x0010E4FF
		public TransportPodsArrivalAction_LandInSpecificCell()
		{
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x00110108 File Offset: 0x0010E508
		public TransportPodsArrivalAction_LandInSpecificCell(MapParent mapParent, IntVec3 cell)
		{
			this.mapParent = mapParent;
			this.cell = cell;
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x00110120 File Offset: 0x0010E520
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x00110160 File Offset: 0x0010E560
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

		// Token: 0x06001F65 RID: 8037 RVA: 0x001101CC File Offset: 0x0010E5CC
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			Thing lookTarget = TransportPodsArrivalActionUtility.GetLookTarget(pods);
			TransportPodsArrivalActionUtility.DropTravelingTransportPods(pods, this.cell, this.mapParent.Map);
			Messages.Message("MessageTransportPodsArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x00110214 File Offset: 0x0010E614
		public static bool CanLandInSpecificCell(IEnumerable<IThingHolder> pods, MapParent mapParent)
		{
			return mapParent != null && mapParent.Spawned && mapParent.HasMap && (!mapParent.EnterCooldownBlocksEntering() || FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(new object[]
			{
				mapParent.EnterCooldownDaysLeft().ToString("0.#")
			})));
		}

		// Token: 0x0400124C RID: 4684
		private MapParent mapParent;

		// Token: 0x0400124D RID: 4685
		private IntVec3 cell;
	}
}
