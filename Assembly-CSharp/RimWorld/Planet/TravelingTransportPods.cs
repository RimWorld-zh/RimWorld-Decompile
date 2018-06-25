using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000618 RID: 1560
	public class TravelingTransportPods : WorldObject, IThingHolder
	{
		// Token: 0x04001252 RID: 4690
		public int destinationTile = -1;

		// Token: 0x04001253 RID: 4691
		public TransportPodsArrivalAction arrivalAction;

		// Token: 0x04001254 RID: 4692
		private List<ActiveDropPodInfo> pods = new List<ActiveDropPodInfo>();

		// Token: 0x04001255 RID: 4693
		private bool arrived;

		// Token: 0x04001256 RID: 4694
		private int initialTile = -1;

		// Token: 0x04001257 RID: 4695
		private float traveledPct;

		// Token: 0x04001258 RID: 4696
		private const float TravelSpeed = 0.00025f;

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06001F72 RID: 8050 RVA: 0x00110F50 File Offset: 0x0010F350
		private Vector3 Start
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.initialTile);
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06001F73 RID: 8051 RVA: 0x00110F78 File Offset: 0x0010F378
		private Vector3 End
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.destinationTile);
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06001F74 RID: 8052 RVA: 0x00110FA0 File Offset: 0x0010F3A0
		public override Vector3 DrawPos
		{
			get
			{
				return Vector3.Slerp(this.Start, this.End, this.traveledPct);
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06001F75 RID: 8053 RVA: 0x00110FCC File Offset: 0x0010F3CC
		private float TraveledPctStepPerTick
		{
			get
			{
				Vector3 start = this.Start;
				Vector3 end = this.End;
				float result;
				if (start == end)
				{
					result = 1f;
				}
				else
				{
					float num = GenMath.SphericalDistance(start.normalized, end.normalized);
					if (num == 0f)
					{
						result = 1f;
					}
					else
					{
						result = 0.00025f / num;
					}
				}
				return result;
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06001F76 RID: 8054 RVA: 0x00111038 File Offset: 0x0010F438
		private bool PodsHaveAnyPotentialCaravanOwner
		{
			get
			{
				for (int i = 0; i < this.pods.Count; i++)
				{
					ThingOwner innerContainer = this.pods[i].innerContainer;
					for (int j = 0; j < innerContainer.Count; j++)
					{
						Pawn pawn = innerContainer[j] as Pawn;
						if (pawn != null && CaravanUtility.IsOwner(pawn, base.Faction))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06001F77 RID: 8055 RVA: 0x001110C8 File Offset: 0x0010F4C8
		public bool PodsHaveAnyFreeColonist
		{
			get
			{
				for (int i = 0; i < this.pods.Count; i++)
				{
					ThingOwner innerContainer = this.pods[i].innerContainer;
					for (int j = 0; j < innerContainer.Count; j++)
					{
						Pawn pawn = innerContainer[j] as Pawn;
						if (pawn != null && pawn.IsColonist && pawn.HostFaction == null)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06001F78 RID: 8056 RVA: 0x0011115C File Offset: 0x0010F55C
		public IEnumerable<Pawn> Pawns
		{
			get
			{
				for (int i = 0; i < this.pods.Count; i++)
				{
					ThingOwner things = this.pods[i].innerContainer;
					for (int j = 0; j < things.Count; j++)
					{
						Pawn p = things[j] as Pawn;
						if (p != null)
						{
							yield return p;
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x00111188 File Offset: 0x0010F588
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<ActiveDropPodInfo>(ref this.pods, "pods", LookMode.Deep, new object[0]);
			Scribe_Values.Look<int>(ref this.destinationTile, "destinationTile", 0, false);
			Scribe_Deep.Look<TransportPodsArrivalAction>(ref this.arrivalAction, "arrivalAction", new object[0]);
			Scribe_Values.Look<bool>(ref this.arrived, "arrived", false, false);
			Scribe_Values.Look<int>(ref this.initialTile, "initialTile", 0, false);
			Scribe_Values.Look<float>(ref this.traveledPct, "traveledPct", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = 0; i < this.pods.Count; i++)
				{
					this.pods[i].parent = this;
				}
			}
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x00111252 File Offset: 0x0010F652
		public override void PostAdd()
		{
			base.PostAdd();
			this.initialTile = base.Tile;
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x00111267 File Offset: 0x0010F667
		public override void Tick()
		{
			base.Tick();
			this.traveledPct += this.TraveledPctStepPerTick;
			if (this.traveledPct >= 1f)
			{
				this.traveledPct = 1f;
				this.Arrived();
			}
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x001112A8 File Offset: 0x0010F6A8
		public void AddPod(ActiveDropPodInfo contents, bool justLeftTheMap)
		{
			contents.parent = this;
			this.pods.Add(contents);
			ThingOwner innerContainer = contents.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Pawn pawn = innerContainer[i] as Pawn;
				if (pawn != null && !pawn.IsWorldPawn())
				{
					if (!base.Spawned)
					{
						Log.Warning("Passing pawn " + pawn + " to world, but the TravelingTransportPod is not spawned. This means that WorldPawns can discard this pawn which can cause bugs.", false);
					}
					if (justLeftTheMap)
					{
						pawn.ExitMap(false, Rot4.Invalid);
					}
					else
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					}
				}
			}
			contents.savePawnsWithReferenceMode = true;
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x00111354 File Offset: 0x0010F754
		public bool ContainsPawn(Pawn p)
		{
			for (int i = 0; i < this.pods.Count; i++)
			{
				if (this.pods[i].innerContainer.Contains(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x001113AC File Offset: 0x0010F7AC
		private void Arrived()
		{
			if (!this.arrived)
			{
				this.arrived = true;
				if (this.arrivalAction == null || !this.arrivalAction.StillValid(this.pods.Cast<IThingHolder>(), this.destinationTile))
				{
					this.arrivalAction = null;
					List<Map> maps = Find.Maps;
					for (int i = 0; i < maps.Count; i++)
					{
						if (maps[i].Tile == this.destinationTile)
						{
							this.arrivalAction = new TransportPodsArrivalAction_LandInSpecificCell(maps[i].Parent, DropCellFinder.RandomDropSpot(maps[i]));
							break;
						}
					}
					if (this.arrivalAction == null)
					{
						if (TransportPodsArrivalAction_FormCaravan.CanFormCaravanAt(this.pods.Cast<IThingHolder>(), this.destinationTile))
						{
							this.arrivalAction = new TransportPodsArrivalAction_FormCaravan();
						}
						else
						{
							List<Caravan> caravans = Find.WorldObjects.Caravans;
							for (int j = 0; j < caravans.Count; j++)
							{
								if (caravans[j].Tile == this.destinationTile && TransportPodsArrivalAction_GiveToCaravan.CanGiveTo(this.pods.Cast<IThingHolder>(), caravans[j]))
								{
									this.arrivalAction = new TransportPodsArrivalAction_GiveToCaravan(caravans[j]);
									break;
								}
							}
						}
					}
				}
				if (this.arrivalAction != null && this.arrivalAction.ShouldUseLongEvent(this.pods, this.destinationTile))
				{
					LongEventHandler.QueueLongEvent(delegate()
					{
						this.DoArrivalAction();
					}, "GeneratingMapForNewEncounter", false, null);
				}
				else
				{
					this.DoArrivalAction();
				}
			}
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x00111564 File Offset: 0x0010F964
		private void DoArrivalAction()
		{
			for (int i = 0; i < this.pods.Count; i++)
			{
				this.pods[i].savePawnsWithReferenceMode = false;
				this.pods[i].parent = null;
			}
			if (this.arrivalAction != null)
			{
				try
				{
					this.arrivalAction.Arrived(this.pods, this.destinationTile);
				}
				catch (Exception arg)
				{
					Log.Error("Exception in transport pods arrival action: " + arg, false);
				}
				this.arrivalAction = null;
			}
			else
			{
				for (int j = 0; j < this.pods.Count; j++)
				{
					this.pods[j].innerContainer.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
				}
				Messages.Message("MessageTransportPodsArrivedAndLost".Translate(), new GlobalTargetInfo(this.destinationTile), MessageTypeDefOf.NegativeEvent, true);
			}
			this.pods.Clear();
			Find.WorldObjects.Remove(this);
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x00111688 File Offset: 0x0010FA88
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x001116A0 File Offset: 0x0010FAA0
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			for (int i = 0; i < this.pods.Count; i++)
			{
				outChildren.Add(this.pods[i]);
			}
		}
	}
}
