using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobDriver_RemoveBuilding : JobDriver
	{
		private float workLeft;

		private float totalNeededWork;

		protected JobDriver_RemoveBuilding()
		{
		}

		protected Thing Target
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		protected Building Building
		{
			get
			{
				return (Building)this.Target.GetInnerIfMinified();
			}
		}

		protected abstract DesignationDef Designation { get; }

		protected abstract float TotalNeededWork { get; }

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.totalNeededWork, "totalNeededWork", 0f, false);
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			Pawn pawn = this.pawn;
			LocalTargetInfo target = this.Target;
			Job job = this.job;
			return pawn.Reserve(target, job, 1, -1, null, errorOnFailed);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnThingMissingDesignation(TargetIndex.A, this.Designation);
			this.FailOnForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil doWork = new Toil().FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			doWork.initAction = delegate()
			{
				this.totalNeededWork = this.TotalNeededWork;
				this.workLeft = this.totalNeededWork;
			};
			doWork.tickAction = delegate()
			{
				this.workLeft -= this.pawn.GetStatValue(StatDefOf.ConstructionSpeed, true);
				this.TickAction();
				if (this.workLeft <= 0f)
				{
					doWork.actor.jobs.curDriver.ReadyForNextToil();
				}
			};
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / this.totalNeededWork, false, -0.5f);
			doWork.activeSkill = (() => SkillDefOf.Construction);
			yield return doWork;
			yield return new Toil
			{
				initAction = delegate()
				{
					this.FinishedRemoving();
					base.Map.designationManager.RemoveAllDesignationsOn(this.Target, false);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		protected virtual void FinishedRemoving()
		{
		}

		protected virtual void TickAction()
		{
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <finalize>__2;

			internal JobDriver_RemoveBuilding $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_RemoveBuilding.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

			private static Func<SkillDef> <>f__am$cache0;

			[DebuggerHidden]
			public <MakeNewToils>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.FailOnThingMissingDesignation(TargetIndex.A, this.Designation);
					this.FailOnForbidden(TargetIndex.A);
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
				{
					Toil doWork = new Toil().FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
					doWork.initAction = delegate()
					{
						this.totalNeededWork = this.TotalNeededWork;
						this.workLeft = this.totalNeededWork;
					};
					doWork.tickAction = delegate()
					{
						this.workLeft -= this.pawn.GetStatValue(StatDefOf.ConstructionSpeed, true);
						this.TickAction();
						if (this.workLeft <= 0f)
						{
							doWork.actor.jobs.curDriver.ReadyForNextToil();
						}
					};
					doWork.defaultCompleteMode = ToilCompleteMode.Never;
					doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / this.totalNeededWork, false, -0.5f);
					doWork.activeSkill = (() => SkillDefOf.Construction);
					this.$current = doWork;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				case 2u:
				{
					Toil finalize = new Toil();
					finalize.initAction = delegate()
					{
						this.FinishedRemoving();
						base.Map.designationManager.RemoveAllDesignationsOn(base.Target, false);
					};
					finalize.defaultCompleteMode = ToilCompleteMode.Instant;
					this.$current = finalize;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				case 3u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			Toil IEnumerator<Toil>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.AI.Toil>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				JobDriver_RemoveBuilding.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_RemoveBuilding.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private static SkillDef <>m__0()
			{
				return SkillDefOf.Construction;
			}

			internal void <>m__1()
			{
				this.FinishedRemoving();
				base.Map.designationManager.RemoveAllDesignationsOn(base.Target, false);
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil doWork;

				internal JobDriver_RemoveBuilding.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$0.$this.totalNeededWork = this.<>f__ref$0.$this.TotalNeededWork;
					this.<>f__ref$0.$this.workLeft = this.<>f__ref$0.$this.totalNeededWork;
				}

				internal void <>m__1()
				{
					this.<>f__ref$0.$this.workLeft -= this.<>f__ref$0.$this.pawn.GetStatValue(StatDefOf.ConstructionSpeed, true);
					this.<>f__ref$0.$this.TickAction();
					if (this.<>f__ref$0.$this.workLeft <= 0f)
					{
						this.doWork.actor.jobs.curDriver.ReadyForNextToil();
					}
				}

				internal float <>m__2()
				{
					return 1f - this.<>f__ref$0.$this.workLeft / this.<>f__ref$0.$this.totalNeededWork;
				}
			}
		}
	}
}
