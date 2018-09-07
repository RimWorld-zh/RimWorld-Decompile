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
	public class JobDriver_BuildSnowman : JobDriver
	{
		private float workLeft = -1000f;

		protected const int BaseWorkAmount = 2300;

		public JobDriver_BuildSnowman()
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
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil doWork = new Toil();
			doWork.initAction = delegate()
			{
				this.workLeft = 2300f;
			};
			doWork.tickAction = delegate()
			{
				this.workLeft -= doWork.actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
				if (this.workLeft <= 0f)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.Snowman, null);
					thing.SetFaction(this.pawn.Faction, null);
					GenSpawn.Spawn(thing, this.TargetLocA, this.Map, WipeMode.Vanish);
					this.ReadyForNextToil();
					return;
				}
				JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, 1f, null);
			};
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.FailOn(() => !JoyUtility.EnjoyableOutsideNow(this.pawn, null));
			doWork.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return doWork;
			yield break;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_BuildSnowman $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_BuildSnowman.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

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
					this.$current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					<MakeNewToils>c__AnonStorey.doWork = new Toil();
					<MakeNewToils>c__AnonStorey.doWork.initAction = delegate()
					{
						<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.workLeft = 2300f;
					};
					<MakeNewToils>c__AnonStorey.doWork.tickAction = delegate()
					{
						<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.workLeft -= <MakeNewToils>c__AnonStorey.doWork.actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
						if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.workLeft <= 0f)
						{
							Thing thing = ThingMaker.MakeThing(ThingDefOf.Snowman, null);
							thing.SetFaction(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn.Faction, null);
							GenSpawn.Spawn(thing, <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetLocA, <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Map, WipeMode.Vanish);
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.ReadyForNextToil();
							return;
						}
						JoyUtility.JoyTickCheckEnd(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn, JoyTickFullJoyAction.EndJob, 1f, null);
					};
					<MakeNewToils>c__AnonStorey.doWork.defaultCompleteMode = ToilCompleteMode.Never;
					<MakeNewToils>c__AnonStorey.doWork.FailOn(() => !JoyUtility.EnjoyableOutsideNow(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn, null));
					<MakeNewToils>c__AnonStorey.doWork.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
					this.$current = <MakeNewToils>c__AnonStorey.doWork;
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
				JobDriver_BuildSnowman.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_BuildSnowman.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil doWork;

				internal JobDriver_BuildSnowman.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$0.$this.workLeft = 2300f;
				}

				internal void <>m__1()
				{
					this.<>f__ref$0.$this.workLeft -= this.doWork.actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
					if (this.<>f__ref$0.$this.workLeft <= 0f)
					{
						Thing thing = ThingMaker.MakeThing(ThingDefOf.Snowman, null);
						thing.SetFaction(this.<>f__ref$0.$this.pawn.Faction, null);
						GenSpawn.Spawn(thing, this.<>f__ref$0.$this.TargetLocA, this.<>f__ref$0.$this.Map, WipeMode.Vanish);
						this.<>f__ref$0.$this.ReadyForNextToil();
						return;
					}
					JoyUtility.JoyTickCheckEnd(this.<>f__ref$0.$this.pawn, JoyTickFullJoyAction.EndJob, 1f, null);
				}

				internal bool <>m__2()
				{
					return !JoyUtility.EnjoyableOutsideNow(this.<>f__ref$0.$this.pawn, null);
				}
			}
		}
	}
}
