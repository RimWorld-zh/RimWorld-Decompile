using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class TravelingTransportPods : WorldObject, IThingHolder
	{
		public int destinationTile = -1;

		public IntVec3 destinationCell = IntVec3.Invalid;

		public PawnsArriveMode arriveMode = PawnsArriveMode.Undecided;

		public bool attackOnArrival;

		private List<ActiveDropPodInfo> pods = new List<ActiveDropPodInfo>();

		private bool arrived;

		private int initialTile = -1;

		private float traveledPct;

		private const float TravelSpeed = 0.00025f;

		private static List<Pawn> tmpPawns = new List<Pawn>();

		private static List<Thing> tmpContainedThings = new List<Thing>();

		private Vector3 Start
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.initialTile);
			}
		}

		private Vector3 End
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.destinationTile);
			}
		}

		public override Vector3 DrawPos
		{
			get
			{
				return Vector3.Slerp(this.Start, this.End, this.traveledPct);
			}
		}

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
					result = (float)((num != 0.0) ? (0.00025000001187436283 / num) : 1.0);
				}
				return result;
			}
		}

		private bool PodsHaveAnyPotentialCaravanOwner
		{
			get
			{
				int num = 0;
				bool result;
				while (true)
				{
					if (num < this.pods.Count)
					{
						ThingOwner innerContainer = this.pods[num].innerContainer;
						for (int i = 0; i < innerContainer.Count; i++)
						{
							Pawn pawn = innerContainer[i] as Pawn;
							if (pawn != null && CaravanUtility.IsOwner(pawn, base.Faction))
								goto IL_0047;
						}
						num++;
						continue;
					}
					result = false;
					break;
					IL_0047:
					result = true;
					break;
				}
				return result;
			}
		}

		public bool PodsHaveAnyFreeColonist
		{
			get
			{
				int num = 0;
				bool result;
				while (true)
				{
					if (num < this.pods.Count)
					{
						ThingOwner innerContainer = this.pods[num].innerContainer;
						for (int i = 0; i < innerContainer.Count; i++)
						{
							Pawn pawn = innerContainer[i] as Pawn;
							if (pawn != null && pawn.IsColonist && pawn.HostFaction == null)
								goto IL_004c;
						}
						num++;
						continue;
					}
					result = false;
					break;
					IL_004c:
					result = true;
					break;
				}
				return result;
			}
		}

		public IEnumerable<Pawn> Pawns
		{
			get
			{
				for (int j = 0; j < this.pods.Count; j++)
				{
					ThingOwner things = this.pods[j].innerContainer;
					for (int i = 0; i < things.Count; i++)
					{
						Pawn p = things[i] as Pawn;
						if (p != null)
						{
							yield return p;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<ActiveDropPodInfo>(ref this.pods, "pods", LookMode.Deep, new object[0]);
			Scribe_Values.Look<int>(ref this.destinationTile, "destinationTile", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.destinationCell, "destinationCell", default(IntVec3), false);
			Scribe_Values.Look<PawnsArriveMode>(ref this.arriveMode, "arriveMode", PawnsArriveMode.Undecided, false);
			Scribe_Values.Look<bool>(ref this.attackOnArrival, "attackOnArrival", false, false);
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

		public override void PostAdd()
		{
			base.PostAdd();
			this.initialTile = base.Tile;
		}

		public override void Tick()
		{
			base.Tick();
			this.traveledPct += this.TraveledPctStepPerTick;
			if (this.traveledPct >= 1.0)
			{
				this.traveledPct = 1f;
				this.Arrived();
			}
		}

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
						Log.Warning("Passing pawn " + pawn + " to world, but the TravelingTransportPod is not spawned. This means that WorldPawns can discard this pawn which can cause bugs.");
					}
					if (justLeftTheMap)
					{
						pawn.ExitMap(false);
					}
					else
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					}
				}
			}
			contents.savePawnsWithReferenceMode = true;
		}

		public bool ContainsPawn(Pawn p)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.pods.Count)
				{
					if (this.pods[num].innerContainer.Contains(p))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		private void Arrived()
		{
			if (!this.arrived)
			{
				this.arrived = true;
				Map map = Current.Game.FindMap(this.destinationTile);
				if (map != null)
				{
					this.SpawnDropPodsInMap(map, (string)null);
				}
				else if (!this.PodsHaveAnyPotentialCaravanOwner)
				{
					Caravan caravan = Find.WorldObjects.PlayerControlledCaravanAt(this.destinationTile);
					if (caravan != null)
					{
						this.GivePodContentsToCaravan(caravan);
					}
					else
					{
						for (int i = 0; i < this.pods.Count; i++)
						{
							this.pods[i].innerContainer.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
						}
						this.RemoveAllPods();
						Find.WorldObjects.Remove(this);
						Messages.Message("MessageTransportPodsArrivedAndLost".Translate(), new GlobalTargetInfo(this.destinationTile), MessageTypeDefOf.NegativeEvent);
					}
				}
				else
				{
					MapParent mapParent = Find.WorldObjects.MapParentAt(this.destinationTile);
					if (mapParent != null && mapParent.TransportPodsCanLandAndGenerateMap && this.attackOnArrival)
					{
						LongEventHandler.QueueLongEvent((Action)delegate
						{
							Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(mapParent.Tile, mapParent.MapSizeGeneratedByTransportPodsArrival, null);
							string extraMessagePart = (string)null;
							if (mapParent.Faction != null && !mapParent.Faction.HostileTo(Faction.OfPlayer))
							{
								mapParent.Faction.SetHostileTo(Faction.OfPlayer, true);
								extraMessagePart = "MessageTransportPodsArrived_BecameHostile".Translate(mapParent.Faction.Name).CapitalizeFirst();
							}
							Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
							this.SpawnDropPodsInMap(orGenerateMap, extraMessagePart);
						}, "GeneratingMapForNewEncounter", false, null);
					}
					else
					{
						this.SpawnCaravanAtDestinationTile();
					}
				}
			}
		}

		private void SpawnDropPodsInMap(Map map, string extraMessagePart = null)
		{
			this.RemoveAllPawnsFromWorldPawns();
			IntVec3 intVec = default(IntVec3);
			if (this.destinationCell.IsValid && this.destinationCell.InBounds(map))
			{
				intVec = this.destinationCell;
			}
			else if (this.arriveMode == PawnsArriveMode.CenterDrop)
			{
				if (!DropCellFinder.TryFindRaidDropCenterClose(out intVec, map))
				{
					intVec = DropCellFinder.FindRaidDropCenterDistant(map);
				}
			}
			else
			{
				if (((this.arriveMode != PawnsArriveMode.EdgeDrop) ? this.arriveMode : PawnsArriveMode.Undecided) != 0)
				{
					Log.Warning("Unsupported arrive mode " + this.arriveMode);
				}
				intVec = DropCellFinder.FindRaidDropCenterDistant(map);
			}
			for (int i = 0; i < this.pods.Count; i++)
			{
				IntVec3 c = default(IntVec3);
				DropCellFinder.TryFindDropSpotNear(intVec, map, out c, false, true);
				this.pods[i].parent = null;
				DropPodUtility.MakeDropPodAt(c, map, this.pods[i], false);
			}
			this.RemoveAllPods();
			Find.WorldObjects.Remove(this);
			string text = "MessageTransportPodsArrived".Translate();
			if (extraMessagePart != null)
			{
				text = text + " " + extraMessagePart;
			}
			Messages.Message(text, new TargetInfo(intVec, map, false), MessageTypeDefOf.TaskCompletion);
		}

		private void SpawnCaravanAtDestinationTile()
		{
			TravelingTransportPods.tmpPawns.Clear();
			for (int i = 0; i < this.pods.Count; i++)
			{
				ThingOwner innerContainer = this.pods[i].innerContainer;
				for (int num = innerContainer.Count - 1; num >= 0; num--)
				{
					Pawn pawn = innerContainer[num] as Pawn;
					if (pawn != null)
					{
						TravelingTransportPods.tmpPawns.Add(pawn);
						innerContainer.Remove(pawn);
					}
				}
			}
			int startingTile = default(int);
			if (!GenWorldClosest.TryFindClosestPassableTile(this.destinationTile, out startingTile))
			{
				startingTile = this.destinationTile;
			}
			Caravan o = CaravanMaker.MakeCaravan(TravelingTransportPods.tmpPawns, base.Faction, startingTile, true);
			for (int j = 0; j < this.pods.Count; j++)
			{
				TravelingTransportPods.tmpContainedThings.Clear();
				TravelingTransportPods.tmpContainedThings.AddRange(this.pods[j].innerContainer);
				for (int k = 0; k < TravelingTransportPods.tmpContainedThings.Count; k++)
				{
					this.pods[j].innerContainer.Remove(TravelingTransportPods.tmpContainedThings[k]);
					Pawn pawn2 = CaravanInventoryUtility.FindPawnToMoveInventoryTo(TravelingTransportPods.tmpContainedThings[k], TravelingTransportPods.tmpPawns, null, null);
					bool flag = false;
					if (pawn2 != null)
					{
						flag = pawn2.inventory.innerContainer.TryAdd(TravelingTransportPods.tmpContainedThings[k], true);
					}
					if (!flag)
					{
						TravelingTransportPods.tmpContainedThings[k].Destroy(DestroyMode.Vanish);
					}
				}
			}
			this.RemoveAllPods();
			Find.WorldObjects.Remove(this);
			TravelingTransportPods.tmpPawns.Clear();
			TravelingTransportPods.tmpContainedThings.Clear();
			Messages.Message("MessageTransportPodsArrived".Translate(), (WorldObject)o, MessageTypeDefOf.TaskCompletion);
		}

		private void GivePodContentsToCaravan(Caravan caravan)
		{
			for (int i = 0; i < this.pods.Count; i++)
			{
				TravelingTransportPods.tmpContainedThings.Clear();
				TravelingTransportPods.tmpContainedThings.AddRange(this.pods[i].innerContainer);
				for (int j = 0; j < TravelingTransportPods.tmpContainedThings.Count; j++)
				{
					this.pods[i].innerContainer.Remove(TravelingTransportPods.tmpContainedThings[j]);
					Pawn pawn = TravelingTransportPods.tmpContainedThings[j] as Pawn;
					if (pawn != null)
					{
						caravan.AddPawn(pawn, true);
					}
					else
					{
						Pawn pawn2 = CaravanInventoryUtility.FindPawnToMoveInventoryTo(TravelingTransportPods.tmpContainedThings[j], caravan.PawnsListForReading, null, null);
						bool flag = false;
						if (pawn2 != null)
						{
							flag = pawn2.inventory.innerContainer.TryAdd(TravelingTransportPods.tmpContainedThings[j], true);
						}
						if (!flag)
						{
							TravelingTransportPods.tmpContainedThings[j].Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
			this.RemoveAllPods();
			Find.WorldObjects.Remove(this);
			TravelingTransportPods.tmpContainedThings.Clear();
			Messages.Message("MessageTransportPodsArrivedAndAddedToCaravan".Translate(), (WorldObject)caravan, MessageTypeDefOf.TaskCompletion);
		}

		private void RemoveAllPawnsFromWorldPawns()
		{
			for (int i = 0; i < this.pods.Count; i++)
			{
				ThingOwner innerContainer = this.pods[i].innerContainer;
				for (int j = 0; j < innerContainer.Count; j++)
				{
					Pawn pawn = innerContainer[j] as Pawn;
					if (pawn != null && pawn.IsWorldPawn())
					{
						Find.WorldPawns.RemovePawn(pawn);
					}
				}
			}
		}

		private void RemoveAllPods()
		{
			for (int i = 0; i < this.pods.Count; i++)
			{
				this.pods[i].savePawnsWithReferenceMode = false;
				this.pods[i].parent = null;
			}
			this.pods.Clear();
		}

		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

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
