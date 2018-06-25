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
	public class JobDriver_OperateDeepDrill : JobDriver
	{
		public JobDriver_OperateDeepDrill()
		{
		}

		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			this.FailOn(delegate()
			{
				CompDeepDrill compDeepDrill = this.job.targetA.Thing.TryGetComp<CompDeepDrill>();
				return !compDeepDrill.CanDrillNow();
			});
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil work = new Toil();
			work.tickAction = delegate()
			{
				Pawn actor = work.actor;
				Building building = (Building)actor.CurJob.targetA.Thing;
				CompDeepDrill comp = building.GetComp<CompDeepDrill>();
				comp.DrillWorkDone(actor);
				actor.skills.Learn(SkillDefOf.Mining, 0.0714999959f, false);
			};
			work.defaultCompleteMode = ToilCompleteMode.Never;
			work.WithEffect(EffecterDefOf.Drill, TargetIndex.A);
			work.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			work.activeSkill = (() => SkillDefOf.Mining);
			yield return work;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_OperateDeepDrill $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_OperateDeepDrill.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

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
					this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
					this.FailOnBurningImmobile(TargetIndex.A);
					this.FailOn(delegate()
					{
						CompDeepDrill compDeepDrill = this.job.targetA.Thing.TryGetComp<CompDeepDrill>();
						return !compDeepDrill.CanDrillNow();
					});
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					<MakeNewToils>c__AnonStorey.work = new Toil();
					<MakeNewToils>c__AnonStorey.work.tickAction = delegate()
					{
						Pawn actor = <MakeNewToils>c__AnonStorey.work.actor;
						Building building = (Building)actor.CurJob.targetA.Thing;
						CompDeepDrill comp = building.GetComp<CompDeepDrill>();
						comp.DrillWorkDone(actor);
						actor.skills.Learn(SkillDefOf.Mining, 0.0714999959f, false);
					};
					<MakeNewToils>c__AnonStorey.work.defaultCompleteMode = ToilCompleteMode.Never;
					<MakeNewToils>c__AnonStorey.work.WithEffect(EffecterDefOf.Drill, TargetIndex.A);
					<MakeNewToils>c__AnonStorey.work.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
					<MakeNewToils>c__AnonStorey.work.activeSkill = (() => SkillDefOf.Mining);
					this.$current = <MakeNewToils>c__AnonStorey.work;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
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
				JobDriver_OperateDeepDrill.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_OperateDeepDrill.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private static SkillDef <>m__0()
			{
				return SkillDefOf.Mining;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil work;

				internal JobDriver_OperateDeepDrill.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal bool <>m__0()
				{
					CompDeepDrill compDeepDrill = this.<>f__ref$0.$this.job.targetA.Thing.TryGetComp<CompDeepDrill>();
					return !compDeepDrill.CanDrillNow();
				}

				internal void <>m__1()
				{
					Pawn actor = this.work.actor;
					Building building = (Building)actor.CurJob.targetA.Thing;
					CompDeepDrill comp = building.GetComp<CompDeepDrill>();
					comp.DrillWorkDone(actor);
					actor.skills.Learn(SkillDefOf.Mining, 0.0714999959f, false);
				}
			}
		}
	}
}
