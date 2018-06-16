using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A45 RID: 2629
	public abstract class JobDriver : IExposable, IJobEndable
	{
		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06003A53 RID: 14931 RVA: 0x00010F04 File Offset: 0x0000F304
		protected Toil CurToil
		{
			get
			{
				Toil result;
				if (this.curToilIndex < 0 || this.pawn.CurJob != this.job)
				{
					result = null;
				}
				else if (this.curToilIndex >= this.toils.Count)
				{
					Log.Error(string.Concat(new object[]
					{
						this.pawn,
						" with job ",
						this.pawn.CurJob,
						" tried to get CurToil with curToilIndex=",
						this.curToilIndex,
						" but only has ",
						this.toils.Count,
						" toils."
					}), false);
					result = null;
				}
				else
				{
					result = this.toils[this.curToilIndex];
				}
				return result;
			}
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06003A54 RID: 14932 RVA: 0x00010FDC File Offset: 0x0000F3DC
		protected bool HaveCurToil
		{
			get
			{
				return this.curToilIndex >= 0 && this.curToilIndex < this.toils.Count;
			}
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06003A55 RID: 14933 RVA: 0x00011014 File Offset: 0x0000F414
		private bool CanStartNextToilInBusyStance
		{
			get
			{
				int num = this.curToilIndex + 1;
				return num < this.toils.Count && this.toils[num].atomicWithPrevious;
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06003A56 RID: 14934 RVA: 0x0001105C File Offset: 0x0000F45C
		public int CurToilIndex
		{
			get
			{
				return this.curToilIndex;
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06003A57 RID: 14935 RVA: 0x00011078 File Offset: 0x0000F478
		public bool OnLastToil
		{
			get
			{
				return this.CurToilIndex == this.toils.Count - 1;
			}
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06003A58 RID: 14936 RVA: 0x000110A4 File Offset: 0x0000F4A4
		public SkillDef ActiveSkill
		{
			get
			{
				return (!this.HaveCurToil || this.CurToil.activeSkill == null) ? null : this.CurToil.activeSkill();
			}
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06003A59 RID: 14937 RVA: 0x000110EC File Offset: 0x0000F4EC
		public bool HandlingFacing
		{
			get
			{
				return this.CurToil != null && this.CurToil.handlingFacing;
			}
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06003A5A RID: 14938 RVA: 0x0001111C File Offset: 0x0000F51C
		protected LocalTargetInfo TargetA
		{
			get
			{
				return this.job.targetA;
			}
		}

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06003A5B RID: 14939 RVA: 0x0001113C File Offset: 0x0000F53C
		protected LocalTargetInfo TargetB
		{
			get
			{
				return this.job.targetB;
			}
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06003A5C RID: 14940 RVA: 0x0001115C File Offset: 0x0000F55C
		protected LocalTargetInfo TargetC
		{
			get
			{
				return this.job.targetC;
			}
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06003A5D RID: 14941 RVA: 0x0001117C File Offset: 0x0000F57C
		// (set) Token: 0x06003A5E RID: 14942 RVA: 0x000111A1 File Offset: 0x0000F5A1
		protected Thing TargetThingA
		{
			get
			{
				return this.job.targetA.Thing;
			}
			set
			{
				this.job.targetA = value;
			}
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06003A5F RID: 14943 RVA: 0x000111B8 File Offset: 0x0000F5B8
		// (set) Token: 0x06003A60 RID: 14944 RVA: 0x000111DD File Offset: 0x0000F5DD
		protected Thing TargetThingB
		{
			get
			{
				return this.job.targetB.Thing;
			}
			set
			{
				this.job.targetB = value;
			}
		}

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06003A61 RID: 14945 RVA: 0x000111F4 File Offset: 0x0000F5F4
		protected IntVec3 TargetLocA
		{
			get
			{
				return this.job.targetA.Cell;
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06003A62 RID: 14946 RVA: 0x0001121C File Offset: 0x0000F61C
		protected Map Map
		{
			get
			{
				return this.pawn.Map;
			}
		}

		// Token: 0x06003A63 RID: 14947 RVA: 0x0001123C File Offset: 0x0000F63C
		public virtual string GetReport()
		{
			return this.ReportStringProcessed(this.job.def.reportString);
		}

		// Token: 0x06003A64 RID: 14948 RVA: 0x00011268 File Offset: 0x0000F668
		protected string ReportStringProcessed(string str)
		{
			LocalTargetInfo localTargetInfo = LocalTargetInfo.Invalid;
			if (this.job.targetA.IsValid)
			{
				localTargetInfo = this.job.targetA;
			}
			else
			{
				localTargetInfo = this.job.targetQueueA.FirstValid();
			}
			if (!localTargetInfo.IsValid)
			{
				str = str.Replace("TargetA", "UnknownLower".Translate());
			}
			else if (localTargetInfo.HasThing)
			{
				str = str.Replace("TargetA", localTargetInfo.Thing.LabelShort);
			}
			else
			{
				str = str.Replace("TargetA", "AreaLower".Translate());
			}
			LocalTargetInfo localTargetInfo2 = LocalTargetInfo.Invalid;
			if (this.job.targetB.IsValid)
			{
				localTargetInfo2 = this.job.targetB;
			}
			else
			{
				localTargetInfo2 = this.job.targetQueueB.FirstValid();
			}
			if (!localTargetInfo2.IsValid)
			{
				str = str.Replace("TargetB", "UnknownLower".Translate());
			}
			else if (localTargetInfo2.HasThing)
			{
				str = str.Replace("TargetB", localTargetInfo2.Thing.LabelShort);
			}
			else
			{
				str = str.Replace("TargetB", "AreaLower".Translate());
			}
			LocalTargetInfo targetC = this.job.targetC;
			if (!targetC.IsValid)
			{
				str = str.Replace("TargetC", "UnknownLower".Translate());
			}
			else if (targetC.HasThing)
			{
				str = str.Replace("TargetC", targetC.Thing.LabelShort);
			}
			else
			{
				str = str.Replace("TargetC", "AreaLower".Translate());
			}
			return str;
		}

		// Token: 0x06003A65 RID: 14949
		public abstract bool TryMakePreToilReservations();

		// Token: 0x06003A66 RID: 14950
		protected abstract IEnumerable<Toil> MakeNewToils();

		// Token: 0x06003A67 RID: 14951 RVA: 0x00011443 File Offset: 0x0000F843
		public virtual void SetInitialPosture()
		{
			this.pawn.jobs.posture = PawnPosture.Standing;
		}

		// Token: 0x06003A68 RID: 14952 RVA: 0x00011458 File Offset: 0x0000F858
		public virtual void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.ended, "ended", false, false);
			Scribe_Values.Look<int>(ref this.curToilIndex, "curToilIndex", 0, true);
			Scribe_Values.Look<int>(ref this.ticksLeftThisToil, "ticksLeftThisToil", 0, false);
			Scribe_Values.Look<bool>(ref this.wantBeginNextToil, "wantBeginNextToil", false, false);
			Scribe_Values.Look<ToilCompleteMode>(ref this.curToilCompleteMode, "curToilCompleteMode", ToilCompleteMode.Undefined, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<TargetIndex>(ref this.rotateToFace, "rotateToFace", TargetIndex.A, false);
			Scribe_Values.Look<bool>(ref this.asleep, "asleep", false, false);
			Scribe_Values.Look<float>(ref this.uninstallWorkLeft, "uninstallWorkLeft", 0f, false);
			Scribe_Values.Look<int>(ref this.nextToilIndex, "nextToilIndex", -1, false);
			Scribe_Values.Look<bool>(ref this.collideWithPawns, "collideWithPawns", false, false);
			Scribe_References.Look<Pawn>(ref this.locomotionUrgencySameAs, "locomotionUrgencySameAs", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.SetupToils();
			}
		}

		// Token: 0x06003A69 RID: 14953 RVA: 0x00011554 File Offset: 0x0000F954
		public void Cleanup(JobCondition condition)
		{
			for (int i = 0; i < this.globalFinishActions.Count; i++)
			{
				try
				{
					this.globalFinishActions[i]();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Pawn ",
						this.pawn.ToStringSafe<Pawn>(),
						" threw exception while executing a global finish action (",
						i,
						"), jobDriver=",
						this.ToStringSafe<JobDriver>(),
						", job=",
						this.job.ToStringSafe<Job>(),
						": ",
						ex
					}), false);
				}
			}
			if (this.HaveCurToil)
			{
				this.CurToil.Cleanup(this.curToilIndex, this);
			}
		}

		// Token: 0x06003A6A RID: 14954 RVA: 0x0001163C File Offset: 0x0000FA3C
		public virtual bool CanBeginNowWhileLyingDown()
		{
			return false;
		}

		// Token: 0x06003A6B RID: 14955 RVA: 0x00011654 File Offset: 0x0000FA54
		internal void SetupToils()
		{
			try
			{
				this.toils.Clear();
				foreach (Toil toil in this.MakeNewToils())
				{
					if (toil.defaultCompleteMode == ToilCompleteMode.Undefined)
					{
						Log.Error("Toil has undefined complete mode.", false);
						toil.defaultCompleteMode = ToilCompleteMode.Instant;
					}
					toil.actor = this.pawn;
					this.toils.Add(toil);
				}
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, "Exception in SetupToils for pawn " + this.pawn.ToStringSafe<Pawn>(), exception, this);
			}
		}

		// Token: 0x06003A6C RID: 14956 RVA: 0x0001172C File Offset: 0x0000FB2C
		public void DriverTick()
		{
			try
			{
				this.ticksLeftThisToil--;
				this.debugTicksSpentThisToil++;
				if (this.CurToil == null)
				{
					if (!this.pawn.stances.FullBodyBusy || this.CanStartNextToilInBusyStance)
					{
						this.ReadyForNextToil();
					}
				}
				else if (!this.CheckCurrentToilEndOrFail())
				{
					if (this.curToilCompleteMode == ToilCompleteMode.Delay)
					{
						if (this.ticksLeftThisToil <= 0)
						{
							this.ReadyForNextToil();
							return;
						}
					}
					else if (this.curToilCompleteMode == ToilCompleteMode.FinishedBusy)
					{
						if (!this.pawn.stances.FullBodyBusy)
						{
							this.ReadyForNextToil();
							return;
						}
					}
					if (this.wantBeginNextToil)
					{
						this.TryActuallyStartNextToil();
					}
					else if (this.curToilCompleteMode == ToilCompleteMode.Instant && this.debugTicksSpentThisToil > 300)
					{
						Log.Error(string.Concat(new object[]
						{
							this.pawn,
							" had to be broken from frozen state. He was doing job ",
							this.job,
							", toilindex=",
							this.curToilIndex
						}), false);
						this.ReadyForNextToil();
					}
					else
					{
						Job curJob = this.pawn.CurJob;
						if (this.CurToil.preTickActions != null)
						{
							Toil curToil = this.CurToil;
							for (int i = 0; i < curToil.preTickActions.Count; i++)
							{
								curToil.preTickActions[i]();
								if (this.pawn.CurJob != curJob)
								{
									return;
								}
								if (this.CurToil != curToil || this.wantBeginNextToil)
								{
									return;
								}
							}
						}
						if (this.CurToil.tickAction != null)
						{
							this.CurToil.tickAction();
						}
					}
				}
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, "Exception in JobDriver tick for pawn " + this.pawn.ToStringSafe<Pawn>(), exception, this);
			}
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x00011970 File Offset: 0x0000FD70
		public void ReadyForNextToil()
		{
			this.wantBeginNextToil = true;
			this.TryActuallyStartNextToil();
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x00011980 File Offset: 0x0000FD80
		private void TryActuallyStartNextToil()
		{
			if (this.pawn.Spawned)
			{
				if (!this.pawn.stances.FullBodyBusy || this.CanStartNextToilInBusyStance)
				{
					if (this.HaveCurToil)
					{
						this.CurToil.Cleanup(this.curToilIndex, this);
					}
					if (this.nextToilIndex >= 0)
					{
						this.curToilIndex = this.nextToilIndex;
						this.nextToilIndex = -1;
					}
					else
					{
						this.curToilIndex++;
					}
					this.wantBeginNextToil = false;
					if (!this.HaveCurToil)
					{
						if (this.pawn.stances != null && this.pawn.stances.curStance.StanceBusy)
						{
							Log.ErrorOnce(this.pawn.ToStringSafe<Pawn>() + " ended job " + this.job.ToStringSafe<Job>() + " due to running out of toils during a busy stance.", 6453432, false);
						}
						this.EndJobWith(JobCondition.Succeeded);
					}
					else
					{
						this.debugTicksSpentThisToil = 0;
						this.ticksLeftThisToil = this.CurToil.defaultDuration;
						this.curToilCompleteMode = this.CurToil.defaultCompleteMode;
						if (!this.CheckCurrentToilEndOrFail())
						{
							Toil curToil = this.CurToil;
							if (this.CurToil.preInitActions != null)
							{
								for (int i = 0; i < this.CurToil.preInitActions.Count; i++)
								{
									try
									{
										this.CurToil.preInitActions[i]();
									}
									catch (Exception exception)
									{
										JobUtility.TryStartErrorRecoverJob(this.pawn, string.Concat(new object[]
										{
											"JobDriver threw exception in preInitActions[",
											i,
											"] for pawn ",
											this.pawn.ToStringSafe<Pawn>()
										}), exception, this);
										return;
									}
									if (this.CurToil != curToil)
									{
										break;
									}
								}
							}
							if (this.CurToil == curToil)
							{
								if (this.CurToil.initAction != null)
								{
									try
									{
										this.CurToil.initAction();
									}
									catch (Exception exception2)
									{
										JobUtility.TryStartErrorRecoverJob(this.pawn, "JobDriver threw exception in initAction for pawn " + this.pawn.ToStringSafe<Pawn>(), exception2, this);
										return;
									}
								}
								if (this.CurToil == curToil && !this.ended && this.curToilCompleteMode == ToilCompleteMode.Instant)
								{
									this.ReadyForNextToil();
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003A6F RID: 14959 RVA: 0x00011C20 File Offset: 0x00010020
		public void EndJobWith(JobCondition condition)
		{
			if (!this.pawn.Destroyed && this.pawn.CurJob == this.job)
			{
				this.pawn.jobs.EndCurrentJob(condition, true);
			}
		}

		// Token: 0x06003A70 RID: 14960 RVA: 0x00011C5C File Offset: 0x0001005C
		public virtual object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn
			};
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x00011C80 File Offset: 0x00010080
		private bool CheckCurrentToilEndOrFail()
		{
			bool result;
			try
			{
				Toil curToil = this.CurToil;
				if (this.globalFailConditions != null)
				{
					for (int i = 0; i < this.globalFailConditions.Count; i++)
					{
						JobCondition jobCondition = this.globalFailConditions[i]();
						if (jobCondition != JobCondition.Ongoing)
						{
							if (this.pawn.jobs.debugLog)
							{
								this.pawn.jobs.DebugLogEvent(string.Concat(new object[]
								{
									base.GetType().Name,
									" ends current job ",
									this.job.ToStringSafe<Job>(),
									" because of globalFailConditions[",
									i,
									"]"
								}));
							}
							this.EndJobWith(jobCondition);
							return true;
						}
					}
				}
				if (curToil != null && curToil.endConditions != null)
				{
					for (int j = 0; j < curToil.endConditions.Count; j++)
					{
						JobCondition jobCondition2 = curToil.endConditions[j]();
						if (jobCondition2 != JobCondition.Ongoing)
						{
							if (this.pawn.jobs.debugLog)
							{
								this.pawn.jobs.DebugLogEvent(string.Concat(new object[]
								{
									base.GetType().Name,
									" ends current job ",
									this.job.ToStringSafe<Job>(),
									" because of toils[",
									this.curToilIndex,
									"].endConditions[",
									j,
									"]"
								}));
							}
							this.EndJobWith(jobCondition2);
							return true;
						}
					}
				}
				result = false;
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, "Exception in CheckCurrentToilEndOrFail for pawn " + this.pawn.ToStringSafe<Pawn>(), exception, this);
				result = true;
			}
			return result;
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x00011E8C File Offset: 0x0001028C
		private void SetNextToil(Toil to)
		{
			if (to != null && !this.toils.Contains(to))
			{
				Log.Warning(string.Concat(new string[]
				{
					"SetNextToil with non-existent toil (",
					to.ToStringSafe<Toil>(),
					"). pawn=",
					this.pawn.ToStringSafe<Pawn>(),
					", job=",
					this.pawn.CurJob.ToStringSafe<Job>()
				}), false);
			}
			this.nextToilIndex = this.toils.IndexOf(to);
		}

		// Token: 0x06003A73 RID: 14963 RVA: 0x00011F18 File Offset: 0x00010318
		public void JumpToToil(Toil to)
		{
			if (to == null)
			{
				Log.Warning("JumpToToil with null toil. pawn=" + this.pawn.ToStringSafe<Pawn>() + ", job=" + this.pawn.CurJob.ToStringSafe<Job>(), false);
			}
			this.SetNextToil(to);
			this.ReadyForNextToil();
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x00011F6B File Offset: 0x0001036B
		public virtual void Notify_Starting()
		{
			this.startTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x00011F7E File Offset: 0x0001037E
		public virtual void Notify_PatherArrived()
		{
			if (this.curToilCompleteMode == ToilCompleteMode.PatherArrival)
			{
				this.ReadyForNextToil();
			}
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x00011F93 File Offset: 0x00010393
		public virtual void Notify_PatherFailed()
		{
			this.EndJobWith(JobCondition.ErroredPather);
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x00011F9D File Offset: 0x0001039D
		public virtual void Notify_StanceChanged()
		{
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x00011FA0 File Offset: 0x000103A0
		public virtual void Notify_DamageTaken(DamageInfo dinfo)
		{
		}

		// Token: 0x06003A79 RID: 14969 RVA: 0x00011FA4 File Offset: 0x000103A4
		public Pawn GetActor()
		{
			return this.pawn;
		}

		// Token: 0x06003A7A RID: 14970 RVA: 0x00011FBF File Offset: 0x000103BF
		public void AddEndCondition(Func<JobCondition> newEndCondition)
		{
			this.globalFailConditions.Add(newEndCondition);
		}

		// Token: 0x06003A7B RID: 14971 RVA: 0x00011FD0 File Offset: 0x000103D0
		public void AddFailCondition(Func<bool> newFailCondition)
		{
			this.globalFailConditions.Add(delegate
			{
				JobCondition result;
				if (newFailCondition())
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
		}

		// Token: 0x06003A7C RID: 14972 RVA: 0x00012002 File Offset: 0x00010402
		public void AddFinishAction(Action newAct)
		{
			this.globalFinishActions.Add(newAct);
		}

		// Token: 0x06003A7D RID: 14973 RVA: 0x00012014 File Offset: 0x00010414
		public virtual bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool behind, ref bool flip)
		{
			return false;
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x0001202C File Offset: 0x0001042C
		public virtual RandomSocialMode DesiredSocialMode()
		{
			RandomSocialMode result;
			if (this.CurToil != null)
			{
				result = this.CurToil.socialMode;
			}
			else
			{
				result = RandomSocialMode.Normal;
			}
			return result;
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x00012060 File Offset: 0x00010460
		public virtual bool IsContinuation(Job j)
		{
			return true;
		}

		// Token: 0x04002519 RID: 9497
		public Pawn pawn;

		// Token: 0x0400251A RID: 9498
		public Job job;

		// Token: 0x0400251B RID: 9499
		private List<Toil> toils = new List<Toil>();

		// Token: 0x0400251C RID: 9500
		public List<Func<JobCondition>> globalFailConditions = new List<Func<JobCondition>>();

		// Token: 0x0400251D RID: 9501
		public List<Action> globalFinishActions = new List<Action>();

		// Token: 0x0400251E RID: 9502
		public bool ended = false;

		// Token: 0x0400251F RID: 9503
		private int curToilIndex = -1;

		// Token: 0x04002520 RID: 9504
		private ToilCompleteMode curToilCompleteMode;

		// Token: 0x04002521 RID: 9505
		public int ticksLeftThisToil = 99999;

		// Token: 0x04002522 RID: 9506
		private bool wantBeginNextToil = false;

		// Token: 0x04002523 RID: 9507
		protected int startTick = -1;

		// Token: 0x04002524 RID: 9508
		public TargetIndex rotateToFace = TargetIndex.A;

		// Token: 0x04002525 RID: 9509
		private int nextToilIndex = -1;

		// Token: 0x04002526 RID: 9510
		public bool asleep;

		// Token: 0x04002527 RID: 9511
		public float uninstallWorkLeft;

		// Token: 0x04002528 RID: 9512
		public bool collideWithPawns;

		// Token: 0x04002529 RID: 9513
		public Pawn locomotionUrgencySameAs;

		// Token: 0x0400252A RID: 9514
		public int debugTicksSpentThisToil = 0;
	}
}
