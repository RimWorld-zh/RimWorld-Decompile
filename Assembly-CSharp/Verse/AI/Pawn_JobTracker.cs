using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using UnityEngine.Profiling;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x02000AA3 RID: 2723
	public class Pawn_JobTracker : IExposable
	{
		// Token: 0x06003C93 RID: 15507 RVA: 0x002003AC File Offset: 0x001FE7AC
		public Pawn_JobTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06003C94 RID: 15508 RVA: 0x0020042C File Offset: 0x001FE82C
		public bool HandlingFacing
		{
			get
			{
				return this.curDriver != null && this.curDriver.HandlingFacing;
			}
		}

		// Token: 0x06003C95 RID: 15509 RVA: 0x0020045C File Offset: 0x001FE85C
		public virtual void ExposeData()
		{
			Scribe_Deep.Look<Job>(ref this.curJob, "curJob", new object[0]);
			Scribe_Deep.Look<JobDriver>(ref this.curDriver, "curDriver", new object[0]);
			Scribe_Deep.Look<JobQueue>(ref this.jobQueue, "jobQueue", new object[0]);
			Scribe_Values.Look<PawnPosture>(ref this.posture, "posture", PawnPosture.Standing, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.curDriver != null)
				{
					this.curDriver.pawn = this.pawn;
					this.curDriver.job = this.curJob;
				}
			}
			else if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.curDriver == null && this.curJob != null)
				{
					Log.Warning(string.Format("Cleaning up invalid job state on {0}", this.pawn), false);
					this.EndCurrentJob(JobCondition.Errored, true);
				}
			}
		}

		// Token: 0x06003C96 RID: 15510 RVA: 0x00200544 File Offset: 0x001FE944
		public virtual void JobTrackerTick()
		{
			this.jobsGivenThisTick = 0;
			this.jobsGivenThisTickTextual = "";
			if (this.pawn.IsHashIntervalTick(30))
			{
				ThinkResult thinkResult = this.DetermineNextConstantThinkTreeJob();
				if (thinkResult.IsValid && this.ShouldStartJobFromThinkTree(thinkResult))
				{
					this.CheckLeaveJoinableLordBecauseJobIssued(thinkResult);
					this.StartJob(thinkResult.Job, JobCondition.InterruptForced, thinkResult.SourceNode, false, false, this.pawn.thinker.ConstantThinkTree, thinkResult.Tag, false);
				}
			}
			if (this.curDriver != null)
			{
				if (this.curJob.expiryInterval > 0 && (Find.TickManager.TicksGame - this.curJob.startTick) % this.curJob.expiryInterval == 0 && Find.TickManager.TicksGame != this.curJob.startTick)
				{
					if (!this.curJob.expireRequiresEnemiesNearby || PawnUtility.EnemiesAreNearby(this.pawn, 25, false))
					{
						if (this.debugLog)
						{
							this.DebugLogEvent("Job expire");
						}
						if (!this.curJob.checkOverrideOnExpire)
						{
							this.EndCurrentJob(JobCondition.Succeeded, true);
						}
						else
						{
							this.CheckForJobOverride();
						}
						this.FinalizeTick();
						return;
					}
					if (this.debugLog)
					{
						this.DebugLogEvent("Job expire skipped because there are no enemies nearby");
					}
				}
				this.curDriver.DriverTick();
			}
			if (this.curJob == null && !this.pawn.Dead && this.pawn.mindState.Active && this.CanDoAnyJob())
			{
				if (this.debugLog)
				{
					this.DebugLogEvent("Starting job from Tick because curJob == null.");
				}
				this.TryFindAndStartJob();
			}
			this.FinalizeTick();
		}

		// Token: 0x06003C97 RID: 15511 RVA: 0x00200720 File Offset: 0x001FEB20
		private void FinalizeTick()
		{
			Profiler.BeginSample("FinalizeTick");
			this.jobsGivenRecentTicks.Add(this.jobsGivenThisTick);
			this.jobsGivenRecentTicksTextual.Add(this.jobsGivenThisTickTextual);
			while (this.jobsGivenRecentTicks.Count > 10)
			{
				this.jobsGivenRecentTicks.RemoveAt(0);
				this.jobsGivenRecentTicksTextual.RemoveAt(0);
			}
			if (this.jobsGivenThisTick != 0)
			{
				int num = 0;
				for (int i = 0; i < this.jobsGivenRecentTicks.Count; i++)
				{
					num += this.jobsGivenRecentTicks[i];
				}
				if (num >= 10)
				{
					string text = this.jobsGivenRecentTicksTextual.ToCommaList(false);
					this.jobsGivenRecentTicks.Clear();
					this.jobsGivenRecentTicksTextual.Clear();
					JobUtility.TryStartErrorRecoverJob(this.pawn, string.Concat(new object[]
					{
						this.pawn.ToStringSafe<Pawn>(),
						" started ",
						10,
						" jobs in ",
						10,
						" ticks. List: ",
						text
					}), null, null);
				}
			}
			Profiler.EndSample();
		}

		// Token: 0x06003C98 RID: 15512 RVA: 0x00200850 File Offset: 0x001FEC50
		public void StartJob(Job newJob, JobCondition lastJobEndCondition = JobCondition.None, ThinkNode jobGiver = null, bool resumeCurJobAfterwards = false, bool cancelBusyStances = true, ThinkTreeDef thinkTree = null, JobTag? tag = null, bool fromQueue = false)
		{
			this.startingNewJob = true;
			try
			{
				if (!fromQueue && (!Find.TickManager.Paused || this.lastJobGivenAtFrame == RealTime.frameCount))
				{
					this.jobsGivenThisTick++;
					this.jobsGivenThisTickTextual = this.jobsGivenThisTickTextual + "(" + newJob.ToString() + ") ";
				}
				this.lastJobGivenAtFrame = RealTime.frameCount;
				if (this.jobsGivenThisTick > 10)
				{
					string text = this.jobsGivenThisTickTextual;
					this.jobsGivenThisTick = 0;
					this.jobsGivenThisTickTextual = "";
					this.startingNewJob = false;
					this.pawn.ClearReservationsForJob(newJob);
					JobUtility.TryStartErrorRecoverJob(this.pawn, string.Concat(new string[]
					{
						this.pawn.ToStringSafe<Pawn>(),
						" started 10 jobs in one tick. newJob=",
						newJob.ToStringSafe<Job>(),
						" jobGiver=",
						jobGiver.ToStringSafe<ThinkNode>(),
						" jobList=",
						text
					}), null, null);
				}
				else
				{
					if (this.debugLog)
					{
						this.DebugLogEvent(string.Concat(new object[]
						{
							"StartJob [",
							newJob,
							"] lastJobEndCondition=",
							lastJobEndCondition,
							", jobGiver=",
							jobGiver,
							", cancelBusyStances=",
							cancelBusyStances
						}));
					}
					if (cancelBusyStances && this.pawn.stances.FullBodyBusy)
					{
						this.pawn.stances.CancelBusyStanceHard();
					}
					if (this.curJob != null)
					{
						if (lastJobEndCondition == JobCondition.None)
						{
							Log.Warning(string.Concat(new object[]
							{
								this.pawn,
								" starting job ",
								newJob,
								" from JobGiver ",
								this.pawn.mindState.lastJobGiver,
								" while already having job ",
								this.curJob,
								" without a specific job end condition."
							}), false);
							lastJobEndCondition = JobCondition.InterruptForced;
						}
						if (resumeCurJobAfterwards && this.curJob.def.suspendable)
						{
							this.jobQueue.EnqueueFirst(this.curJob, null);
							if (this.debugLog)
							{
								this.DebugLogEvent("   JobQueue EnqueueFirst curJob: " + this.curJob);
							}
							this.CleanupCurrentJob(lastJobEndCondition, false, cancelBusyStances);
						}
						else
						{
							this.CleanupCurrentJob(lastJobEndCondition, true, cancelBusyStances);
						}
					}
					if (newJob == null)
					{
						Log.Warning(this.pawn + " tried to start doing a null job.", false);
					}
					else
					{
						newJob.startTick = Find.TickManager.TicksGame;
						if (this.pawn.Drafted || newJob.playerForced)
						{
							newJob.ignoreForbidden = true;
							newJob.ignoreDesignations = true;
						}
						this.curJob = newJob;
						this.pawn.mindState.lastJobGiver = jobGiver;
						this.pawn.mindState.lastJobGiverThinkTree = thinkTree;
						this.curDriver = this.curJob.MakeDriver(this.pawn);
						if (this.curDriver.TryMakePreToilReservations())
						{
							Job job = this.TryOpportunisticJob(newJob);
							if (job != null)
							{
								this.jobQueue.EnqueueFirst(newJob, null);
								this.curJob = null;
								this.curDriver = null;
								this.StartJob(job, JobCondition.None, null, false, true, null, null, false);
							}
							else
							{
								if (tag != null)
								{
									this.pawn.mindState.lastJobTag = tag.Value;
								}
								this.curDriver.SetInitialPosture();
								this.curDriver.Notify_Starting();
								this.curDriver.SetupToils();
								this.curDriver.ReadyForNextToil();
							}
						}
						else if (fromQueue)
						{
							this.EndCurrentJob(JobCondition.QueuedNoLongerValid, true);
						}
						else
						{
							Log.Warning("TryMakePreToilReservations() returned false for a non-queued job right after StartJob(). This should have been checked before. curJob=" + this.curJob.ToStringSafe<Job>(), false);
							this.EndCurrentJob(JobCondition.Errored, true);
						}
					}
				}
			}
			finally
			{
				this.startingNewJob = false;
			}
		}

		// Token: 0x06003C99 RID: 15513 RVA: 0x00200C80 File Offset: 0x001FF080
		public void EndJob(Job job, JobCondition condition)
		{
			if (this.debugLog)
			{
				this.DebugLogEvent(string.Concat(new object[]
				{
					"EndJob [",
					job,
					"] condition=",
					condition
				}));
			}
			QueuedJob queuedJob = this.jobQueue.Extract(job);
			if (queuedJob != null)
			{
				this.pawn.ClearReservationsForJob(queuedJob.job);
			}
			if (this.curJob == job)
			{
				this.EndCurrentJob(condition, true);
			}
		}

		// Token: 0x06003C9A RID: 15514 RVA: 0x00200D00 File Offset: 0x001FF100
		public void EndCurrentJob(JobCondition condition, bool startNewJob = true)
		{
			if (this.debugLog)
			{
				this.DebugLogEvent(string.Concat(new object[]
				{
					"EndCurrentJob ",
					(this.curJob == null) ? "null" : this.curJob.ToString(),
					" condition=",
					condition,
					" curToil=",
					(this.curDriver == null) ? "null_driver" : this.curDriver.CurToilIndex.ToString()
				}));
			}
			if (condition == JobCondition.Ongoing)
			{
				Log.Warning("Ending a job with Ongoing as the condition. This makes no sense.", false);
			}
			if (condition == JobCondition.Succeeded && this.curJob != null && this.curJob.def.taleOnCompletion != null)
			{
				TaleRecorder.RecordTale(this.curJob.def.taleOnCompletion, this.curDriver.TaleParameters());
			}
			Job job = this.curJob;
			this.CleanupCurrentJob(condition, true, true);
			if (startNewJob)
			{
				if (condition == JobCondition.ErroredPather || condition == JobCondition.Errored)
				{
					this.StartJob(new Job(JobDefOf.Wait, 250, false), JobCondition.None, null, false, true, null, null, false);
				}
				else if (condition == JobCondition.Succeeded && job != null && job.def != JobDefOf.Wait_MaintainPosture && !this.pawn.pather.Moving)
				{
					this.StartJob(new Job(JobDefOf.Wait_MaintainPosture, 1, false), JobCondition.None, null, false, false, null, null, false);
				}
				else
				{
					this.TryFindAndStartJob();
				}
			}
		}

		// Token: 0x06003C9B RID: 15515 RVA: 0x00200EAC File Offset: 0x001FF2AC
		private void CleanupCurrentJob(JobCondition condition, bool releaseReservations, bool cancelBusyStancesSoft = true)
		{
			if (this.debugLog)
			{
				this.DebugLogEvent(string.Concat(new object[]
				{
					"CleanupCurrentJob ",
					(this.curJob == null) ? "null" : this.curJob.def.ToString(),
					" condition ",
					condition
				}));
			}
			if (this.curJob != null)
			{
				if (releaseReservations)
				{
					this.pawn.ClearReservationsForJob(this.curJob);
				}
				if (this.curDriver != null)
				{
					this.curDriver.ended = true;
					this.curDriver.Cleanup(condition);
				}
				this.curDriver = null;
				this.curJob = null;
				this.pawn.VerifyReservations();
				if (cancelBusyStancesSoft)
				{
					this.pawn.stances.CancelBusyStanceSoft();
				}
				if (!this.pawn.Destroyed && this.pawn.carryTracker != null && this.pawn.carryTracker.CarriedThing != null)
				{
					Thing thing;
					this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out thing, null);
				}
			}
		}

		// Token: 0x06003C9C RID: 15516 RVA: 0x00200FEC File Offset: 0x001FF3EC
		public void ClearQueuedJobs()
		{
			if (this.debugLog)
			{
				this.DebugLogEvent("ClearQueuedJobs");
			}
			while (this.jobQueue.Count > 0)
			{
				this.pawn.ClearReservationsForJob(this.jobQueue.Dequeue().job);
			}
		}

		// Token: 0x06003C9D RID: 15517 RVA: 0x00201044 File Offset: 0x001FF444
		public void CheckForJobOverride()
		{
			if (this.debugLog)
			{
				this.DebugLogEvent("CheckForJobOverride");
			}
			ThinkTreeDef thinkTree;
			ThinkResult thinkResult = this.DetermineNextJob(out thinkTree);
			if (this.ShouldStartJobFromThinkTree(thinkResult))
			{
				this.CheckLeaveJoinableLordBecauseJobIssued(thinkResult);
				this.StartJob(thinkResult.Job, JobCondition.InterruptOptional, thinkResult.SourceNode, false, false, thinkTree, thinkResult.Tag, thinkResult.FromQueue);
			}
		}

		// Token: 0x06003C9E RID: 15518 RVA: 0x002010AC File Offset: 0x001FF4AC
		public void StopAll(bool ifLayingKeepLaying = false)
		{
			bool flag = this.pawn.InBed() || (this.pawn.CurJob != null && this.pawn.CurJob.def == JobDefOf.LayDown);
			if (!flag || !ifLayingKeepLaying)
			{
				this.CleanupCurrentJob(JobCondition.InterruptForced, true, true);
			}
			this.ClearQueuedJobs();
		}

		// Token: 0x06003C9F RID: 15519 RVA: 0x00201114 File Offset: 0x001FF514
		private void TryFindAndStartJob()
		{
			if (this.pawn.thinker == null)
			{
				Log.ErrorOnce(this.pawn + " did TryFindAndStartJob but had no thinker.", 8573261, false);
			}
			else
			{
				if (this.curJob != null)
				{
					Log.Warning(this.pawn + " doing TryFindAndStartJob while still having job " + this.curJob, false);
				}
				if (this.debugLog)
				{
					this.DebugLogEvent("TryFindAndStartJob");
				}
				if (!this.CanDoAnyJob())
				{
					if (this.debugLog)
					{
						this.DebugLogEvent("   CanDoAnyJob is false. Clearing queue and returning");
					}
					this.ClearQueuedJobs();
				}
				else
				{
					ThinkTreeDef thinkTreeDef;
					ThinkResult result = this.DetermineNextJob(out thinkTreeDef);
					if (result.IsValid)
					{
						this.CheckLeaveJoinableLordBecauseJobIssued(result);
						Job job = result.Job;
						ThinkNode sourceNode = result.SourceNode;
						ThinkTreeDef thinkTree = thinkTreeDef;
						this.StartJob(job, JobCondition.None, sourceNode, false, false, thinkTree, result.Tag, result.FromQueue);
					}
				}
			}
		}

		// Token: 0x06003CA0 RID: 15520 RVA: 0x0020120C File Offset: 0x001FF60C
		public Job TryOpportunisticJob(Job job)
		{
			Job result;
			if (this.pawn.Drafted)
			{
				result = null;
			}
			else if (job.playerForced)
			{
				result = null;
			}
			else if (this.pawn.Faction != Faction.OfPlayer)
			{
				result = null;
			}
			else if (this.pawn.RaceProps.intelligence < Intelligence.Humanlike)
			{
				result = null;
			}
			else if (!job.def.allowOpportunisticPrefix)
			{
				result = null;
			}
			else if (!this.pawn.workSettings.WorkIsActive(WorkTypeDefOf.Hauling))
			{
				result = null;
			}
			else if (this.pawn.InMentalState)
			{
				result = null;
			}
			else
			{
				IntVec3 cell = job.targetA.Cell;
				if (!cell.IsValid)
				{
					result = null;
				}
				else
				{
					float num = this.pawn.Position.DistanceTo(cell);
					if (num < Pawn_JobTracker.OpportunisticJobMinDistance)
					{
						result = null;
					}
					else
					{
						List<Thing> list = this.pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling();
						for (int i = 0; i < list.Count; i++)
						{
							Thing thing = list[i];
							if (this.pawn.Map.reservationManager.FirstRespectedReserver(thing, this.pawn) == null)
							{
								float num2 = this.pawn.Position.DistanceTo(thing.Position);
								if (num2 <= Pawn_JobTracker.OpportunisticJobMaxPickupDistance && num2 <= num * Pawn_JobTracker.OpportunisticJobMaxPickupDistanceFactor)
								{
									if (num2 + thing.Position.DistanceTo(cell) <= num * Pawn_JobTracker.OpportunisticJobMaxDistanceFactor)
									{
										if (HaulAIUtility.PawnCanAutomaticallyHaulFast(this.pawn, thing, false))
										{
											StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(thing);
											IntVec3 invalid = IntVec3.Invalid;
											if (StoreUtility.TryFindBestBetterStoreCellFor(thing, this.pawn, this.pawn.Map, currentPriority, this.pawn.Faction, out invalid, true))
											{
												float num3 = invalid.DistanceTo(cell);
												if (num3 <= Pawn_JobTracker.OpportunisticJobMaxDropoffDistance && num3 <= num * Pawn_JobTracker.OpportunisticJobMaxDropoffDistanceFactor)
												{
													if (num2 + thing.Position.DistanceTo(invalid) + num3 <= num * Pawn_JobTracker.OpportunisticJobMaxDistanceFactor)
													{
														if (num2 + num3 <= num)
														{
															if (this.pawn.Position.WithinRegions(thing.Position, this.pawn.Map, Pawn_JobTracker.OpportunisticJobMaxPickupRegions, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false), RegionType.Set_Passable))
															{
																if (invalid.WithinRegions(cell, this.pawn.Map, Pawn_JobTracker.OpportunisticJobMaxDropoffRegions, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false), RegionType.Set_Passable))
																{
																	if (DebugViewSettings.drawOpportunisticJobs)
																	{
																		Log.Message("Opportunistic job spawned", false);
																		this.pawn.Map.debugDrawer.FlashLine(this.pawn.Position, thing.Position, 600, SimpleColor.Red);
																		this.pawn.Map.debugDrawer.FlashLine(thing.Position, invalid, 600, SimpleColor.Green);
																		this.pawn.Map.debugDrawer.FlashLine(invalid, cell, 600, SimpleColor.Blue);
																	}
																	return HaulAIUtility.HaulToCellStorageJob(this.pawn, thing, invalid, false);
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x06003CA1 RID: 15521 RVA: 0x0020159C File Offset: 0x001FF99C
		private ThinkResult DetermineNextJob(out ThinkTreeDef thinkTree)
		{
			ThinkResult thinkResult = this.DetermineNextConstantThinkTreeJob();
			ThinkResult result;
			if (thinkResult.Job != null)
			{
				thinkTree = this.pawn.thinker.ConstantThinkTree;
				result = thinkResult;
			}
			else
			{
				ThinkResult thinkResult2 = ThinkResult.NoJob;
				try
				{
					Profiler.BeginSample("Determine next job (main)");
					thinkResult2 = this.pawn.thinker.MainThinkNodeRoot.TryIssueJobPackage(this.pawn, default(JobIssueParams));
				}
				catch (Exception exception)
				{
					JobUtility.TryStartErrorRecoverJob(this.pawn, this.pawn.ToStringSafe<Pawn>() + " threw exception while determining job (main)", exception, null);
					thinkTree = null;
					return ThinkResult.NoJob;
				}
				finally
				{
					Profiler.EndSample();
				}
				thinkTree = this.pawn.thinker.MainThinkTree;
				result = thinkResult2;
			}
			return result;
		}

		// Token: 0x06003CA2 RID: 15522 RVA: 0x00201688 File Offset: 0x001FFA88
		private ThinkResult DetermineNextConstantThinkTreeJob()
		{
			ThinkResult noJob;
			if (this.pawn.thinker.ConstantThinkTree == null)
			{
				noJob = ThinkResult.NoJob;
			}
			else
			{
				try
				{
					Profiler.BeginSample("Determine next job (constant)");
					return this.pawn.thinker.ConstantThinkNodeRoot.TryIssueJobPackage(this.pawn, default(JobIssueParams));
				}
				catch (Exception exception)
				{
					JobUtility.TryStartErrorRecoverJob(this.pawn, this.pawn.ToStringSafe<Pawn>() + " threw exception while determining job (constant)", exception, null);
				}
				finally
				{
					Profiler.EndSample();
				}
				noJob = ThinkResult.NoJob;
			}
			return noJob;
		}

		// Token: 0x06003CA3 RID: 15523 RVA: 0x00201748 File Offset: 0x001FFB48
		private void CheckLeaveJoinableLordBecauseJobIssued(ThinkResult result)
		{
			if (result.IsValid && result.SourceNode != null)
			{
				Lord lord = this.pawn.GetLord();
				if (lord != null && lord.LordJob is LordJob_VoluntarilyJoinable)
				{
					bool flag = false;
					ThinkNode thinkNode = result.SourceNode;
					while (!thinkNode.leaveJoinableLordIfIssuesJob)
					{
						thinkNode = thinkNode.parent;
						if (thinkNode == null)
						{
							IL_7C:
							if (flag)
							{
								lord.Notify_PawnLost(this.pawn, PawnLostCondition.LeftVoluntarily);
								return;
							}
							return;
						}
					}
					flag = true;
					goto IL_7C;
				}
			}
		}

		// Token: 0x06003CA4 RID: 15524 RVA: 0x002017E4 File Offset: 0x001FFBE4
		private bool CanDoAnyJob()
		{
			return this.pawn.Spawned;
		}

		// Token: 0x06003CA5 RID: 15525 RVA: 0x00201804 File Offset: 0x001FFC04
		private bool ShouldStartJobFromThinkTree(ThinkResult thinkResult)
		{
			return this.curJob == null || (this.curJob != thinkResult.Job && (thinkResult.FromQueue || (thinkResult.Job.def != this.curJob.def || thinkResult.SourceNode != this.pawn.mindState.lastJobGiver || !this.curDriver.IsContinuation(thinkResult.Job))));
		}

		// Token: 0x06003CA6 RID: 15526 RVA: 0x002018B0 File Offset: 0x001FFCB0
		public bool IsCurrentJobPlayerInterruptible()
		{
			return (this.curJob == null || this.curJob.def.playerInterruptible) && !this.pawn.HasAttachment(ThingDefOf.Fire);
		}

		// Token: 0x06003CA7 RID: 15527 RVA: 0x0020190C File Offset: 0x001FFD0C
		public bool TryTakeOrderedJobPrioritizedWork(Job job, WorkGiver giver, IntVec3 cell)
		{
			bool result;
			if (this.TryTakeOrderedJob(job, giver.def.tagToGive))
			{
				this.pawn.mindState.lastGivenWorkType = giver.def.workType;
				if (giver.def.prioritizeSustains)
				{
					this.pawn.mindState.priorityWork.Set(cell, giver.def.workType);
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06003CA8 RID: 15528 RVA: 0x00201990 File Offset: 0x001FFD90
		public bool TryTakeOrderedJob(Job job, JobTag tag = JobTag.Misc)
		{
			if (this.debugLog)
			{
				this.DebugLogEvent("TryTakeOrderedJob " + job);
			}
			job.playerForced = true;
			bool result;
			if (this.curJob != null && this.curJob.JobIsSameAs(job))
			{
				result = true;
			}
			else
			{
				bool flag = this.pawn.jobs.IsCurrentJobPlayerInterruptible();
				bool flag2 = this.pawn.mindState.IsIdle || this.pawn.CurJob == null || this.pawn.CurJob.def.isIdle;
				bool isDownEvent = KeyBindingDefOf.QueueOrder.IsDownEvent;
				if (isDownEvent)
				{
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.QueueOrders, KnowledgeAmount.NoteTaught);
				}
				if (flag && (!isDownEvent || flag2))
				{
					this.pawn.stances.CancelBusyStanceSoft();
					if (this.debugLog)
					{
						this.DebugLogEvent("    Queueing job");
					}
					this.ClearQueuedJobs();
					if (job.TryMakePreToilReservations(this.pawn))
					{
						this.jobQueue.EnqueueFirst(job, new JobTag?(tag));
						if (this.curJob != null)
						{
							this.curDriver.EndJobWith(JobCondition.InterruptForced);
						}
						else
						{
							this.CheckForJobOverride();
						}
						result = true;
					}
					else
					{
						Log.Warning("TryMakePreToilReservations() returned false right after TryTakeOrderedJob(). This should have been checked before. job=" + job.ToStringSafe<Job>(), false);
						this.pawn.ClearReservationsForJob(job);
						result = false;
					}
				}
				else if (isDownEvent)
				{
					if (job.TryMakePreToilReservations(this.pawn))
					{
						this.jobQueue.EnqueueLast(job, new JobTag?(tag));
						result = true;
					}
					else
					{
						Log.Warning("TryMakePreToilReservations() returned false right after TryTakeOrderedJob(). This should have been checked before. job=" + job.ToStringSafe<Job>(), false);
						this.pawn.ClearReservationsForJob(job);
						result = false;
					}
				}
				else
				{
					this.ClearQueuedJobs();
					if (job.TryMakePreToilReservations(this.pawn))
					{
						this.jobQueue.EnqueueLast(job, new JobTag?(tag));
						result = true;
					}
					else
					{
						Log.Warning("TryMakePreToilReservations() returned false right after TryTakeOrderedJob(). This should have been checked before. job=" + job.ToStringSafe<Job>(), false);
						this.pawn.ClearReservationsForJob(job);
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06003CA9 RID: 15529 RVA: 0x00201BC0 File Offset: 0x001FFFC0
		public void Notify_TuckedIntoBed(Building_Bed bed)
		{
			this.pawn.Position = RestUtility.GetBedSleepingSlotPosFor(this.pawn, bed);
			this.pawn.Notify_Teleported(false);
			this.pawn.stances.CancelBusyStanceHard();
			this.StartJob(new Job(JobDefOf.LayDown, bed), JobCondition.InterruptForced, null, false, true, null, new JobTag?(JobTag.TuckedIntoBed), false);
		}

		// Token: 0x06003CAA RID: 15530 RVA: 0x00201C24 File Offset: 0x00200024
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
			if (this.curJob != null)
			{
				Job job = this.curJob;
				this.curDriver.Notify_DamageTaken(dinfo);
				if (this.curJob == job)
				{
					if (dinfo.Def.externalViolence && dinfo.Def.canInterruptJobs && !this.curJob.playerForced && Find.TickManager.TicksGame >= this.lastDamageCheckTick + 180)
					{
						Thing instigator = dinfo.Instigator;
						if (this.curJob.def.checkOverrideOnDamage == CheckJobOverrideOnDamageMode.Always || (this.curJob.def.checkOverrideOnDamage == CheckJobOverrideOnDamageMode.OnlyIfInstigatorNotJobTarget && !this.curJob.AnyTargetIs(instigator)))
						{
							this.lastDamageCheckTick = Find.TickManager.TicksGame;
							this.CheckForJobOverride();
						}
					}
				}
			}
		}

		// Token: 0x06003CAB RID: 15531 RVA: 0x00201D14 File Offset: 0x00200114
		internal void Notify_MasterDraftedOrUndrafted()
		{
			Pawn master = this.pawn.playerSettings.Master;
			if (master.Spawned && master.Map == this.pawn.Map && this.pawn.playerSettings.followDrafted)
			{
				this.EndCurrentJob(JobCondition.InterruptForced, true);
			}
		}

		// Token: 0x06003CAC RID: 15532 RVA: 0x00201D74 File Offset: 0x00200174
		public void DrawLinesBetweenTargets()
		{
			Vector3 a = this.pawn.Position.ToVector3Shifted();
			if (this.pawn.pather.curPath != null)
			{
				a = this.pawn.pather.Destination.CenterVector3;
			}
			else if (this.curJob != null && this.curJob.targetA.IsValid && (!this.curJob.targetA.HasThing || this.curJob.targetA.Thing.Spawned))
			{
				GenDraw.DrawLineBetween(a, this.curJob.targetA.CenterVector3, AltitudeLayer.Item.AltitudeFor());
				a = this.curJob.targetA.CenterVector3;
			}
			for (int i = 0; i < this.jobQueue.Count; i++)
			{
				Vector3 centerVector = this.jobQueue[i].job.targetA.CenterVector3;
				if (centerVector != Vector3.zero)
				{
					GenDraw.DrawLineBetween(a, centerVector, AltitudeLayer.Item.AltitudeFor());
					a = centerVector;
				}
				else
				{
					List<LocalTargetInfo> targetQueueA = this.jobQueue[i].job.targetQueueA;
					if (!targetQueueA.NullOrEmpty<LocalTargetInfo>())
					{
						Vector3 centerVector2 = targetQueueA[0].CenterVector3;
						GenDraw.DrawLineBetween(a, centerVector2, AltitudeLayer.Item.AltitudeFor());
						a = centerVector2;
					}
				}
			}
		}

		// Token: 0x06003CAD RID: 15533 RVA: 0x00201EFC File Offset: 0x002002FC
		public void DebugLogEvent(string s)
		{
			if (this.debugLog)
			{
				Log.Message(string.Concat(new object[]
				{
					Find.TickManager.TicksGame,
					" ",
					this.pawn,
					": ",
					s
				}), false);
			}
		}

		// Token: 0x04002658 RID: 9816
		protected Pawn pawn;

		// Token: 0x04002659 RID: 9817
		public Job curJob = null;

		// Token: 0x0400265A RID: 9818
		public JobDriver curDriver = null;

		// Token: 0x0400265B RID: 9819
		public JobQueue jobQueue = new JobQueue();

		// Token: 0x0400265C RID: 9820
		public PawnPosture posture = PawnPosture.Standing;

		// Token: 0x0400265D RID: 9821
		public bool startingNewJob;

		// Token: 0x0400265E RID: 9822
		private int jobsGivenThisTick = 0;

		// Token: 0x0400265F RID: 9823
		private string jobsGivenThisTickTextual = "";

		// Token: 0x04002660 RID: 9824
		private int lastJobGivenAtFrame = -1;

		// Token: 0x04002661 RID: 9825
		private List<int> jobsGivenRecentTicks = new List<int>(10);

		// Token: 0x04002662 RID: 9826
		private List<string> jobsGivenRecentTicksTextual = new List<string>(10);

		// Token: 0x04002663 RID: 9827
		public bool debugLog = false;

		// Token: 0x04002664 RID: 9828
		private const int ConstantThinkTreeJobCheckIntervalTicks = 30;

		// Token: 0x04002665 RID: 9829
		[TweakValue("Gameplay", 1f, 3f)]
		private static float OpportunisticJobMaxDistanceFactor = 1.5f;

		// Token: 0x04002666 RID: 9830
		[TweakValue("Gameplay", 5f, 50f)]
		private static float OpportunisticJobMaxPickupDistance = 20f;

		// Token: 0x04002667 RID: 9831
		[TweakValue("Gameplay", 0f, 2f)]
		private static float OpportunisticJobMaxPickupDistanceFactor = 0.5f;

		// Token: 0x04002668 RID: 9832
		[TweakValue("Gameplay", 1f, 50f)]
		private static int OpportunisticJobMaxPickupRegions = 25;

		// Token: 0x04002669 RID: 9833
		[TweakValue("Gameplay", 5f, 50f)]
		private static float OpportunisticJobMaxDropoffDistance = 20f;

		// Token: 0x0400266A RID: 9834
		[TweakValue("Gameplay", 0f, 2f)]
		private static float OpportunisticJobMaxDropoffDistanceFactor = 0.5f;

		// Token: 0x0400266B RID: 9835
		[TweakValue("Gameplay", 1f, 50f)]
		private static int OpportunisticJobMaxDropoffRegions = 25;

		// Token: 0x0400266C RID: 9836
		[TweakValue("Gameplay", 0f, 10f)]
		private static float OpportunisticJobMinDistance = 3f;

		// Token: 0x0400266D RID: 9837
		private const int RecentJobQueueMaxLength = 10;

		// Token: 0x0400266E RID: 9838
		private const int MaxRecentJobs = 10;

		// Token: 0x0400266F RID: 9839
		private int lastDamageCheckTick = -99999;

		// Token: 0x04002670 RID: 9840
		private const int DamageCheckMinInterval = 180;
	}
}
