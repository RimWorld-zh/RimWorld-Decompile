using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	public class Pawn_PathFollower : IExposable
	{
		protected Pawn pawn;

		private bool moving = false;

		public IntVec3 nextCell;

		private IntVec3 lastCell;

		public float nextCellCostLeft = 0f;

		public float nextCellCostTotal = 1f;

		private int cellsUntilClamor = 0;

		private int lastMovedTick = -999999;

		private LocalTargetInfo destination;

		private PathEndMode peMode;

		public PawnPath curPath;

		public IntVec3 lastPathedTargetPosition;

		private int foundPathWhichCollidesWithPawns = -999999;

		private int foundPathWithDanger = -999999;

		private int failedToFindCloseUnoccupiedCellTicks = -999999;

		private const int MaxMoveTicks = 450;

		private const int MaxCheckAheadNodes = 20;

		private const float SnowReductionFromWalking = 0.001f;

		private const int ClamorCellsInterval = 12;

		private const int MinCostWalk = 50;

		private const int MinCostAmble = 60;

		private const float StaggerMoveSpeedFactor = 0.17f;

		private const int CheckForMovingCollidingPawnsIfCloserToTargetThanX = 30;

		private const int AttackBlockingHostilePawnAfterTicks = 180;

		public LocalTargetInfo Destination
		{
			get
			{
				return this.destination;
			}
		}

		public bool Moving
		{
			get
			{
				return this.moving;
			}
		}

		public bool MovingNow
		{
			get
			{
				return this.Moving && !this.WillCollideWithPawnOnNextPathCell();
			}
		}

		public Pawn_PathFollower(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.moving, "moving", true, false);
			Scribe_Values.Look<IntVec3>(ref this.nextCell, "nextCell", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.nextCellCostLeft, "nextCellCostLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.nextCellCostTotal, "nextCellCostInitial", 0f, false);
			Scribe_Values.Look<PathEndMode>(ref this.peMode, "peMode", PathEndMode.None, false);
			Scribe_Values.Look<int>(ref this.cellsUntilClamor, "cellsUntilClamor", 0, false);
			Scribe_Values.Look<int>(ref this.lastMovedTick, "lastMovedTick", -999999, false);
			if (this.moving)
			{
				Scribe_TargetInfo.Look(ref this.destination, "destination");
			}
		}

		public void StartPath(LocalTargetInfo dest, PathEndMode peMode)
		{
			dest = (LocalTargetInfo)GenPath.ResolvePathMode(this.pawn, dest.ToTargetInfo(this.pawn.Map), ref peMode);
			if (dest.HasThing && dest.ThingDestroyed)
			{
				Log.Error(this.pawn + " pathing to destroyed thing " + dest.Thing);
				this.PatherFailed();
			}
			else
			{
				if (!this.PawnCanOccupy(this.pawn.Position) && !this.TryRecoverFromUnwalkablePosition(dest))
					return;
				if (this.moving && this.curPath != null && this.destination == dest && this.peMode == peMode)
					return;
				if (!this.pawn.Map.reachability.CanReach(this.pawn.Position, dest, peMode, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)))
				{
					this.PatherFailed();
				}
				else
				{
					this.peMode = peMode;
					this.destination = dest;
					if (!this.IsNextCellWalkable())
					{
						this.nextCell = this.pawn.Position;
					}
					if (!this.destination.HasThing && this.pawn.Map.pawnDestinationReservationManager.MostRecentReservationFor(this.pawn) != this.destination.Cell)
					{
						this.pawn.Map.pawnDestinationReservationManager.ObsoleteAllClaimedBy(this.pawn);
					}
					if (this.AtDestinationPosition())
					{
						this.PatherArrived();
					}
					else if (this.pawn.Downed)
					{
						Log.Error(this.pawn.LabelCap + " tried to path while incapacitated. This should never happen.");
						this.PatherFailed();
					}
					else
					{
						if (this.curPath != null)
						{
							this.curPath.ReleaseToPool();
						}
						this.curPath = null;
						this.moving = true;
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
			this.nextCell = this.pawn.Position;
		}

		public void PatherTick()
		{
			if (this.WillCollideWithPawnAt(this.pawn.Position))
			{
				if (!this.FailedToFindCloseUnoccupiedCellRecently())
				{
					IntVec3 position = default(IntVec3);
					if (CellFinder.TryFindBestPawnStandCell(this.pawn, out position, true))
					{
						this.pawn.Position = position;
						this.ResetToCurrentPosition();
						if (this.moving && this.TrySetNewPath())
						{
							this.TryEnterNextPathCell();
						}
					}
					else
					{
						this.failedToFindCloseUnoccupiedCellTicks = Find.TickManager.TicksGame;
					}
				}
			}
			else if (!this.pawn.stances.FullBodyBusy)
			{
				if (this.moving && this.WillCollideWithPawnOnNextPathCell())
				{
					this.nextCellCostLeft = this.nextCellCostTotal;
					if (((this.curPath != null && this.curPath.NodesLeftCount < 30) || PawnUtility.AnyPawnBlockingPathAt(this.nextCell, this.pawn, false, true)) && !this.BestPathHadPawnsInTheWayRecently() && this.TrySetNewPath())
					{
						this.ResetToCurrentPosition();
						this.TryEnterNextPathCell();
						return;
					}
					if (Find.TickManager.TicksGame - this.lastMovedTick >= 180)
					{
						Pawn pawn = PawnUtility.PawnBlockingPathAt(this.nextCell, this.pawn, false, false);
						if (pawn != null && this.pawn.HostileTo(pawn) && this.pawn.TryGetAttackVerb(false) != null)
						{
							Job job = new Job(JobDefOf.AttackMelee, (Thing)pawn);
							job.maxNumMeleeAttacks = 1;
							job.expiryInterval = 300;
							this.pawn.jobs.StartJob(job, JobCondition.Incompletable, null, false, true, null, default(JobTag?), false);
						}
					}
				}
				else
				{
					this.lastMovedTick = Find.TickManager.TicksGame;
					if (this.nextCellCostLeft > 0.0)
					{
						this.nextCellCostLeft -= this.CostToPayThisTick();
					}
					else if (this.moving)
					{
						this.TryEnterNextPathCell();
					}
				}
			}
		}

		public void TryResumePathingAfterLoading()
		{
			if (this.moving)
			{
				this.StartPath(this.destination, this.peMode);
			}
		}

		public void Notify_Teleported_Int()
		{
			this.StopDead();
			this.ResetToCurrentPosition();
		}

		public void ResetToCurrentPosition()
		{
			this.nextCell = this.pawn.Position;
		}

		private bool PawnCanOccupy(IntVec3 c)
		{
			bool result;
			if (!c.Walkable(this.pawn.Map))
			{
				result = false;
			}
			else
			{
				Building edifice = c.GetEdifice(this.pawn.Map);
				if (edifice != null)
				{
					Building_Door building_Door = edifice as Building_Door;
					if (building_Door != null && !building_Door.PawnCanOpen(this.pawn) && !building_Door.Open)
					{
						result = false;
						goto IL_006f;
					}
				}
				result = true;
			}
			goto IL_006f;
			IL_006f:
			return result;
		}

		public Building BuildingBlockingNextPathCell()
		{
			Building edifice = this.nextCell.GetEdifice(this.pawn.Map);
			return (edifice == null || !edifice.BlocksPawn(this.pawn)) ? null : edifice;
		}

		public bool WillCollideWithPawnOnNextPathCell()
		{
			return this.WillCollideWithPawnAt(this.nextCell);
		}

		private bool IsNextCellWalkable()
		{
			return (byte)(this.nextCell.Walkable(this.pawn.Map) ? ((!this.WillCollideWithPawnAt(this.nextCell)) ? 1 : 0) : 0) != 0;
		}

		private bool WillCollideWithPawnAt(IntVec3 c)
		{
			return PawnUtility.ShouldCollideWithPawns(this.pawn) && PawnUtility.AnyPawnBlockingPathAt(c, this.pawn, false, false);
		}

		public Building_Door NextCellDoorToManuallyOpen()
		{
			Building_Door building_Door = this.pawn.Map.thingGrid.ThingAt<Building_Door>(this.nextCell);
			return (building_Door == null || !building_Door.SlowsPawns || building_Door.Open || !building_Door.PawnCanOpen(this.pawn)) ? null : building_Door;
		}

		public void PatherDraw()
		{
			if (DebugViewSettings.drawPaths && this.curPath != null && Find.Selector.IsSelected(this.pawn))
			{
				this.curPath.DrawPath(this.pawn);
			}
		}

		public bool MovedRecently(int ticks)
		{
			return Find.TickManager.TicksGame - this.lastMovedTick <= ticks;
		}

		private bool TryRecoverFromUnwalkablePosition(LocalTargetInfo originalDest)
		{
			bool flag = false;
			for (int i = 0; i < GenRadial.RadialPattern.Length; i++)
			{
				IntVec3 intVec = this.pawn.Position + GenRadial.RadialPattern[i];
				if (this.PawnCanOccupy(intVec))
				{
					Log.Warning(this.pawn + " on unwalkable cell " + this.pawn.Position + ". Teleporting to " + intVec);
					this.pawn.Position = intVec;
					this.moving = false;
					this.nextCell = this.pawn.Position;
					this.StartPath(originalDest, this.peMode);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.pawn.Destroy(DestroyMode.Vanish);
				Log.Error(this.pawn + " on unwalkable cell " + this.pawn.Position + ". Could not find walkable position nearby. Destroyed.");
			}
			return flag;
		}

		private void PatherArrived()
		{
			this.StopDead();
			if (this.pawn.jobs.curJob != null)
			{
				this.pawn.jobs.curDriver.Notify_PatherArrived();
			}
		}

		private void PatherFailed()
		{
			this.StopDead();
			this.pawn.jobs.curDriver.Notify_PatherFailed();
		}

		private void TryEnterNextPathCell()
		{
			Building building = this.BuildingBlockingNextPathCell();
			if (building != null)
			{
				Building_Door building_Door = building as Building_Door;
				if (building_Door != null && building_Door.FreePassage)
				{
					goto IL_00ad;
				}
				if (this.pawn.CurJob != null && this.pawn.CurJob.canBash)
				{
					goto IL_005e;
				}
				if (this.pawn.HostileTo(building))
					goto IL_005e;
				this.PatherFailed();
				return;
			}
			goto IL_00ad;
			IL_00ad:
			Building_Door building_Door2 = this.NextCellDoorToManuallyOpen();
			if (building_Door2 != null)
			{
				Stance_Cooldown stance_Cooldown = new Stance_Cooldown(building_Door2.TicksToOpenNow, (Thing)building_Door2, null);
				stance_Cooldown.neverAimWeapon = true;
				this.pawn.stances.SetStance(stance_Cooldown);
				building_Door2.StartManualOpenBy(this.pawn);
				if (!this.pawn.HostileTo(building_Door2))
				{
					building_Door2.FriendlyTouched();
				}
			}
			else
			{
				this.lastCell = this.pawn.Position;
				this.pawn.Position = this.nextCell;
				if (this.pawn.RaceProps.Humanlike)
				{
					this.cellsUntilClamor--;
					if (this.cellsUntilClamor <= 0)
					{
						GenClamor.DoClamor(this.pawn, 7f, ClamorType.Movement);
						this.cellsUntilClamor = 12;
					}
				}
				this.pawn.filth.Notify_EnteredNewCell();
				if (this.pawn.BodySize > 0.89999997615814209)
				{
					this.pawn.Map.snowGrid.AddDepth(this.pawn.Position, -0.001f);
				}
				Building_Door building_Door3 = this.pawn.Map.thingGrid.ThingAt<Building_Door>(this.lastCell);
				if (building_Door3 != null && !this.pawn.HostileTo(building_Door3))
				{
					building_Door3.FriendlyTouched();
					if (!building_Door3.BlockedOpenMomentary && !building_Door3.HoldOpen && building_Door3.SlowsPawns)
					{
						building_Door3.StartManualCloseBy(this.pawn);
						return;
					}
				}
				if (this.NeedNewPath() && !this.TrySetNewPath())
					return;
				if (this.AtDestinationPosition())
				{
					this.PatherArrived();
				}
				else
				{
					this.SetupMoveIntoNextCell();
				}
			}
			return;
			IL_005e:
			Job job = new Job(JobDefOf.AttackMelee, (Thing)building);
			job.expiryInterval = 300;
			this.pawn.jobs.StartJob(job, JobCondition.Incompletable, null, false, true, null, default(JobTag?), false);
		}

		private void SetupMoveIntoNextCell()
		{
			if (this.curPath.NodesLeftCount <= 1)
			{
				Log.Error(this.pawn + " at " + this.pawn.Position + " ran out of path nodes while pathing to " + this.destination + ".");
				this.PatherFailed();
			}
			else
			{
				this.nextCell = this.curPath.ConsumeNextNode();
				if (!this.nextCell.Walkable(this.pawn.Map))
				{
					Log.Error(this.pawn + " entering " + this.nextCell + " which is unwalkable.");
				}
				Building_Door building_Door = this.pawn.Map.thingGrid.ThingAt<Building_Door>(this.nextCell);
				if (building_Door != null)
				{
					building_Door.Notify_PawnApproaching(this.pawn);
				}
				int num = this.CostToMoveIntoCell(this.nextCell);
				this.nextCellCostTotal = (float)num;
				this.nextCellCostLeft = (float)num;
			}
		}

		private int CostToMoveIntoCell(IntVec3 c)
		{
			int x = c.x;
			IntVec3 position = this.pawn.Position;
			int num;
			if (x != position.x)
			{
				int z = c.z;
				IntVec3 position2 = this.pawn.Position;
				if (z == position2.z)
					goto IL_003f;
				num = this.pawn.TicksPerMoveDiagonal;
				goto IL_005c;
			}
			goto IL_003f;
			IL_003f:
			num = this.pawn.TicksPerMoveCardinal;
			goto IL_005c;
			IL_005c:
			num += this.pawn.Map.pathGrid.CalculatedCostAt(c, false, this.pawn.Position);
			Building edifice = c.GetEdifice(this.pawn.Map);
			if (edifice != null)
			{
				num += edifice.PathWalkCostFor(this.pawn);
			}
			if (num > 450)
			{
				num = 450;
			}
			if (this.pawn.jobs.curJob != null)
			{
				switch (this.pawn.jobs.curJob.locomotionUrgency)
				{
				case LocomotionUrgency.Amble:
				{
					num *= 3;
					if (num < 60)
					{
						num = 60;
					}
					break;
				}
				case LocomotionUrgency.Walk:
				{
					num *= 2;
					if (num < 50)
					{
						num = 50;
					}
					break;
				}
				case LocomotionUrgency.Jog:
				{
					num = num;
					break;
				}
				case LocomotionUrgency.Sprint:
				{
					num = Mathf.RoundToInt((float)((float)num * 0.75));
					break;
				}
				}
			}
			return Mathf.Max(num, 1);
		}

		private float CostToPayThisTick()
		{
			float num = 1f;
			if (this.pawn.stances.Staggered)
			{
				num = (float)(num * 0.17000000178813934);
			}
			if (num < this.nextCellCostTotal / 450.0)
			{
				num = (float)(this.nextCellCostTotal / 450.0);
			}
			return num;
		}

		private bool TrySetNewPath()
		{
			PawnPath pawnPath = this.GenerateNewPath();
			bool result;
			if (!pawnPath.Found)
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
				this.curPath = pawnPath;
				int num = 0;
				while (num < 20 && num < this.curPath.NodesLeftCount)
				{
					IntVec3 c = this.curPath.Peek(num);
					if (PawnUtility.ShouldCollideWithPawns(this.pawn) && PawnUtility.AnyPawnBlockingPathAt(c, this.pawn, false, false))
					{
						this.foundPathWhichCollidesWithPawns = Find.TickManager.TicksGame;
					}
					if (PawnUtility.KnownDangerAt(c, this.pawn))
					{
						this.foundPathWithDanger = Find.TickManager.TicksGame;
					}
					if (this.foundPathWhichCollidesWithPawns == Find.TickManager.TicksGame && this.foundPathWithDanger == Find.TickManager.TicksGame)
						break;
					num++;
				}
				result = true;
			}
			return result;
		}

		private PawnPath GenerateNewPath()
		{
			this.lastPathedTargetPosition = this.destination.Cell;
			return this.pawn.Map.pathFinder.FindPath(this.pawn.Position, this.destination, this.pawn, this.peMode);
		}

		private bool AtDestinationPosition()
		{
			return this.pawn.CanReachImmediate(this.destination, this.peMode);
		}

		private bool NeedNewPath()
		{
			bool result;
			if (!this.destination.IsValid || this.curPath == null || !this.curPath.Found || this.curPath.NodesLeftCount == 0)
			{
				result = true;
			}
			else if (this.destination.HasThing && this.destination.Thing.Map != this.pawn.Map)
			{
				result = true;
			}
			else if (!ReachabilityImmediate.CanReachImmediate(this.curPath.LastNode, this.destination, this.pawn.Map, this.peMode, this.pawn))
			{
				result = true;
			}
			else
			{
				if (this.lastPathedTargetPosition != this.destination.Cell)
				{
					float num = (float)(this.pawn.Position - this.destination.Cell).LengthHorizontalSquared;
					float num2 = (float)((!(num > 900.0)) ? ((!(num > 289.0)) ? ((!(num > 100.0)) ? ((!(num > 49.0)) ? 0.5 : 2.0) : 3.0) : 5.0) : 10.0);
					if ((float)(this.lastPathedTargetPosition - this.destination.Cell).LengthHorizontalSquared > num2 * num2)
					{
						result = true;
						goto IL_0341;
					}
				}
				bool flag = PawnUtility.ShouldCollideWithPawns(this.pawn);
				bool flag2 = this.curPath.NodesLeftCount < 30;
				IntVec3 other = IntVec3.Invalid;
				int num3 = 0;
				while (num3 < 20 && num3 < this.curPath.NodesLeftCount)
				{
					IntVec3 intVec = this.curPath.Peek(num3);
					if (!intVec.Walkable(this.pawn.Map))
						goto IL_01d7;
					if (flag && !this.BestPathHadPawnsInTheWayRecently() && (PawnUtility.AnyPawnBlockingPathAt(intVec, this.pawn, false, true) || (flag2 && PawnUtility.AnyPawnBlockingPathAt(intVec, this.pawn, false, false))))
					{
						goto IL_021f;
					}
					if (!this.BestPathHadDangerRecently() && PawnUtility.KnownDangerAt(intVec, this.pawn))
						goto IL_0244;
					Building_Door building_Door = intVec.GetEdifice(this.pawn.Map) as Building_Door;
					if (building_Door != null)
					{
						if (!building_Door.CanPhysicallyPass(this.pawn) && !this.pawn.HostileTo(building_Door))
							goto IL_0290;
						if (building_Door.IsForbiddenToPass(this.pawn))
							goto IL_02a9;
					}
					if (num3 != 0 && intVec.AdjacentToDiagonal(other) && (PathFinder.BlocksDiagonalMovement(intVec.x, other.z, this.pawn.Map) || PathFinder.BlocksDiagonalMovement(other.x, intVec.z, this.pawn.Map)))
					{
						goto IL_030c;
					}
					other = intVec;
					num3++;
				}
				result = false;
			}
			goto IL_0341;
			IL_01d7:
			result = true;
			goto IL_0341;
			IL_0290:
			result = true;
			goto IL_0341;
			IL_0244:
			result = true;
			goto IL_0341;
			IL_02a9:
			result = true;
			goto IL_0341;
			IL_0341:
			return result;
			IL_021f:
			result = true;
			goto IL_0341;
			IL_030c:
			result = true;
			goto IL_0341;
		}

		private bool BestPathHadPawnsInTheWayRecently()
		{
			return this.foundPathWhichCollidesWithPawns + 240 > Find.TickManager.TicksGame;
		}

		private bool BestPathHadDangerRecently()
		{
			return this.foundPathWithDanger + 240 > Find.TickManager.TicksGame;
		}

		private bool FailedToFindCloseUnoccupiedCellRecently()
		{
			return this.failedToFindCloseUnoccupiedCellTicks + 100 > Find.TickManager.TicksGame;
		}
	}
}
