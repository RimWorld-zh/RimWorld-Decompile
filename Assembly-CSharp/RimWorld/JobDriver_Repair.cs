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
	public class JobDriver_Repair : JobDriver
	{
		protected float ticksToNextRepair;

		private const float WarmupTicks = 80f;

		private const float TicksBetweenRepairs = 20f;

		public JobDriver_Repair()
		{
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			Pawn pawn = this.pawn;
			LocalTargetInfo targetA = this.job.targetA;
			Job job = this.job;
			return pawn.Reserve(targetA, job, 1, -1, null, errorOnFailed);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil repair = new Toil();
			repair.initAction = delegate()
			{
				this.ticksToNextRepair = 80f;
			};
			repair.tickAction = delegate()
			{
				Pawn actor = repair.actor;
				actor.skills.Learn(SkillDefOf.Construction, 0.05f, false);
				float statValue = actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
				this.ticksToNextRepair -= statValue;
				if (this.ticksToNextRepair <= 0f)
				{
					this.ticksToNextRepair += 20f;
					this.TargetThingA.HitPoints++;
					this.TargetThingA.HitPoints = Mathf.Min(this.TargetThingA.HitPoints, this.TargetThingA.MaxHitPoints);
					this.Map.listerBuildingsRepairable.Notify_BuildingRepaired((Building)this.TargetThingA);
					if (this.TargetThingA.HitPoints == this.TargetThingA.MaxHitPoints)
					{
						actor.records.Increment(RecordDefOf.ThingsRepaired);
						actor.jobs.EndCurrentJob(JobCondition.Succeeded, true);
					}
				}
			};
			repair.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			repair.WithEffect(base.TargetThingA.def.repairEffect, TargetIndex.A);
			repair.defaultCompleteMode = ToilCompleteMode.Never;
			repair.activeSkill = (() => SkillDefOf.Construction);
			yield return repair;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_Repair $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_Repair.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

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
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					<MakeNewToils>c__AnonStorey.repair = new Toil();
					<MakeNewToils>c__AnonStorey.repair.initAction = delegate()
					{
						<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.ticksToNextRepair = 80f;
					};
					<MakeNewToils>c__AnonStorey.repair.tickAction = delegate()
					{
						Pawn actor = <MakeNewToils>c__AnonStorey.repair.actor;
						actor.skills.Learn(SkillDefOf.Construction, 0.05f, false);
						float statValue = actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
						<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.ticksToNextRepair -= statValue;
						if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.ticksToNextRepair <= 0f)
						{
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.ticksToNextRepair += 20f;
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetThingA.HitPoints++;
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetThingA.HitPoints = Mathf.Min(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetThingA.HitPoints, <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetThingA.MaxHitPoints);
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Map.listerBuildingsRepairable.Notify_BuildingRepaired((Building)<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetThingA);
							if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetThingA.HitPoints == <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetThingA.MaxHitPoints)
							{
								actor.records.Increment(RecordDefOf.ThingsRepaired);
								actor.jobs.EndCurrentJob(JobCondition.Succeeded, true);
							}
						}
					};
					<MakeNewToils>c__AnonStorey.repair.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
					<MakeNewToils>c__AnonStorey.repair.WithEffect(base.TargetThingA.def.repairEffect, TargetIndex.A);
					<MakeNewToils>c__AnonStorey.repair.defaultCompleteMode = ToilCompleteMode.Never;
					<MakeNewToils>c__AnonStorey.repair.activeSkill = (() => SkillDefOf.Construction);
					this.$current = <MakeNewToils>c__AnonStorey.repair;
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
				JobDriver_Repair.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Repair.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private static SkillDef <>m__0()
			{
				return SkillDefOf.Construction;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil repair;

				internal JobDriver_Repair.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$0.$this.ticksToNextRepair = 80f;
				}

				internal void <>m__1()
				{
					Pawn actor = this.repair.actor;
					actor.skills.Learn(SkillDefOf.Construction, 0.05f, false);
					float statValue = actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
					this.<>f__ref$0.$this.ticksToNextRepair -= statValue;
					if (this.<>f__ref$0.$this.ticksToNextRepair <= 0f)
					{
						this.<>f__ref$0.$this.ticksToNextRepair += 20f;
						this.<>f__ref$0.$this.TargetThingA.HitPoints++;
						this.<>f__ref$0.$this.TargetThingA.HitPoints = Mathf.Min(this.<>f__ref$0.$this.TargetThingA.HitPoints, this.<>f__ref$0.$this.TargetThingA.MaxHitPoints);
						this.<>f__ref$0.$this.Map.listerBuildingsRepairable.Notify_BuildingRepaired((Building)this.<>f__ref$0.$this.TargetThingA);
						if (this.<>f__ref$0.$this.TargetThingA.HitPoints == this.<>f__ref$0.$this.TargetThingA.MaxHitPoints)
						{
							actor.records.Increment(RecordDefOf.ThingsRepaired);
							actor.jobs.EndCurrentJob(JobCondition.Succeeded, true);
						}
					}
				}
			}
		}
	}
}
