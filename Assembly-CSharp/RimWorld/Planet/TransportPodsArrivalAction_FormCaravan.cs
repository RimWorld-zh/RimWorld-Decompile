using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000614 RID: 1556
	public class TransportPodsArrivalAction_FormCaravan : TransportPodsArrivalAction
	{
		// Token: 0x06001F50 RID: 8016 RVA: 0x0010FB18 File Offset: 0x0010DF18
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			FloatMenuAcceptanceReport result;
			if (!floatMenuAcceptanceReport)
			{
				result = floatMenuAcceptanceReport;
			}
			else
			{
				result = TransportPodsArrivalAction_FormCaravan.CanFormCaravanAt(pods, destinationTile);
			}
			return result;
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x0010FB54 File Offset: 0x0010DF54
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			TransportPodsArrivalAction_FormCaravan.tmpPawns.Clear();
			for (int i = 0; i < pods.Count; i++)
			{
				ThingOwner innerContainer = pods[i].innerContainer;
				for (int j = innerContainer.Count - 1; j >= 0; j--)
				{
					Pawn pawn = innerContainer[j] as Pawn;
					if (pawn != null)
					{
						TransportPodsArrivalAction_FormCaravan.tmpPawns.Add(pawn);
						innerContainer.Remove(pawn);
					}
				}
			}
			int startingTile;
			if (!GenWorldClosest.TryFindClosestPassableTile(tile, out startingTile))
			{
				startingTile = tile;
			}
			Caravan caravan = CaravanMaker.MakeCaravan(TransportPodsArrivalAction_FormCaravan.tmpPawns, Faction.OfPlayer, startingTile, true);
			for (int k = 0; k < pods.Count; k++)
			{
				TransportPodsArrivalAction_FormCaravan.tmpContainedThings.Clear();
				TransportPodsArrivalAction_FormCaravan.tmpContainedThings.AddRange(pods[k].innerContainer);
				for (int l = 0; l < TransportPodsArrivalAction_FormCaravan.tmpContainedThings.Count; l++)
				{
					pods[k].innerContainer.Remove(TransportPodsArrivalAction_FormCaravan.tmpContainedThings[l]);
					CaravanInventoryUtility.GiveThing(caravan, TransportPodsArrivalAction_FormCaravan.tmpContainedThings[l]);
				}
			}
			TransportPodsArrivalAction_FormCaravan.tmpPawns.Clear();
			TransportPodsArrivalAction_FormCaravan.tmpContainedThings.Clear();
			Messages.Message("MessageTransportPodsArrived".Translate(), caravan, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x0010FCBC File Offset: 0x0010E0BC
		public static bool CanFormCaravanAt(IEnumerable<IThingHolder> pods, int tile)
		{
			return TransportPodsArrivalActionUtility.AnyPotentialCaravanOwner(pods, Faction.OfPlayer) && !Find.World.Impassable(tile);
		}

		// Token: 0x04001247 RID: 4679
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04001248 RID: 4680
		private static List<Thing> tmpContainedThings = new List<Thing>();
	}
}
