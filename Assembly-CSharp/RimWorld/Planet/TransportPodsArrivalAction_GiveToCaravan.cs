using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000616 RID: 1558
	public class TransportPodsArrivalAction_GiveToCaravan : TransportPodsArrivalAction
	{
		// Token: 0x06001F5B RID: 8027 RVA: 0x0010FEF7 File Offset: 0x0010E2F7
		public TransportPodsArrivalAction_GiveToCaravan()
		{
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x0010FF00 File Offset: 0x0010E300
		public TransportPodsArrivalAction_GiveToCaravan(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x0010FF10 File Offset: 0x0010E310
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Caravan>(ref this.caravan, "caravan", false);
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x0010FF2C File Offset: 0x0010E32C
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

		// Token: 0x06001F5F RID: 8031 RVA: 0x0010FF9C File Offset: 0x0010E39C
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

		// Token: 0x06001F60 RID: 8032 RVA: 0x00110078 File Offset: 0x0010E478
		public static FloatMenuAcceptanceReport CanGiveTo(IEnumerable<IThingHolder> pods, Caravan caravan)
		{
			return caravan != null && caravan.Spawned && caravan.IsPlayerControlled;
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x001100AC File Offset: 0x0010E4AC
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Caravan caravan)
		{
			return TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_GiveToCaravan>(() => TransportPodsArrivalAction_GiveToCaravan.CanGiveTo(pods, caravan), () => new TransportPodsArrivalAction_GiveToCaravan(caravan), "GiveToCaravan".Translate(new object[]
			{
				caravan.Label
			}), representative, caravan.Tile);
		}

		// Token: 0x0400124A RID: 4682
		private Caravan caravan;

		// Token: 0x0400124B RID: 4683
		private static List<Thing> tmpContainedThings = new List<Thing>();
	}
}
