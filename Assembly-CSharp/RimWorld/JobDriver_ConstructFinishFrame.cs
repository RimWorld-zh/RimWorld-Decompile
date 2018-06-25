using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_ConstructFinishFrame : JobDriver
	{
		private const int JobEndInterval = 5000;

		public JobDriver_ConstructFinishFrame()
		{
		}

		private Frame Frame
		{
			get
			{
				return (Frame)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil build = new Toil();
			build.initAction = delegate()
			{
				GenClamor.DoClamor(build.actor, 15f, ClamorDefOf.Construction);
			};
			build.tickAction = delegate()
			{
				Pawn actor = build.actor;
				Frame frame = this.Frame;
				if (frame.resourceContainer.Count > 0)
				{
					actor.skills.Learn(SkillDefOf.Construction, 0.275f, false);
				}
				float num = actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
				if (frame.Stuff != null)
				{
					num *= frame.Stuff.GetStatValueAbstract(StatDefOf.ConstructionSpeedFactor, null);
				}
				float workToMake = frame.WorkToMake;
				if (actor.Faction == Faction.OfPlayer)
				{
					float statValue = actor.GetStatValue(StatDefOf.ConstructSuccessChance, true);
					if (Rand.Value < 1f - Mathf.Pow(statValue, num / workToMake))
					{
						frame.FailConstruction(actor);
						this.ReadyForNextToil();
						return;
					}
				}
				if (frame.def.entityDefToBuild is TerrainDef)
				{
					this.Map.snowGrid.SetDepth(frame.Position, 0f);
				}
				frame.workDone += num;
				if (frame.workDone >= workToMake)
				{
					frame.CompleteConstruction(actor);
					this.ReadyForNextToil();
				}
			};
			build.WithEffect(() => ((Frame)build.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing).ConstructionEffect, TargetIndex.A);
			build.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			build.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			build.FailOn(() => !GenConstruct.CanConstruct(this.Frame, this.pawn, true, false));
			build.defaultCompleteMode = ToilCompleteMode.Delay;
			build.defaultDuration = 5000;
			build.activeSkill = (() => SkillDefOf.Construction);
			yield return build;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_ConstructFinishFrame $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_ConstructFinishFrame.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

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
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					<MakeNewToils>c__AnonStorey.build = new Toil();
					<MakeNewToils>c__AnonStorey.build.initAction = delegate()
					{
						GenClamor.DoClamor(<MakeNewToils>c__AnonStorey.build.actor, 15f, ClamorDefOf.Construction);
					};
					<MakeNewToils>c__AnonStorey.build.tickAction = delegate()
					{
						Pawn actor = <MakeNewToils>c__AnonStorey.build.actor;
						Frame frame = <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Frame;
						if (frame.resourceContainer.Count > 0)
						{
							actor.skills.Learn(SkillDefOf.Construction, 0.275f, false);
						}
						float num2 = actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
						if (frame.Stuff != null)
						{
							num2 *= frame.Stuff.GetStatValueAbstract(StatDefOf.ConstructionSpeedFactor, null);
						}
						float workToMake = frame.WorkToMake;
						if (actor.Faction == Faction.OfPlayer)
						{
							float statValue = actor.GetStatValue(StatDefOf.ConstructSuccessChance, true);
							if (Rand.Value < 1f - Mathf.Pow(statValue, num2 / workToMake))
							{
								frame.FailConstruction(actor);
								<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.ReadyForNextToil();
								return;
							}
						}
						if (frame.def.entityDefToBuild is TerrainDef)
						{
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Map.snowGrid.SetDepth(frame.Position, 0f);
						}
						frame.workDone += num2;
						if (frame.workDone >= workToMake)
						{
							frame.CompleteConstruction(actor);
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.ReadyForNextToil();
						}
					};
					<MakeNewToils>c__AnonStorey.build.WithEffect(() => ((Frame)<MakeNewToils>c__AnonStorey.build.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing).ConstructionEffect, TargetIndex.A);
					<MakeNewToils>c__AnonStorey.build.FailOnDespawnedNullOrForbidden(TargetIndex.A);
					<MakeNewToils>c__AnonStorey.build.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
					<MakeNewToils>c__AnonStorey.build.FailOn(() => !GenConstruct.CanConstruct(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Frame, <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn, true, false));
					<MakeNewToils>c__AnonStorey.build.defaultCompleteMode = ToilCompleteMode.Delay;
					<MakeNewToils>c__AnonStorey.build.defaultDuration = 5000;
					<MakeNewToils>c__AnonStorey.build.activeSkill = (() => SkillDefOf.Construction);
					this.$current = <MakeNewToils>c__AnonStorey.build;
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
				JobDriver_ConstructFinishFrame.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_ConstructFinishFrame.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private static SkillDef <>m__0()
			{
				return SkillDefOf.Construction;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil build;

				internal JobDriver_ConstructFinishFrame.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					GenClamor.DoClamor(this.build.actor, 15f, ClamorDefOf.Construction);
				}

				internal void <>m__1()
				{
					Pawn actor = this.build.actor;
					Frame frame = this.<>f__ref$0.$this.Frame;
					if (frame.resourceContainer.Count > 0)
					{
						actor.skills.Learn(SkillDefOf.Construction, 0.275f, false);
					}
					float num = actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
					if (frame.Stuff != null)
					{
						num *= frame.Stuff.GetStatValueAbstract(StatDefOf.ConstructionSpeedFactor, null);
					}
					float workToMake = frame.WorkToMake;
					if (actor.Faction == Faction.OfPlayer)
					{
						float statValue = actor.GetStatValue(StatDefOf.ConstructSuccessChance, true);
						if (Rand.Value < 1f - Mathf.Pow(statValue, num / workToMake))
						{
							frame.FailConstruction(actor);
							this.<>f__ref$0.$this.ReadyForNextToil();
							return;
						}
					}
					if (frame.def.entityDefToBuild is TerrainDef)
					{
						this.<>f__ref$0.$this.Map.snowGrid.SetDepth(frame.Position, 0f);
					}
					frame.workDone += num;
					if (frame.workDone >= workToMake)
					{
						frame.CompleteConstruction(actor);
						this.<>f__ref$0.$this.ReadyForNextToil();
					}
				}

				internal EffecterDef <>m__2()
				{
					return ((Frame)this.build.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing).ConstructionEffect;
				}

				internal bool <>m__3()
				{
					return !GenConstruct.CanConstruct(this.<>f__ref$0.$this.Frame, this.<>f__ref$0.$this.pawn, true, false);
				}
			}
		}
	}
}
