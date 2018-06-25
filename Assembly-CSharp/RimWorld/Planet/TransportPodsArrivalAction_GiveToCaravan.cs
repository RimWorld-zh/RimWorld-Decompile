using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000614 RID: 1556
	public class TransportPodsArrivalAction_GiveToCaravan : TransportPodsArrivalAction
	{
		// Token: 0x0400124B RID: 4683
		private Caravan caravan;

		// Token: 0x0400124C RID: 4684
		private static List<Thing> tmpContainedThings = new List<Thing>();

		// Token: 0x06001F55 RID: 8021 RVA: 0x00110303 File Offset: 0x0010E703
		public TransportPodsArrivalAction_GiveToCaravan()
		{
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x0011030C File Offset: 0x0010E70C
		public TransportPodsArrivalAction_GiveToCaravan(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x0011031C File Offset: 0x0010E71C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Caravan>(ref this.caravan, "caravan", false);
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x00110338 File Offset: 0x0010E738
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

		// Token: 0x06001F59 RID: 8025 RVA: 0x001103A8 File Offset: 0x0010E7A8
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

		// Token: 0x06001F5A RID: 8026 RVA: 0x00110484 File Offset: 0x0010E884
		public static FloatMenuAcceptanceReport CanGiveTo(IEnumerable<IThingHolder> pods, Caravan caravan)
		{
			return caravan != null && caravan.Spawned && caravan.IsPlayerControlled;
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x001104B8 File Offset: 0x0010E8B8
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Caravan caravan)
		{
			return TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_GiveToCaravan>(() => TransportPodsArrivalAction_GiveToCaravan.CanGiveTo(pods, caravan), () => new TransportPodsArrivalAction_GiveToCaravan(caravan), "GiveToCaravan".Translate(new object[]
			{
				caravan.Label
			}), representative, caravan.Tile);
		}
	}
}
