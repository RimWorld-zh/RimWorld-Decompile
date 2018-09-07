using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class JobDriver_PrepareCaravan_GatherPawns : JobDriver
	{
		private const TargetIndex AnimalOrSlaveInd = TargetIndex.A;

		public JobDriver_PrepareCaravan_GatherPawns()
		{
		}

		private Pawn AnimalOrSlave
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			Pawn pawn = this.pawn;
			LocalTargetInfo target = this.AnimalOrSlave;
			Job job = this.job;
			return pawn.Reserve(target, job, 1, -1, null, errorOnFailed);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !base.Map.lordManager.lords.Contains(this.job.lord));
			this.FailOn(() => this.AnimalOrSlave == null || this.AnimalOrSlave.GetLord() != this.job.lord);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A).FailOn(() => GatherAnimalsAndSlavesForCaravanUtility.IsFollowingAnyone(this.AnimalOrSlave));
			yield return this.SetFollowerToil();
			yield break;
		}

		private Toil SetFollowerToil()
		{
			return new Toil
			{
				initAction = delegate()
				{
					GatherAnimalsAndSlavesForCaravanUtility.SetFollower(this.AnimalOrSlave, this.pawn);
					RestUtility.WakeUp(this.pawn);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}

		[CompilerGenerated]
		private void <SetFollowerToil>m__0()
		{
			GatherAnimalsAndSlavesForCaravanUtility.SetFollower(this.AnimalOrSlave, this.pawn);
			RestUtility.WakeUp(this.pawn);
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_PrepareCaravan_GatherPawns $this;

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
					this.FailOn(() => !base.Map.lordManager.lords.Contains(this.job.lord));
					this.FailOn(() => base.AnimalOrSlave == null || base.AnimalOrSlave.GetLord() != this.job.lord);
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A).FailOn(() => GatherAnimalsAndSlavesForCaravanUtility.IsFollowingAnyone(base.AnimalOrSlave));
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = base.SetFollowerToil();
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
				JobDriver_PrepareCaravan_GatherPawns.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_PrepareCaravan_GatherPawns.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal bool <>m__0()
			{
				return !base.Map.lordManager.lords.Contains(this.job.lord);
			}

			internal bool <>m__1()
			{
				return base.AnimalOrSlave == null || base.AnimalOrSlave.GetLord() != this.job.lord;
			}

			internal bool <>m__2()
			{
				return GatherAnimalsAndSlavesForCaravanUtility.IsFollowingAnyone(base.AnimalOrSlave);
			}
		}
	}
}
