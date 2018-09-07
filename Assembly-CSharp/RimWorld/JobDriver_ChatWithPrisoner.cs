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
	public class JobDriver_ChatWithPrisoner : JobDriver
	{
		public JobDriver_ChatWithPrisoner()
		{
		}

		protected Pawn Talkee
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
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
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnMentalState(TargetIndex.A);
			this.FailOnNotAwake(TargetIndex.A);
			this.FailOn(() => !this.Talkee.IsPrisonerOfColony || !this.Talkee.guest.PrisonerIsSecure);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Talkee);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Talkee);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Talkee);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Talkee);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Talkee);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
			yield return Toils_Interpersonal.TryRecruit(TargetIndex.A);
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_ChatWithPrisoner $this;

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
					this.FailOnMentalState(TargetIndex.A);
					this.FailOnNotAwake(TargetIndex.A);
					this.FailOn(() => !base.Talkee.IsPrisonerOfColony || !base.Talkee.guest.PrisonerIsSecure);
					this.$current = Toils_Interpersonal.GotoPrisoner(this.pawn, base.Talkee, base.Talkee.guest.interactionMode);
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
					this.$current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = Toils_Interpersonal.ConvinceRecruitee(this.pawn, base.Talkee);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = Toils_Interpersonal.GotoPrisoner(this.pawn, base.Talkee, base.Talkee.guest.interactionMode);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$current = Toils_Interpersonal.ConvinceRecruitee(this.pawn, base.Talkee);
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
					this.$current = Toils_Interpersonal.GotoPrisoner(this.pawn, base.Talkee, base.Talkee.guest.interactionMode);
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				case 8u:
					this.$current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 9;
					}
					return true;
				case 9u:
					this.$current = Toils_Interpersonal.ConvinceRecruitee(this.pawn, base.Talkee);
					if (!this.$disposing)
					{
						this.$PC = 10;
					}
					return true;
				case 10u:
					this.$current = Toils_Interpersonal.GotoPrisoner(this.pawn, base.Talkee, base.Talkee.guest.interactionMode);
					if (!this.$disposing)
					{
						this.$PC = 11;
					}
					return true;
				case 11u:
					this.$current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 12;
					}
					return true;
				case 12u:
					this.$current = Toils_Interpersonal.ConvinceRecruitee(this.pawn, base.Talkee);
					if (!this.$disposing)
					{
						this.$PC = 13;
					}
					return true;
				case 13u:
					this.$current = Toils_Interpersonal.GotoPrisoner(this.pawn, base.Talkee, base.Talkee.guest.interactionMode);
					if (!this.$disposing)
					{
						this.$PC = 14;
					}
					return true;
				case 14u:
					this.$current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 15;
					}
					return true;
				case 15u:
					this.$current = Toils_Interpersonal.ConvinceRecruitee(this.pawn, base.Talkee);
					if (!this.$disposing)
					{
						this.$PC = 16;
					}
					return true;
				case 16u:
					this.$current = Toils_Interpersonal.GotoPrisoner(this.pawn, base.Talkee, base.Talkee.guest.interactionMode);
					if (!this.$disposing)
					{
						this.$PC = 17;
					}
					return true;
				case 17u:
					this.$current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 18;
					}
					return true;
				case 18u:
					this.$current = Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 19;
					}
					return true;
				case 19u:
					this.$current = Toils_Interpersonal.TryRecruit(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 20;
					}
					return true;
				case 20u:
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
				JobDriver_ChatWithPrisoner.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_ChatWithPrisoner.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal bool <>m__0()
			{
				return !base.Talkee.IsPrisonerOfColony || !base.Talkee.guest.PrisonerIsSecure;
			}
		}
	}
}
