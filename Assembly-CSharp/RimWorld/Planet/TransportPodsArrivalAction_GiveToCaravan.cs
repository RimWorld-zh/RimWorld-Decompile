using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000612 RID: 1554
	public class TransportPodsArrivalAction_GiveToCaravan : TransportPodsArrivalAction
	{
		// Token: 0x04001247 RID: 4679
		private Caravan caravan;

		// Token: 0x04001248 RID: 4680
		private static List<Thing> tmpContainedThings = new List<Thing>();

		// Token: 0x06001F52 RID: 8018 RVA: 0x0010FF4B File Offset: 0x0010E34B
		public TransportPodsArrivalAction_GiveToCaravan()
		{
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x0010FF54 File Offset: 0x0010E354
		public TransportPodsArrivalAction_GiveToCaravan(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x0010FF64 File Offset: 0x0010E364
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Caravan>(ref this.caravan, "caravan", false);
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x0010FF80 File Offset: 0x0010E380
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			FloatMenuAcceptanceReport result;
			if (!floatMenuAcceptanceReport)
			{
				result = floatMenuAcceptanceReport;
			}
			else if (this.caravan != null && !Find.WorldGrid.IsNeighborOrSame(this.caravan.Tile, destinationTile))
			{
				result = false;
			}
			else
			{
				result = TransportPodsArrivalAction_GiveToCaravan.CanGiveTo(pods, this.caravan);
			}
			return result;
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x0010FFF0 File Offset: 0x0010E3F0
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			for (int i = 0; i < pods.Count; i++)
			{
				TransportPodsArrivalAction_GiveToCaravan.tmpContainedThings.Clear();
				TransportPodsArrivalAction_GiveToCaravan.tmpContainedThings.AddRange(pods[i].innerContainer);
				for (int j = 0; j < TransportPodsArrivalAction_GiveToCaravan.tmpContainedThings.Count; j++)
				{
					pods[i].innerContainer.Remove(TransportPodsArrivalAction_GiveToCaravan.tmpContainedThings[j]);
					this.caravan.AddPawnOrItem(TransportPodsArrivalAction_GiveToCaravan.tmpContainedThings[j], true);
				}
			}
			TransportPodsArrivalAction_GiveToCaravan.tmpContainedThings.Clear();
			Messages.Message("MessageTransportPodsArrivedAndAddedToCaravan".Translate(new object[]
			{
				this.caravan.Name
			}).CapitalizeFirst(), this.caravan, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x001100CC File Offset: 0x0010E4CC
		public static FloatMenuAcceptanceReport CanGiveTo(IEnumerable<IThingHolder> pods, Caravan caravan)
		{
			return caravan != null && caravan.Spawned && caravan.IsPlayerControlled;
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x00110100 File Offset: 0x0010E500
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Caravan caravan)
		{
			return TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_GiveToCaravan>(() => TransportPodsArrivalAction_GiveToCaravan.CanGiveTo(pods, caravan), () => new TransportPodsArrivalAction_GiveToCaravan(caravan), "GiveToCaravan".Translate(new object[]
			{
				caravan.Label
			}), representative, caravan.Tile);
		}
	}
}
