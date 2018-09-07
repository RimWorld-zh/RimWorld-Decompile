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
	public class JobDriver_EnterTransporter : JobDriver
	{
		private TargetIndex TransporterInd = TargetIndex.A;

		public JobDriver_EnterTransporter()
		{
		}

		public CompTransporter Transporter
		{
			get
			{
				Thing thing = this.job.GetTarget(this.TransporterInd).Thing;
				if (thing == null)
				{
					return null;
				}
				return thing.TryGetComp<CompTransporter>();
			}
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(this.TransporterInd);
			this.FailOn(() => !this.Transporter.LoadingInProgressOrReadyToLaunch);
			yield return Toils_Goto.GotoThing(this.TransporterInd, PathEndMode.Touch);
			yield return new Toil
			{
				initAction = delegate()
				{
					CompTransporter transporter = this.Transporter;
					this.pawn.DeSpawn(DestroyMode.Vanish);
					transporter.GetDirectlyHeldThings().TryAdd(this.pawn, true);
				}
			};
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <enter>__0;

			internal JobDriver_EnterTransporter $this;

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
					this.FailOnDespawnedOrNull(this.TransporterInd);
					this.FailOn(() => !base.Transporter.LoadingInProgressOrReadyToLaunch);
					this.$current = Toils_Goto.GotoThing(this.TransporterInd, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
				{
					Toil enter = new Toil();
					enter.initAction = delegate()
					{
						CompTransporter transporter = base.Transporter;
						this.pawn.DeSpawn(DestroyMode.Vanish);
						transporter.GetDirectlyHeldThings().TryAdd(this.pawn, true);
					};
					this.$current = enter;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
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
				JobDriver_EnterTransporter.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_EnterTransporter.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal bool <>m__0()
			{
				return !base.Transporter.LoadingInProgressOrReadyToLaunch;
			}

			internal void <>m__1()
			{
				CompTransporter transporter = base.Transporter;
				this.pawn.DeSpawn(DestroyMode.Vanish);
				transporter.GetDirectlyHeldThings().TryAdd(this.pawn, true);
			}
		}
	}
}
