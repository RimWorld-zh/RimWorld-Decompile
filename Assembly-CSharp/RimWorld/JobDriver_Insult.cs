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
	public class JobDriver_Insult : JobDriver
	{
		private const TargetIndex TargetInd = TargetIndex.A;

		public JobDriver_Insult()
		{
		}

		private Pawn Target
		{
			get
			{
				return (Pawn)((Thing)this.pawn.CurJob.GetTarget(TargetIndex.A));
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return this.InsultingSpreeDelayToil();
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			Toil finalGoto = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			finalGoto.socialMode = RandomSocialMode.Off;
			yield return finalGoto;
			yield return this.InteractToil();
			yield break;
		}

		private Toil InteractToil()
		{
			return Toils_General.Do(delegate
			{
				if (this.pawn.interactions.TryInteractWith(this.Target, InteractionDefOf.Insult))
				{
					MentalState_InsultingSpree mentalState_InsultingSpree = this.pawn.MentalState as MentalState_InsultingSpree;
					if (mentalState_InsultingSpree != null)
					{
						mentalState_InsultingSpree.lastInsultTicks = Find.TickManager.TicksGame;
						if (mentalState_InsultingSpree.target == this.Target)
						{
							mentalState_InsultingSpree.insultedTargetAtLeastOnce = true;
						}
					}
				}
			});
		}

		private Toil InsultingSpreeDelayToil()
		{
			Action action = delegate()
			{
				MentalState_InsultingSpree mentalState_InsultingSpree = this.pawn.MentalState as MentalState_InsultingSpree;
				if (mentalState_InsultingSpree == null || Find.TickManager.TicksGame - mentalState_InsultingSpree.lastInsultTicks >= 1200)
				{
					this.pawn.jobs.curDriver.ReadyForNextToil();
				}
			};
			return new Toil
			{
				initAction = action,
				tickAction = action,
				socialMode = RandomSocialMode.Off,
				defaultCompleteMode = ToilCompleteMode.Never
			};
		}

		[CompilerGenerated]
		private void <InteractToil>m__0()
		{
			if (this.pawn.interactions.TryInteractWith(this.Target, InteractionDefOf.Insult))
			{
				MentalState_InsultingSpree mentalState_InsultingSpree = this.pawn.MentalState as MentalState_InsultingSpree;
				if (mentalState_InsultingSpree != null)
				{
					mentalState_InsultingSpree.lastInsultTicks = Find.TickManager.TicksGame;
					if (mentalState_InsultingSpree.target == this.Target)
					{
						mentalState_InsultingSpree.insultedTargetAtLeastOnce = true;
					}
				}
			}
		}

		[CompilerGenerated]
		private void <InsultingSpreeDelayToil>m__1()
		{
			MentalState_InsultingSpree mentalState_InsultingSpree = this.pawn.MentalState as MentalState_InsultingSpree;
			if (mentalState_InsultingSpree == null || Find.TickManager.TicksGame - mentalState_InsultingSpree.lastInsultTicks >= 1200)
			{
				this.pawn.jobs.curDriver.ReadyForNextToil();
			}
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <finalGoto>__0;

			internal JobDriver_Insult $this;

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
					this.FailOnDespawnedOrNull(TargetIndex.A);
					this.$current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = base.InsultingSpreeDelayToil();
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					finalGoto = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
					finalGoto.socialMode = RandomSocialMode.Off;
					this.$current = finalGoto;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = base.InteractToil();
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
				JobDriver_Insult.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Insult.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}
		}
	}
}
