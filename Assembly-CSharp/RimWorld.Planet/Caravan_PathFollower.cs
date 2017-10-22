using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class Caravan_PathFollower : IExposable
	{
		private Caravan caravan;

		private bool moving;

		public int nextTile = -1;

		public float nextTileCostLeft = 0f;

		public float nextTileCostTotal = 1f;

		private int destTile;

		public CaravanArrivalAction arrivalAction;

		public WorldPath curPath;

		public int lastPathedTargetTile;

		public const int MaxMoveTicks = 120000;

		private const int MaxCheckAheadNodes = 20;

		private const int MinCostWalk = 50;

		private const int MinCostAmble = 60;

		public const float DefaultPathCostToPayPerTick = 1f;

		public int Destination
		{
			get
			{
				return this.destTile;
			}
		}

		public bool Moving
		{
			get
			{
				return this.moving && this.caravan.Spawned;
			}
		}

		public Caravan_PathFollower(Caravan caravan)
		{
			this.caravan = caravan;
		}

		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.moving, "moving", true, false);
			Scribe_Values.Look<int>(ref this.nextTile, "nextTile", 0, false);
			Scribe_Values.Look<float>(ref this.nextTileCostLeft, "nextTileCostLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.nextTileCostTotal, "nextTileCostTotal", 0f, false);
			Scribe_Values.Look<int>(ref this.destTile, "destTile", 0, false);
			Scribe_Deep.Look<CaravanArrivalAction>(ref this.arrivalAction, "arrivalAction", new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && Current.ProgramState != 0 && this.moving)
			{
				this.StartPath(this.destTile, this.arrivalAction, true);
			}
		}

		public void StartPath(int destTile, CaravanArrivalAction arrivalAction, bool repathImmediately = false)
		{
			this.caravan.autoJoinable = false;
			if (!this.IsPassable(this.caravan.Tile) && !this.TryRecoverFromUnwalkablePosition(destTile, arrivalAction))
				return;
			if (this.moving && this.curPath != null && this.destTile == destTile)
				return;
			if (!this.caravan.CanReach(destTile))
			{
				this.PatherFailed();
			}
			else
			{
				this.destTile = destTile;
				this.arrivalAction = arrivalAction;
				if (this.nextTile < 0 || !this.IsNextTilePassable())
				{
					this.nextTile = this.caravan.Tile;
				}
				if (this.AtDestinationPosition())
				{
					this.PatherArrived();
				}
				else
				{
					if (this.curPath != null)
					{
						this.curPath.ReleaseToPool();
					}
					this.curPath = null;
					this.moving = true;
					if (repathImmediately && this.TrySetNewPath() && this.nextTileCostLeft <= 0.0 && this.moving)
					{
						this.TryEnterNextPathTile();
					}
				}
			}
		}

		public void StopDead()
		{
			if (this.curPath != null)
			{
				this.curPath.ReleaseToPool();
			}
			this.curPath = null;
			this.moving = false;
			this.nextTile = this.caravan.Tile;
			this.arrivalAction = null;
			this.nextTileCostLeft = 0f;
		}

		public void PatherTick()
		{
			if (this.moving && this.arrivalAction != null && this.arrivalAction.ShouldFail)
			{
				this.StopDead();
			}
			if (!this.caravan.CantMove)
			{
				if (this.nextTileCostLeft > 0.0)
				{
					this.nextTileCostLeft -= this.CostToPayThisTick();
				}
				else if (this.moving)
				{
					this.TryEnterNextPathTile();
				}
			}
		}

		public void Notify_Teleported_Int()
		{
			this.StopDead();
			this.ResetToCurrentPosition();
		}

		public void ResetToCurrentPosition()
		{
			this.nextTile = this.caravan.Tile;
		}

		private bool IsPassable(int tile)
		{
			return !Find.World.Impassable(tile);
		}

		public bool IsNextTilePassable()
		{
			return this.IsPassable(this.nextTile);
		}

		private bool TryRecoverFromUnwalkablePosition(int originalDest, CaravanArrivalAction originalArrivalAction)
		{
			int num = default(int);
			bool result;
			if (GenWorldClosest.TryFindClosestTile(this.caravan.Tile, (Predicate<int>)((int t) => this.IsPassable(t)), out num, 2147483647, true))
			{
				Log.Warning(this.caravan + " on unwalkable tile " + this.caravan.Tile + ". Teleporting to " + num);
				this.caravan.Tile = num;
				this.moving = false;
				this.nextTile = this.caravan.Tile;
				this.StartPath(originalDest, originalArrivalAction, false);
				result = true;
			}
			else
			{
				Find.WorldObjects.Remove(this.caravan);
				Log.Error(this.caravan + " on unwalkable tile " + this.caravan.Tile + ". Could not find walkable position nearby. Removed.");
				result = false;
			}
			return result;
		}

		private void PatherArrived()
		{
			CaravanArrivalAction caravanArrivalAction = this.arrivalAction;
			this.StopDead();
			if (caravanArrivalAction != null && !caravanArrivalAction.ShouldFail)
			{
				caravanArrivalAction.Arrived(this.caravan);
			}
			else if (this.caravan.IsPlayerControlled && !this.caravan.VisibleToCameraNow())
			{
				Messages.Message("MessageCaravanArrivedAtDestination".Translate(this.caravan.Label).CapitalizeFirst(), (WorldObject)this.caravan, MessageTypeDefOf.TaskCompletion);
			}
		}

		private void PatherFailed()
		{
			this.StopDead();
		}

		private void TryEnterNextPathTile()
		{
			if (!this.IsNextTilePassable())
			{
				this.PatherFailed();
			}
			else
			{
				this.caravan.Tile = this.nextTile;
				if (this.NeedNewPath() && !this.TrySetNewPath())
					return;
				if (this.AtDestinationPosition())
				{
					this.PatherArrived();
				}
				else if (this.curPath.NodesLeftCount == 0)
				{
					Log.Error(this.caravan + " ran out of path nodes. Force-arriving.");
					this.PatherArrived();
				}
				else
				{
					this.SetupMoveIntoNextTile();
				}
			}
		}

		private void SetupMoveIntoNextTile()
		{
			if (this.curPath.NodesLeftCount < 2)
			{
				Log.Error(this.caravan + " at " + this.caravan.Tile + " ran out of path nodes while pathing to " + this.destTile + ".");
				this.PatherFailed();
			}
			else
			{
				this.nextTile = this.curPath.ConsumeNextNode();
				if (Find.World.Impassable(this.nextTile))
				{
					Log.Error(this.caravan + " entering " + this.nextTile + " which is unwalkable.");
				}
				int num = this.CostToMove(this.caravan.Tile, this.nextTile);
				this.nextTileCostTotal = (float)num;
				this.nextTileCostLeft = (float)num;
			}
		}

		private int CostToMove(int start, int end)
		{
			return Caravan_PathFollower.CostToMove(this.caravan, start, end, -1f);
		}

		public static int CostToMove(Caravan caravan, int start, int end, float yearPercent = -1f)
		{
			return Caravan_PathFollower.CostToMove(caravan.TicksPerMove, start, end, yearPercent);
		}

		public static int CostToMove(int caravanTicksPerMove, int start, int end, float yearPercent = -1f)
		{
			int num = caravanTicksPerMove + WorldPathGrid.CalculatedCostAt(end, false, yearPercent);
			num = Mathf.RoundToInt((float)num * Find.WorldGrid.GetRoadMovementMultiplierFast(start, end));
			return Mathf.Clamp(num, 1, 120000);
		}

		public static int CostToDisplay(Caravan caravan, int start, int end, float yearPercent = -1f)
		{
			int result;
			if (start != end && end != -1)
			{
				result = Caravan_PathFollower.CostToMove(caravan.TicksPerMove, start, end, yearPercent);
			}
			else
			{
				int ticksPerMove = caravan.TicksPerMove;
				ticksPerMove += WorldPathGrid.CalculatedCostAt(start, false, yearPercent);
				Tile tile = Find.WorldGrid[start];
				float num = 1f;
				if (tile.roads != null)
				{
					for (int i = 0; i < tile.roads.Count; i++)
					{
						float a = num;
						Tile.RoadLink roadLink = tile.roads[i];
						num = Mathf.Min(a, roadLink.road.movementCostMultiplier);
					}
				}
				result = Mathf.RoundToInt((float)ticksPerMove * num);
			}
			return result;
		}

		private float CostToPayThisTick()
		{
			float num = 1f;
			if (DebugSettings.fastCaravans)
			{
				num = 100f;
			}
			if (num < this.nextTileCostTotal / 120000.0)
			{
				num = (float)(this.nextTileCostTotal / 120000.0);
			}
			return num;
		}

		private bool TrySetNewPath()
		{
			WorldPath worldPath = this.GenerateNewPath();
			bool result;
			if (!worldPath.Found)
			{
				this.PatherFailed();
				result = false;
			}
			else
			{
				if (this.curPath != null)
				{
					this.curPath.ReleaseToPool();
				}
				this.curPath = worldPath;
				result = true;
			}
			return result;
		}

		private WorldPath GenerateNewPath()
		{
			int num = (!this.moving || this.nextTile < 0 || !this.IsNextTilePassable()) ? this.caravan.Tile : this.nextTile;
			this.lastPathedTargetTile = this.destTile;
			WorldPath worldPath = Find.WorldPathFinder.FindPath(num, this.destTile, this.caravan, null);
			if (worldPath.Found && num != this.caravan.Tile)
			{
				worldPath.AddNode(num);
			}
			return worldPath;
		}

		private bool AtDestinationPosition()
		{
			return this.caravan.Tile == this.destTile;
		}

		private bool NeedNewPath()
		{
			bool result;
			if (!this.moving)
			{
				result = false;
			}
			else if (this.curPath == null || !this.curPath.Found || this.curPath.NodesLeftCount == 0)
			{
				result = true;
			}
			else
			{
				int num = 0;
				while (num < 20 && num < this.curPath.NodesLeftCount)
				{
					int tileID = this.curPath.Peek(num);
					if (Find.World.Impassable(tileID))
						goto IL_006a;
					num++;
				}
				result = false;
			}
			goto IL_0096;
			IL_0096:
			return result;
			IL_006a:
			result = true;
			goto IL_0096;
		}
	}
}
