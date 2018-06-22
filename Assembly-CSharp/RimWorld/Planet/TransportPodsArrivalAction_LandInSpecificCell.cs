using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000613 RID: 1555
	public class TransportPodsArrivalAction_LandInSpecificCell : TransportPodsArrivalAction
	{
		// Token: 0x06001F5A RID: 8026 RVA: 0x001101CB File Offset: 0x0010E5CB
		public TransportPodsArrivalAction_LandInSpecificCell()
		{
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x001101D4 File Offset: 0x0010E5D4
		public TransportPodsArrivalAction_LandInSpecificCell(MapParent mapParent, IntVec3 cell)
		{
			this.mapParent = mapParent;
			this.cell = cell;
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x001101EC File Offset: 0x0010E5EC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x0011022C File Offset: 0x0010E62C
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

		// Token: 0x06001F5E RID: 8030 RVA: 0x00110298 File Offset: 0x0010E698
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			Thing lookTarget = TransportPodsArrivalActionUtility.GetLookTarget(pods);
			TransportPodsArrivalActionUtility.DropTravelingTransportPods(pods, this.cell, this.mapParent.Map);
			Messages.Message("MessageTransportPodsArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x001102E0 File Offset: 0x0010E6E0
		public static bool CanLandInSpecificCell(IEnumerable<IThingHolder> pods, MapParent mapParent)
		{
			return mapParent != null && mapParent.Spawned && mapParent.HasMap && (!mapParent.EnterCooldownBlocksEntering() || FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(new object[]
			{
				mapParent.EnterCooldownDaysLeft().ToString("0.#")
			})));
		}

		// Token: 0x04001249 RID: 4681
		private MapParent mapParent;

		// Token: 0x0400124A RID: 4682
		private IntVec3 cell;
	}
}
