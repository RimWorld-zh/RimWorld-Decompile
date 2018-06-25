using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;

namespace Verse.AI
{
	public class JobDriver_ReleasePrisoner : JobDriver
	{
		private const TargetIndex PrisonerInd = TargetIndex.A;

		private const TargetIndex ReleaseCellInd = TargetIndex.B;

		public JobDriver_ReleasePrisoner()
		{
		}

		private Pawn Prisoner
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Prisoner, this.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.B);
			this.FailOn(() => ((Pawn)((Thing)this.GetActor().CurJob.GetTarget(TargetIndex.A))).guest.interactionMode != PrisonerInteractionModeDefOf.Release);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnAggroMentalState(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOn(() => !this.Prisoner.IsPrisonerOfColony || !this.Prisoner.guest.PrisonerIsSecure).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, false);
			Toil setReleased = new Toil();
			setReleased.initAction = delegate()
			{
				Pawn actor = setReleased.actor;
				Job curJob = actor.jobs.curJob;
				Pawn p = curJob.targetA.Thing as Pawn;
				GenGuest.PrisonerRelease(p);
			};
			yield return setReleased;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <carryToCell>__0;

			internal JobDriver_ReleasePrisoner $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_ReleasePrisoner.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

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
					this.FailOnDestroyedOrNull(TargetIndex.A);
					this.FailOnBurningImmobile(TargetIndex.B);
					this.FailOn(() => ((Pawn)((Thing)this.GetActor().CurJob.GetTarget(TargetIndex.A))).guest.interactionMode != PrisonerInteractionModeDefOf.Release);
					this.FailOnDowned(TargetIndex.A);
					this.FailOnAggroMentalState(TargetIndex.A);
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOn(() => !this.Prisoner.IsPrisonerOfColony || !this.Prisoner.guest.PrisonerIsSecure).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
					this.$current = carryToCell;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, false);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					<MakeNewToils>c__AnonStorey.setReleased = new Toil();
					<MakeNewToils>c__AnonStorey.setReleased.initAction = delegate()
					{
						Pawn actor = <MakeNewToils>c__AnonStorey.setReleased.actor;
						Job curJob = actor.jobs.curJob;
						Pawn p = curJob.targetA.Thing as Pawn;
						GenGuest.PrisonerRelease(p);
					};
					this.$current = <MakeNewToils>c__AnonStorey.setReleased;
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
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
				JobDriver_ReleasePrisoner.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_ReleasePrisoner.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil setReleased;

				internal JobDriver_ReleasePrisoner.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal bool <>m__0()
				{
					return ((Pawn)((Thing)this.<>f__ref$0.$this.GetActor().CurJob.GetTarget(TargetIndex.A))).guest.interactionMode != PrisonerInteractionModeDefOf.Release;
				}

				internal bool <>m__1()
				{
					return !this.<>f__ref$0.$this.Prisoner.IsPrisonerOfColony || !this.<>f__ref$0.$this.Prisoner.guest.PrisonerIsSecure;
				}

				internal void <>m__2()
				{
					Pawn actor = this.setReleased.actor;
					Job curJob = actor.jobs.curJob;
					Pawn p = curJob.targetA.Thing as Pawn;
					GenGuest.PrisonerRelease(p);
				}
			}
		}
	}
}
