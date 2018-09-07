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
	public class JobDriver_Research : JobDriver
	{
		private const int JobEndInterval = 4000;

		public JobDriver_Research()
		{
		}

		private ResearchProjectDef Project
		{
			get
			{
				return Find.ResearchManager.currentProj;
			}
		}

		private Building_ResearchBench ResearchBench
		{
			get
			{
				return (Building_ResearchBench)base.TargetThingA;
			}
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			Pawn pawn = this.pawn;
			LocalTargetInfo target = this.ResearchBench;
			Job job = this.job;
			return pawn.Reserve(target, job, 1, -1, null, errorOnFailed);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil research = new Toil();
			research.tickAction = delegate()
			{
				Pawn actor = research.actor;
				float num = actor.GetStatValue(StatDefOf.ResearchSpeed, true);
				num *= this.TargetThingA.GetStatValue(StatDefOf.ResearchSpeedFactor, true);
				Find.ResearchManager.ResearchPerformed(num, actor);
				actor.skills.Learn(SkillDefOf.Intellectual, 0.1f, false);
				actor.GainComfortFromCellIfPossible();
			};
			research.FailOn(() => this.Project == null);
			research.FailOn(() => !this.Project.CanBeResearchedAt(this.ResearchBench, false));
			research.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			research.WithEffect(EffecterDefOf.Research, TargetIndex.A);
			research.WithProgressBar(TargetIndex.A, delegate
			{
				ResearchProjectDef project = this.Project;
				if (project == null)
				{
					return 0f;
				}
				return project.ProgressPercent;
			}, false, -0.5f);
			research.defaultCompleteMode = ToilCompleteMode.Delay;
			research.defaultDuration = 4000;
			research.activeSkill = (() => SkillDefOf.Intellectual);
			yield return research;
			yield return Toils_General.Wait(2, TargetIndex.None);
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_Research $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_Research.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

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
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					<MakeNewToils>c__AnonStorey.research = new Toil();
					<MakeNewToils>c__AnonStorey.research.tickAction = delegate()
					{
						Pawn actor = <MakeNewToils>c__AnonStorey.research.actor;
						float num2 = actor.GetStatValue(StatDefOf.ResearchSpeed, true);
						num2 *= <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetThingA.GetStatValue(StatDefOf.ResearchSpeedFactor, true);
						Find.ResearchManager.ResearchPerformed(num2, actor);
						actor.skills.Learn(SkillDefOf.Intellectual, 0.1f, false);
						actor.GainComfortFromCellIfPossible();
					};
					<MakeNewToils>c__AnonStorey.research.FailOn(() => <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Project == null);
					<MakeNewToils>c__AnonStorey.research.FailOn(() => !<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Project.CanBeResearchedAt(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.ResearchBench, false));
					<MakeNewToils>c__AnonStorey.research.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
					<MakeNewToils>c__AnonStorey.research.WithEffect(EffecterDefOf.Research, TargetIndex.A);
					<MakeNewToils>c__AnonStorey.research.WithProgressBar(TargetIndex.A, delegate
					{
						ResearchProjectDef project = <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Project;
						if (project == null)
						{
							return 0f;
						}
						return project.ProgressPercent;
					}, false, -0.5f);
					<MakeNewToils>c__AnonStorey.research.defaultCompleteMode = ToilCompleteMode.Delay;
					<MakeNewToils>c__AnonStorey.research.defaultDuration = 4000;
					<MakeNewToils>c__AnonStorey.research.activeSkill = (() => SkillDefOf.Intellectual);
					this.$current = <MakeNewToils>c__AnonStorey.research;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = Toils_General.Wait(2, TargetIndex.None);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
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
				JobDriver_Research.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Research.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private static SkillDef <>m__0()
			{
				return SkillDefOf.Intellectual;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil research;

				internal JobDriver_Research.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					Pawn actor = this.research.actor;
					float num = actor.GetStatValue(StatDefOf.ResearchSpeed, true);
					num *= this.<>f__ref$0.$this.TargetThingA.GetStatValue(StatDefOf.ResearchSpeedFactor, true);
					Find.ResearchManager.ResearchPerformed(num, actor);
					actor.skills.Learn(SkillDefOf.Intellectual, 0.1f, false);
					actor.GainComfortFromCellIfPossible();
				}

				internal bool <>m__1()
				{
					return this.<>f__ref$0.$this.Project == null;
				}

				internal bool <>m__2()
				{
					return !this.<>f__ref$0.$this.Project.CanBeResearchedAt(this.<>f__ref$0.$this.ResearchBench, false);
				}

				internal float <>m__3()
				{
					ResearchProjectDef project = this.<>f__ref$0.$this.Project;
					if (project == null)
					{
						return 0f;
					}
					return project.ProgressPercent;
				}
			}
		}
	}
}
