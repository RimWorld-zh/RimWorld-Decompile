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
	public class JobDriver_RearmTrap : JobDriver
	{
		private const int RearmTicks = 800;

		public JobDriver_RearmTrap()
		{
		}

		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnThingMissingDesignation(TargetIndex.A, DesignationDefOf.RearmTrap);
			Toil gotoThing = new Toil();
			gotoThing.initAction = delegate()
			{
				this.pawn.pather.StartPath(base.TargetThingA, PathEndMode.Touch);
			};
			gotoThing.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			gotoThing.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return gotoThing;
			yield return Toils_General.Wait(800, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return new Toil
			{
				initAction = delegate()
				{
					Thing thing = this.job.targetA.Thing;
					Designation designation = base.Map.designationManager.DesignationOn(thing, DesignationDefOf.RearmTrap);
					if (designation != null)
					{
						designation.Delete();
					}
					Building_TrapRearmable building_TrapRearmable = thing as Building_TrapRearmable;
					building_TrapRearmable.Rearm();
					this.pawn.records.Increment(RecordDefOf.TrapsRearmed);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <gotoThing>__1;

			internal Toil <finalize>__2;

			internal JobDriver_RearmTrap $this;

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
					this.FailOnThingMissingDesignation(TargetIndex.A, DesignationDefOf.RearmTrap);
					gotoThing = new Toil();
					gotoThing.initAction = delegate()
					{
						this.pawn.pather.StartPath(base.TargetThingA, PathEndMode.Touch);
					};
					gotoThing.defaultCompleteMode = ToilCompleteMode.PatherArrival;
					gotoThing.FailOnDespawnedNullOrForbidden(TargetIndex.A);
					this.$current = gotoThing;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = Toils_General.Wait(800, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
				{
					Toil finalize = new Toil();
					finalize.initAction = delegate()
					{
						Thing thing = this.job.targetA.Thing;
						Designation designation = base.Map.designationManager.DesignationOn(thing, DesignationDefOf.RearmTrap);
						if (designation != null)
						{
							designation.Delete();
						}
						Building_TrapRearmable building_TrapRearmable = thing as Building_TrapRearmable;
						building_TrapRearmable.Rearm();
						this.pawn.records.Increment(RecordDefOf.TrapsRearmed);
					};
					finalize.defaultCompleteMode = ToilCompleteMode.Instant;
					this.$current = finalize;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
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
				JobDriver_RearmTrap.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_RearmTrap.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				this.pawn.pather.StartPath(base.TargetThingA, PathEndMode.Touch);
			}

			internal void <>m__1()
			{
				Thing thing = this.job.targetA.Thing;
				Designation designation = base.Map.designationManager.DesignationOn(thing, DesignationDefOf.RearmTrap);
				if (designation != null)
				{
					designation.Delete();
				}
				Building_TrapRearmable building_TrapRearmable = thing as Building_TrapRearmable;
				building_TrapRearmable.Rearm();
				this.pawn.records.Increment(RecordDefOf.TrapsRearmed);
			}
		}
	}
}
