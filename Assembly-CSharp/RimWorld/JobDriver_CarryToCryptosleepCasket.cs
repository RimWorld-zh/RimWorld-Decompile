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
	public class JobDriver_CarryToCryptosleepCasket : JobDriver
	{
		private const TargetIndex TakeeInd = TargetIndex.A;

		private const TargetIndex DropPodInd = TargetIndex.B;

		public JobDriver_CarryToCryptosleepCasket()
		{
		}

		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected Building_CryptosleepCasket DropPod
		{
			get
			{
				return (Building_CryptosleepCasket)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null) && this.pawn.Reserve(this.DropPod, this.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedOrNull(TargetIndex.B);
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOn(() => !this.DropPod.Accepts(this.Takee));
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOn(() => this.DropPod.GetDirectlyHeldThings().Count > 0).FailOn(() => !this.Takee.Downed).FailOn(() => !this.pawn.CanReach(this.Takee, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn)).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell);
			Toil prepare = Toils_General.Wait(500, TargetIndex.None);
			prepare.FailOnCannotTouch(TargetIndex.B, PathEndMode.InteractionCell);
			prepare.WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			yield return prepare;
			yield return new Toil
			{
				initAction = delegate()
				{
					this.DropPod.TryAcceptThing(this.Takee, true);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				this.Takee
			};
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <prepare>__0;

			internal Toil <putInPod>__1;

			internal JobDriver_CarryToCryptosleepCasket $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

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
					this.FailOnDestroyedOrNull(TargetIndex.B);
					this.FailOnAggroMentalState(TargetIndex.A);
					this.FailOn(() => !base.DropPod.Accepts(base.Takee));
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOn(() => base.DropPod.GetDirectlyHeldThings().Count > 0).FailOn(() => !base.Takee.Downed).FailOn(() => !this.pawn.CanReach(base.Takee, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn)).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
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
					this.$current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					prepare = Toils_General.Wait(500, TargetIndex.None);
					prepare.FailOnCannotTouch(TargetIndex.B, PathEndMode.InteractionCell);
					prepare.WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
					this.$current = prepare;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
				{
					Toil putInPod = new Toil();
					putInPod.initAction = delegate()
					{
						base.DropPod.TryAcceptThing(base.Takee, true);
					};
					putInPod.defaultCompleteMode = ToilCompleteMode.Instant;
					this.$current = putInPod;
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
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
				JobDriver_CarryToCryptosleepCasket.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_CarryToCryptosleepCasket.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal bool <>m__0()
			{
				return !base.DropPod.Accepts(base.Takee);
			}

			internal bool <>m__1()
			{
				return base.DropPod.GetDirectlyHeldThings().Count > 0;
			}

			internal bool <>m__2()
			{
				return !base.Takee.Downed;
			}

			internal bool <>m__3()
			{
				return !this.pawn.CanReach(base.Takee, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn);
			}

			internal void <>m__4()
			{
				base.DropPod.TryAcceptThing(base.Takee, true);
			}
		}
	}
}
