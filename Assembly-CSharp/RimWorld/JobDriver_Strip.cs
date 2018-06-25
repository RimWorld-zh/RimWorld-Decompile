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
	public class JobDriver_Strip : JobDriver
	{
		private const int StripTicks = 60;

		public JobDriver_Strip()
		{
		}

		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOn(() => !StrippableUtility.CanBeStrippedByColony(base.TargetThingA));
			Toil gotoThing = new Toil();
			gotoThing.initAction = delegate()
			{
				this.pawn.pather.StartPath(base.TargetThingA, PathEndMode.ClosestTouch);
			};
			gotoThing.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			gotoThing.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return gotoThing;
			yield return Toils_General.Wait(60).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return new Toil
			{
				initAction = delegate()
				{
					Thing thing = this.job.targetA.Thing;
					Designation designation = base.Map.designationManager.DesignationOn(thing, DesignationDefOf.Strip);
					if (designation != null)
					{
						designation.Delete();
					}
					IStrippable strippable = thing as IStrippable;
					if (strippable != null)
					{
						strippable.Strip();
					}
					this.pawn.records.Increment(RecordDefOf.BodiesStripped);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		public override object[] TaleParameters()
		{
			Corpse corpse = base.TargetA.Thing as Corpse;
			return new object[]
			{
				this.pawn,
				(corpse == null) ? base.TargetA.Thing : corpse.InnerPawn
			};
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <gotoThing>__1;

			internal Toil <strip>__2;

			internal JobDriver_Strip $this;

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
					this.FailOnAggroMentalState(TargetIndex.A);
					this.FailOn(() => !StrippableUtility.CanBeStrippedByColony(base.TargetThingA));
					gotoThing = new Toil();
					gotoThing.initAction = delegate()
					{
						this.pawn.pather.StartPath(base.TargetThingA, PathEndMode.ClosestTouch);
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
					this.$current = Toils_General.Wait(60).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
				{
					Toil strip = new Toil();
					strip.initAction = delegate()
					{
						Thing thing = this.job.targetA.Thing;
						Designation designation = base.Map.designationManager.DesignationOn(thing, DesignationDefOf.Strip);
						if (designation != null)
						{
							designation.Delete();
						}
						IStrippable strippable = thing as IStrippable;
						if (strippable != null)
						{
							strippable.Strip();
						}
						this.pawn.records.Increment(RecordDefOf.BodiesStripped);
					};
					strip.defaultCompleteMode = ToilCompleteMode.Instant;
					this.$current = strip;
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
				JobDriver_Strip.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Strip.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal bool <>m__0()
			{
				return !StrippableUtility.CanBeStrippedByColony(base.TargetThingA);
			}

			internal void <>m__1()
			{
				this.pawn.pather.StartPath(base.TargetThingA, PathEndMode.ClosestTouch);
			}

			internal void <>m__2()
			{
				Thing thing = this.job.targetA.Thing;
				Designation designation = base.Map.designationManager.DesignationOn(thing, DesignationDefOf.Strip);
				if (designation != null)
				{
					designation.Delete();
				}
				IStrippable strippable = thing as IStrippable;
				if (strippable != null)
				{
					strippable.Strip();
				}
				this.pawn.records.Increment(RecordDefOf.BodiesStripped);
			}
		}
	}
}
