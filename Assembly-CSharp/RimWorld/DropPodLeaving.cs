using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020006E3 RID: 1763
	public class DropPodLeaving : Skyfaller, IActiveDropPod, IThingHolder
	{
		// Token: 0x0400155F RID: 5471
		public int groupID = -1;

		// Token: 0x04001560 RID: 5472
		public int destinationTile = -1;

		// Token: 0x04001561 RID: 5473
		public TransportPodsArrivalAction arrivalAction;

		// Token: 0x04001562 RID: 5474
		private bool alreadyLeft;

		// Token: 0x04001563 RID: 5475
		private static List<Thing> tmpActiveDropPods = new List<Thing>();

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x0600265A RID: 9818 RVA: 0x00149A10 File Offset: 0x00147E10
		// (set) Token: 0x0600265B RID: 9819 RVA: 0x00149A3B File Offset: 0x00147E3B
		public ActiveDropPodInfo Contents
		{
			get
			{
				return ((ActiveDropPod)this.innerContainer[0]).Contents;
			}
			set
			{
				((ActiveDropPod)this.innerContainer[0]).Contents = value;
			}
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x00149A58 File Offset: 0x00147E58
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.groupID, "groupID", 0, false);
			Scribe_Values.Look<int>(ref this.destinationTile, "destinationTile", 0, false);
			Scribe_Deep.Look<TransportPodsArrivalAction>(ref this.arrivalAction, "arrivalAction", new object[0]);
			Scribe_Values.Look<bool>(ref this.alreadyLeft, "alreadyLeft", false, false);
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x00149AB8 File Offset: 0x00147EB8
		protected override void LeaveMap()
		{
			if (this.alreadyLeft)
			{
				base.LeaveMap();
			}
			else if (this.groupID < 0)
			{
				Log.Error("Drop pod left the map, but its group ID is " + this.groupID, false);
				this.Destroy(DestroyMode.Vanish);
			}
			else if (this.destinationTile < 0)
			{
				Log.Error("Drop pod left the map, but its destination tile is " + this.destinationTile, false);
				this.Destroy(DestroyMode.Vanish);
			}
			else
			{
				Lord lord = TransporterUtility.FindLord(this.groupID, base.Map);
				if (lord != null)
				{
					base.Map.lordManager.RemoveLord(lord);
				}
				TravelingTransportPods travelingTransportPods = (TravelingTransportPods)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.TravelingTransportPods);
				travelingTransportPods.Tile = base.Map.Tile;
				travelingTransportPods.SetFaction(Faction.OfPlayer);
				travelingTransportPods.destinationTile = this.destinationTile;
				travelingTransportPods.arrivalAction = this.arrivalAction;
				Find.WorldObjects.Add(travelingTransportPods);
				DropPodLeaving.tmpActiveDropPods.Clear();
				DropPodLeaving.tmpActiveDropPods.AddRange(base.Map.listerThings.ThingsInGroup(ThingRequestGroup.ActiveDropPod));
				for (int i = 0; i < DropPodLeaving.tmpActiveDropPods.Count; i++)
				{
					DropPodLeaving dropPodLeaving = DropPodLeaving.tmpActiveDropPods[i] as DropPodLeaving;
					if (dropPodLeaving != null && dropPodLeaving.groupID == this.groupID)
					{
						dropPodLeaving.alreadyLeft = true;
						travelingTransportPods.AddPod(dropPodLeaving.Contents, true);
						dropPodLeaving.Contents = null;
						dropPodLeaving.Destroy(DestroyMode.Vanish);
					}
				}
			}
		}
	}
}
