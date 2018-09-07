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
	public class JobDriver_LayEgg : JobDriver
	{
		private const int LayEgg = 500;

		private const TargetIndex LaySpotInd = TargetIndex.A;

		public JobDriver_LayEgg()
		{
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield return Toils_General.Wait(500, TargetIndex.None);
			yield return Toils_General.Do(delegate
			{
				Thing forbiddenIfOutsideHomeArea = GenSpawn.Spawn(this.pawn.GetComp<CompEggLayer>().ProduceEgg(), this.pawn.Position, base.Map, WipeMode.Vanish);
				forbiddenIfOutsideHomeArea.SetForbiddenIfOutsideHomeArea();
			});
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_LayEgg $this;

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
					this.$current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = Toils_General.Wait(500, TargetIndex.None);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = Toils_General.Do(delegate
					{
						Thing forbiddenIfOutsideHomeArea = GenSpawn.Spawn(this.pawn.GetComp<CompEggLayer>().ProduceEgg(), this.pawn.Position, base.Map, WipeMode.Vanish);
						forbiddenIfOutsideHomeArea.SetForbiddenIfOutsideHomeArea();
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
				JobDriver_LayEgg.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_LayEgg.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				Thing forbiddenIfOutsideHomeArea = GenSpawn.Spawn(this.pawn.GetComp<CompEggLayer>().ProduceEgg(), this.pawn.Position, base.Map, WipeMode.Vanish);
				forbiddenIfOutsideHomeArea.SetForbiddenIfOutsideHomeArea();
			}
		}
	}
}
