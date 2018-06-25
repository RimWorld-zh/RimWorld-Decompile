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
	public class JobDriver_Nuzzle : JobDriver
	{
		private const int NuzzleDuration = 100;

		public JobDriver_Nuzzle()
		{
		}

		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			Toil gotoTarget = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			gotoTarget.socialMode = RandomSocialMode.Off;
			Toil wait = Toils_General.WaitWith(TargetIndex.A, 100, false, true);
			wait.socialMode = RandomSocialMode.Off;
			yield return Toils_General.Do(delegate
			{
				Pawn recipient = (Pawn)this.pawn.CurJob.targetA.Thing;
				this.pawn.interactions.TryInteractWith(recipient, InteractionDefOf.Nuzzle);
			});
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <gotoTarget>__0;

			internal Toil <wait>__0;

			internal JobDriver_Nuzzle $this;

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
					this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
					this.FailOnNotCasualInterruptible(TargetIndex.A);
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					gotoTarget = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					gotoTarget.socialMode = RandomSocialMode.Off;
					wait = Toils_General.WaitWith(TargetIndex.A, 100, false, true);
					wait.socialMode = RandomSocialMode.Off;
					this.$current = Toils_General.Do(delegate
					{
						Pawn recipient = (Pawn)this.pawn.CurJob.targetA.Thing;
						this.pawn.interactions.TryInteractWith(recipient, InteractionDefOf.Nuzzle);
					});
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
				JobDriver_Nuzzle.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Nuzzle.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				Pawn recipient = (Pawn)this.pawn.CurJob.targetA.Thing;
				this.pawn.interactions.TryInteractWith(recipient, InteractionDefOf.Nuzzle);
			}
		}
	}
}
