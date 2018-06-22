using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A97 RID: 2711
	public class Pawn_PathFollower : IExposable
	{
		// Token: 0x06003C3F RID: 15423 RVA: 0x001FD564 File Offset: 0x001FB964
		public Pawn_PathFollower(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06003C40 RID: 15424 RVA: 0x001FD5D0 File Offset: 0x001FB9D0
		public LocalTargetInfo Destination
		{
			get
			{
				return this.destination;
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06003C41 RID: 15425 RVA: 0x001FD5EC File Offset: 0x001FB9EC
		public bool Moving
		{
			get
			{
				return this.moving;
			}
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06003C42 RID: 15426 RVA: 0x001FD608 File Offset: 0x001FBA08
		public bool MovingNow
		{
			get
			{
				return this.Moving && !this.WillCollideWithPawnOnNextPathCell();
			}
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x06003C43 RID: 15427 RVA: 0x001FD634 File Offset: 0x001FBA34
		public IntVec3 LastPassableCellInPath
		{
			get
			{
				IntVec3 result;
				if (!this.Moving || this.curPath == null)
				{
					result = IntVec3.Invalid;
				}
				else if (!this.Destination.Cell.Impassable(this.pawn.Map))
				{
					result = this.Destination.Cell;
				}
				else
				{
					List<IntVec3> nodesReversed = this.curPath.NodesReversed;
					for (int i = 0; i < nodesReversed.Count; i++)
					{
						if (!nodesReversed[i].Impassable(this.pawn.Map))
						{
							return nodesReversed[i];
						}
					}
					if (!this.pawn.Position.Impassable(this.pawn.Map))
					{
						result = this.pawn.Position;
					}
					else
					{
						result = IntVec3.Invalid;
					}
				}
				return result;
			}
		}

		// Token: 0x06003C44 RID: 15428 RVA: 0x001FD72C File Offset: 0x001FBB2C
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

		// Token: 0x06003C45 RID: 15429 RVA: 0x001FD7E8 File Offset: 0x001FBBE8
		public void StartPath(LocalTargetInfo dest, PathEndMode peMode)
		{
			dest = (LocalTargetInfo)GenPath.ResolvePathMode(this.pawn, dest.ToTargetInfo(this.pawn.Map), ref peMode);
			if (dest.HasThing && dest.ThingDestroyed)
			{
				Log.Error(this.pawn + " pathing to destroyed thing " + dest.Thing, false);
				this.PatherFailed();
			}
			else
			{
				if (!this.PawnCanOccupy(this.pawn.Position))
				{
					if (!this.TryRecoverFromUnwalkablePosition(true))
					{
						return;
					}
				}
				if (!this.moving || this.curPath == null || !(this.destination == dest) || this.peMode != peMode)
				{
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
							Log.Error(this.pawn.LabelCap + " tried to path while downed. This should never happen. curJob=" + this.pawn.CurJob.ToStringSafe<Job>(), false);
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
							this.pawn.jobs.posture = PawnPosture.Standing;
						}
					}
				}
			}
		}

		// Token: 0x06003C46 RID: 15430 RVA: 0x001FDA13 File Offset: 0x001FBE13
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

		// Token: 0x06003C47 RID: 15431 RVA: 0x001FDA4C File Offset: 0x001FBE4C
		public void PatherTick()
		{
			if (this.WillCollideWithPawnAt(this.pawn.Position))
			{
				if (!this.FailedToFindCloseUnoccupiedCellRecently())
				{
					IntVec3 intVec;
					if (CellFinder.TryFindBestPawnStandCell(this.pawn, out intVec, true) && intVec != this.pawn.Position)
					{
						this.pawn.Position = intVec;
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
					if (((this.curPath != null && this.curPath.NodesLeftCount < 30) || PawnUtility.AnyPawnBlockingPathAt(this.nextCell, this.pawn, false, true, false)) && !this.BestPathHadPawnsInTheWayRecently())
					{
						if (this.TrySetNewPath())
						{
							this.ResetToCurrentPosition();
							this.TryEnterNextPathCell();
							return;
						}
					}
					if (Find.TickManager.TicksGame - this.lastMovedTick >= 180)
					{
						Pawn pawn = PawnUtility.PawnBlockingPathAt(this.nextCell, this.pawn, false, false, false);
						if (pawn != null && this.pawn.HostileTo(pawn) && this.pawn.TryGetAttackVerb(pawn, false) != null)
						{
							Job job = new Job(JobDefOf.AttackMelee, pawn);
							job.maxNumMeleeAttacks = 1;
							job.expiryInterval = 300;
							this.pawn.jobs.StartJob(job, JobCondition.Incompletable, null, false, true, null, null, false);
						}
					}
				}
				else
				{
					this.lastMovedTick = Find.TickManager.TicksGame;
					if (this.nextCellCostLeft > 0f)
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

		// Token: 0x06003C48 RID: 15432 RVA: 0x001FDC8C File Offset: 0x001FC08C
		public void TryResumePathingAfterLoading()
		{
			if (this.moving)
			{
				this.StartPath(this.destination, this.peMode);
			}
		}

		// Token: 0x06003C49 RID: 15433 RVA: 0x001FDCAE File Offset: 0x001FC0AE
		public void Notify_Teleported_Int()
		{
			this.StopDead();
			this.ResetToCurrentPosition();
		}

		// Token: 0x06003C4A RID: 15434 RVA: 0x001FDCBD File Offset: 0x001FC0BD
		public void ResetToCurrentPosition()
		{
			this.nextCell = this.pawn.Position;
		}

		// Token: 0x06003C4B RID: 15435 RVA: 0x001FDCD4 File Offset: 0x001FC0D4
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
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06003C4C RID: 15436 RVA: 0x001FDD54 File Offset: 0x001FC154
		public Building BuildingBlockingNextPathCell()
		{
			Building edifice = this.nextCell.GetEdifice(this.pawn.Map);
			Building result;
			if (edifice != null && edifice.BlocksPawn(this.pawn))
			{
				result = edifice;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003C4D RID: 15437 RVA: 0x001FDDA0 File Offset: 0x001FC1A0
		public bool WillCollideWithPawnOnNextPathCell()
		{
			return this.WillCollideWithPawnAt(this.nextCell);
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x001FDDC4 File Offset: 0x001FC1C4
		private bool IsNextCellWalkable()
		{
			return this.nextCell.Walkable(this.pawn.Map) && !this.WillCollideWithPawnAt(this.nextCell);
		}

		// Token: 0x06003C4F RID: 15439 RVA: 0x001FDE14 File Offset: 0x001FC214
		private bool WillCollideWithPawnAt(IntVec3 c)
		{
			return PawnUtility.ShouldCollideWithPawns(this.pawn) && PawnUtility.AnyPawnBlockingPathAt(c, this.pawn, false, false, false);
		}

		// Token: 0x06003C50 RID: 15440 RVA: 0x001FDE50 File Offset: 0x001FC250
		public Building_Door NextCellDoorToManuallyOpen()
		{
			Building_Door building_Door = this.pawn.Map.thingGrid.ThingAt<Building_Door>(this.nextCell);
			Building_Door result;
			if (building_Door != null && building_Door.SlowsPawns && !building_Door.Open && building_Door.PawnCanOpen(this.pawn))
			{
				result = building_Door;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003C51 RID: 15441 RVA: 0x001FDEB7 File Offset: 0x001FC2B7
		public void PatherDraw()
		{
			if (DebugViewSettings.drawPaths && this.curPath != null && Find.Selector.IsSelected(this.pawn))
			{
				this.curPath.DrawPath(this.pawn);
			}
		}

		// Token: 0x06003C52 RID: 15442 RVA: 0x001FDEF8 File Offset: 0x001FC2F8
		public bool MovedRecently(int ticks)
		{
			return Find.TickManager.TicksGame - this.lastMovedTick <= ticks;
		}

		// Token: 0x06003C53 RID: 15443 RVA: 0x001FDF24 File Offset: 0x001FC324
		public bool TryRecoverFromUnwalkablePosition(bool error = true)
		{
			bool flag = false;
			int i = 0;
			while (i < GenRadial.RadialPattern.Length)
			{
				IntVec3 intVec = this.pawn.Position + GenRadial.RadialPattern[i];
				if (this.PawnCanOccupy(intVec))
				{
					if (intVec == this.pawn.Position)
					{
						return true;
					}
					if (error)
					{
						Log.Warning(string.Concat(new object[]
						{
							this.pawn,
							" on unwalkable cell ",
							this.pawn.Position,
							". Teleporting to ",
							intVec
						}), false);
					}
					this.pawn.Position = intVec;
					this.pawn.Notify_Teleported(true);
					flag = true;
					break;
				}
				else
				{
					i++;
				}
			}
			if (!flag)
			{
				this.pawn.Destroy(DestroyMode.Vanish);
				Log.Error(string.Concat(new object[]
				{
					this.pawn.ToStringSafe<Pawn>(),
					" on unwalkable cell ",
					this.pawn.Position,
					". Could not find walkable position nearby. Destroyed."
				}), false);
			}
			return flag;
		}

		// Token: 0x06003C54 RID: 15444 RVA: 0x001FE062 File Offset: 0x001FC462
		private void PatherArrived()
		{
			this.StopDead();
			if (this.pawn.jobs.curJob != null)
			{
				this.pawn.jobs.curDriver.Notify_PatherArrived();
			}
		}

		// Token: 0x06003C55 RID: 15445 RVA: 0x001FE095 File Offset: 0x001FC495
		private void PatherFailed()
		{
			this.StopDead();
			this.pawn.jobs.curDriver.Notify_PatherFailed();
		}

		// Token: 0x06003C56 RID: 15446 RVA: 0x001FE0B4 File Offset: 0x001FC4B4
		private void TryEnterNextPathCell()
		{
			Building building = this.BuildingBlockingNextPathCell();
			if (building != null)
			{
				Building_Door building_Door = building as Building_Door;
				if (building_Door == null || !building_Door.FreePassage)
				{
					if ((this.pawn.CurJob != null && this.pawn.CurJob.canBash) || this.pawn.HostileTo(building))
					{
						Job job = new Job(JobDefOf.AttackMelee, building);
						job.expiryInterval = 300;
						this.pawn.jobs.StartJob(job, JobCondition.Incompletable, null, false, true, null, null, false);
						return;
					}
					this.PatherFailed();
					return;
				}
			}
			Building_Door building_Door2 = this.NextCellDoorToManuallyOpen();
			if (building_Door2 != null)
			{
				Stance_Cooldown stance_Cooldown = new Stance_Cooldown(building_Door2.TicksToOpenNow, building_Door2, null);
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
						GenClamor.DoClamor(this.pawn, 7f, ClamorDefOf.Movement);
						this.cellsUntilClamor = 12;
					}
				}
				this.pawn.filth.Notify_EnteredNewCell();
				if (this.pawn.BodySize > 0.9f)
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
				if (this.NeedNewPath())
				{
					if (!this.TrySetNewPath())
					{
						return;
					}
				}
				if (this.AtDestinationPosition())
				{
					this.PatherArrived();
				}
				else
				{
					this.SetupMoveIntoNextCell();
				}
			}
		}

		// Token: 0x06003C57 RID: 15447 RVA: 0x001FE344 File Offset: 0x001FC744
		private void SetupMoveIntoNextCell()
		{
			if (this.curPath.NodesLeftCount <= 1)
			{
				Log.Error(string.Concat(new object[]
				{
					this.pawn,
					" at ",
					this.pawn.Position,
					" ran out of path nodes while pathing to ",
					this.destination,
					"."
				}), false);
				this.PatherFailed();
			}
			else
			{
				this.nextCell = this.curPath.ConsumeNextNode();
				if (!this.nextCell.Walkable(this.pawn.Map))
				{
					Log.Error(string.Concat(new object[]
					{
						this.pawn,
						" entering ",
						this.nextCell,
						" which is unwalkable."
					}), false);
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

		// Token: 0x06003C58 RID: 15448 RVA: 0x001FE474 File Offset: 0x001FC874
		private int CostToMoveIntoCell(IntVec3 c)
		{
			return Pawn_PathFollower.CostToMoveIntoCell(this.pawn, c);
		}

		// Token: 0x06003C59 RID: 15449 RVA: 0x001FE498 File Offset: 0x001FC898
		private static int CostToMoveIntoCell(Pawn pawn, IntVec3 c)
		{
			int num;
			if (c.x == pawn.Position.x || c.z == pawn.Position.z)
			{
				num = pawn.TicksPerMoveCardinal;
			}
			else
			{
				num = pawn.TicksPerMoveDiagonal;
			}
			num += pawn.Map.pathGrid.CalculatedCostAt(c, false, pawn.Position);
			Building edifice = c.GetEdifice(pawn.Map);
			if (edifice != null)
			{
				num += (int)edifice.PathWalkCostFor(pawn);
			}
			if (num > 450)
			{
				num = 450;
			}
			if (pawn.CurJob != null)
			{
				Pawn locomotionUrgencySameAs = pawn.jobs.curDriver.locomotionUrgencySameAs;
				if (locomotionUrgencySameAs != null && locomotionUrgencySameAs != pawn && locomotionUrgencySameAs.Spawned)
				{
					int num2 = Pawn_PathFollower.CostToMoveIntoCell(locomotionUrgencySameAs, c);
					if (num < num2)
					{
						num = num2;
					}
				}
				else
				{
					switch (pawn.jobs.curJob.locomotionUrgency)
					{
					case LocomotionUrgency.Amble:
						num *= 3;
						if (num < 60)
						{
							num = 60;
						}
						break;
					case LocomotionUrgency.Walk:
						num *= 2;
						if (num < 50)
						{
							num = 50;
						}
						break;
					case LocomotionUrgency.Jog:
						num = num;
						break;
					case LocomotionUrgency.Sprint:
						num = Mathf.RoundToInt((float)num * 0.75f);
						break;
					}
				}
			}
			return Mathf.Max(num, 1);
		}

		// Token: 0x06003C5A RID: 15450 RVA: 0x001FE610 File Offset: 0x001FCA10
		private float CostToPayThisTick()
		{
			float num = 1f;
			if (this.pawn.stances.Staggered)
			{
				num *= 0.17f;
			}
			if (num < this.nextCellCostTotal / 450f)
			{
				num = this.nextCellCostTotal / 450f;
			}
			return num;
		}

		// Token: 0x06003C5B RID: 15451 RVA: 0x001FE668 File Offset: 0x001FCA68
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
					if (PawnUtility.ShouldCollideWithPawns(this.pawn))
					{
						if (PawnUtility.AnyPawnBlockingPathAt(c, this.pawn, false, false, false))
						{
							this.foundPathWhichCollidesWithPawns = Find.TickManager.TicksGame;
						}
					}
					if (PawnUtility.KnownDangerAt(c, this.pawn.Map, this.pawn))
					{
						this.foundPathWithDanger = Find.TickManager.TicksGame;
					}
					if (this.foundPathWhichCollidesWithPawns == Find.TickManager.TicksGame && this.foundPathWithDanger == Find.TickManager.TicksGame)
					{
						break;
					}
					num++;
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06003C5C RID: 15452 RVA: 0x001FE788 File Offset: 0x001FCB88
		private PawnPath GenerateNewPath()
		{
			this.lastPathedTargetPosition = this.destination.Cell;
			return this.pawn.Map.pathFinder.FindPath(this.pawn.Position, this.destination, this.pawn, this.peMode);
		}

		// Token: 0x06003C5D RID: 15453 RVA: 0x001FE7E0 File Offset: 0x001FCBE0
		private bool AtDestinationPosition()
		{
			return this.pawn.CanReachImmediate(this.destination, this.peMode);
		}

		// Token: 0x06003C5E RID: 15454 RVA: 0x001FE80C File Offset: 0x001FCC0C
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
					float num2;
					if (num > 900f)
					{
						num2 = 10f;
					}
					else if (num > 289f)
					{
						num2 = 5f;
					}
					else if (num > 100f)
					{
						num2 = 3f;
					}
					else if (num > 49f)
					{
						num2 = 2f;
					}
					else
					{
						num2 = 0.5f;
					}
					if ((float)(this.lastPathedTargetPosition - this.destination.Cell).LengthHorizontalSquared > num2 * num2)
					{
						return true;
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
					{
						return true;
					}
					if (flag && !this.BestPathHadPawnsInTheWayRecently() && (PawnUtility.AnyPawnBlockingPathAt(intVec, this.pawn, false, true, false) || (flag2 && PawnUtility.AnyPawnBlockingPathAt(intVec, this.pawn, false, false, false))))
					{
						return true;
					}
					if (!this.BestPathHadDangerRecently() && PawnUtility.KnownDangerAt(intVec, this.pawn.Map, this.pawn))
					{
						return true;
					}
					Building_Door building_Door = intVec.GetEdifice(this.pawn.Map) as Building_Door;
					if (building_Door != null)
					{
						if (!building_Door.CanPhysicallyPass(this.pawn) && !this.pawn.HostileTo(building_Door))
						{
							return true;
						}
						if (building_Door.IsForbiddenToPass(this.pawn))
						{
							return true;
						}
					}
					if (num3 != 0 && intVec.AdjacentToDiagonal(other) && (PathFinder.BlocksDiagonalMovement(intVec.x, other.z, this.pawn.Map) || PathFinder.BlocksDiagonalMovement(other.x, intVec.z, this.pawn.Map)))
					{
						return true;
					}
					other = intVec;
					num3++;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06003C5F RID: 15455 RVA: 0x001FEB68 File Offset: 0x001FCF68
		private bool BestPathHadPawnsInTheWayRecently()
		{
			return this.foundPathWhichCollidesWithPawns + 240 > Find.TickManager.TicksGame;
		}

		// Token: 0x06003C60 RID: 15456 RVA: 0x001FEB98 File Offset: 0x001FCF98
		private bool BestPathHadDangerRecently()
		{
			return this.foundPathWithDanger + 240 > Find.TickManager.TicksGame;
		}

		// Token: 0x06003C61 RID: 15457 RVA: 0x001FEBC8 File Offset: 0x001FCFC8
		private bool FailedToFindCloseUnoccupiedCellRecently()
		{
			return this.failedToFindCloseUnoccupiedCellTicks + 100 > Find.TickManager.TicksGame;
		}

		// Token: 0x04002607 RID: 9735
		protected Pawn pawn;

		// Token: 0x04002608 RID: 9736
		private bool moving = false;

		// Token: 0x04002609 RID: 9737
		public IntVec3 nextCell;

		// Token: 0x0400260A RID: 9738
		private IntVec3 lastCell;

		// Token: 0x0400260B RID: 9739
		public float nextCellCostLeft = 0f;

		// Token: 0x0400260C RID: 9740
		public float nextCellCostTotal = 1f;

		// Token: 0x0400260D RID: 9741
		private int cellsUntilClamor = 0;

		// Token: 0x0400260E RID: 9742
		private int lastMovedTick = -999999;

		// Token: 0x0400260F RID: 9743
		private LocalTargetInfo destination;

		// Token: 0x04002610 RID: 9744
		private PathEndMode peMode;

		// Token: 0x04002611 RID: 9745
		public PawnPath curPath;

		// Token: 0x04002612 RID: 9746
		public IntVec3 lastPathedTargetPosition;

		// Token: 0x04002613 RID: 9747
		private int foundPathWhichCollidesWithPawns = -999999;

		// Token: 0x04002614 RID: 9748
		private int foundPathWithDanger = -999999;

		// Token: 0x04002615 RID: 9749
		private int failedToFindCloseUnoccupiedCellTicks = -999999;

		// Token: 0x04002616 RID: 9750
		private const int MaxMoveTicks = 450;

		// Token: 0x04002617 RID: 9751
		private const int MaxCheckAheadNodes = 20;

		// Token: 0x04002618 RID: 9752
		private const float SnowReductionFromWalking = 0.001f;

		// Token: 0x04002619 RID: 9753
		private const int ClamorCellsInterval = 12;

		// Token: 0x0400261A RID: 9754
		private const int MinCostWalk = 50;

		// Token: 0x0400261B RID: 9755
		private const int MinCostAmble = 60;

		// Token: 0x0400261C RID: 9756
		private const float StaggerMoveSpeedFactor = 0.17f;

		// Token: 0x0400261D RID: 9757
		private const int CheckForMovingCollidingPawnsIfCloserToTargetThanX = 30;

		// Token: 0x0400261E RID: 9758
		private const int AttackBlockingHostilePawnAfterTicks = 180;
	}
}
