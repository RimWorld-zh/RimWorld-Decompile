using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020006E1 RID: 1761
	public class DropPodLeaving : Skyfaller, IActiveDropPod, IThingHolder
	{
		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06002657 RID: 9815 RVA: 0x00149660 File Offset: 0x00147A60
		// (set) Token: 0x06002658 RID: 9816 RVA: 0x0014968B File Offset: 0x00147A8B
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

		// Token: 0x06002659 RID: 9817 RVA: 0x001496A8 File Offset: 0x00147AA8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.groupID, "groupID", 0, false);
			Scribe_Values.Look<int>(ref this.destinationTile, "destinationTile", 0, false);
			Scribe_Deep.Look<TransportPodsArrivalAction>(ref this.arrivalAction, "arrivalAction", new object[0]);
			Scribe_Values.Look<bool>(ref this.alreadyLeft, "alreadyLeft", false, false);
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x00149708 File Offset: 0x00147B08
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

		// Token: 0x0400155B RID: 5467
		public int groupID = -1;

		// Token: 0x0400155C RID: 5468
		public int destinationTile = -1;

		// Token: 0x0400155D RID: 5469
		public TransportPodsArrivalAction arrivalAction;

		// Token: 0x0400155E RID: 5470
		private bool alreadyLeft;

		// Token: 0x0400155F RID: 5471
		private static List<Thing> tmpActiveDropPods = new List<Thing>();
	}
}
