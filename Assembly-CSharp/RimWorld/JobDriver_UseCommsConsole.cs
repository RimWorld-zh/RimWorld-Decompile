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
	public class JobDriver_UseCommsConsole : JobDriver
	{
		public JobDriver_UseCommsConsole()
		{
		}

		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.InteractionCell).FailOn(delegate(Toil to)
			{
				Building_CommsConsole building_CommsConsole = (Building_CommsConsole)to.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing;
				return !building_CommsConsole.CanUseCommsNow;
			});
			Toil openComms = new Toil();
			openComms.initAction = delegate()
			{
				Pawn actor = openComms.actor;
				Building_CommsConsole building_CommsConsole = (Building_CommsConsole)actor.jobs.curJob.GetTarget(TargetIndex.A).Thing;
				if (building_CommsConsole.CanUseCommsNow)
				{
					actor.jobs.curJob.commTarget.TryOpenComms(actor);
				}
			};
			yield return openComms;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_UseCommsConsole $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_UseCommsConsole.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

			private static Func<Toil, bool> <>f__am$cache0;

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
					this.$current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.InteractionCell).FailOn(delegate(Toil to)
					{
						Building_CommsConsole building_CommsConsole = (Building_CommsConsole)to.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing;
						return !building_CommsConsole.CanUseCommsNow;
					});
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					<MakeNewToils>c__AnonStorey.openComms = new Toil();
					<MakeNewToils>c__AnonStorey.openComms.initAction = delegate()
					{
						Pawn actor = <MakeNewToils>c__AnonStorey.openComms.actor;
						Building_CommsConsole building_CommsConsole = (Building_CommsConsole)actor.jobs.curJob.GetTarget(TargetIndex.A).Thing;
						if (building_CommsConsole.CanUseCommsNow)
						{
							actor.jobs.curJob.commTarget.TryOpenComms(actor);
						}
					};
					this.$current = <MakeNewToils>c__AnonStorey.openComms;
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
				JobDriver_UseCommsConsole.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_UseCommsConsole.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private static bool <>m__0(Toil to)
			{
				Building_CommsConsole building_CommsConsole = (Building_CommsConsole)to.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing;
				return !building_CommsConsole.CanUseCommsNow;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil openComms;

				internal JobDriver_UseCommsConsole.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					Pawn actor = this.openComms.actor;
					Building_CommsConsole building_CommsConsole = (Building_CommsConsole)actor.jobs.curJob.GetTarget(TargetIndex.A).Thing;
					if (building_CommsConsole.CanUseCommsNow)
					{
						actor.jobs.curJob.commTarget.TryOpenComms(actor);
					}
				}
			}
		}
	}
}
