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
	public class JobDriver_Flick : JobDriver
	{
		public JobDriver_Flick()
		{
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
			this.FailOn(delegate()
			{
				Designation designation = this.Map.designationManager.DesignationOn(this.TargetThingA, DesignationDefOf.Flick);
				return designation == null;
			});
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(15, TargetIndex.None).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			Toil finalize = new Toil();
			finalize.initAction = delegate()
			{
				Pawn actor = finalize.actor;
				ThingWithComps thingWithComps = (ThingWithComps)actor.CurJob.targetA.Thing;
				for (int i = 0; i < thingWithComps.AllComps.Count; i++)
				{
					CompFlickable compFlickable = thingWithComps.AllComps[i] as CompFlickable;
					if (compFlickable != null && compFlickable.WantsFlick())
					{
						compFlickable.DoFlick();
					}
				}
				actor.records.Increment(RecordDefOf.SwitchesFlicked);
				Designation designation = this.Map.designationManager.DesignationOn(thingWithComps, DesignationDefOf.Flick);
				if (designation != null)
				{
					designation.Delete();
				}
			};
			finalize.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return finalize;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_Flick $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_Flick.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

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
					this.FailOn(delegate()
					{
						Designation designation = this.Map.designationManager.DesignationOn(this.TargetThingA, DesignationDefOf.Flick);
						return designation == null;
					});
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = Toils_General.Wait(15, TargetIndex.None).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					<MakeNewToils>c__AnonStorey.finalize = new Toil();
					<MakeNewToils>c__AnonStorey.finalize.initAction = delegate()
					{
						Pawn actor = <MakeNewToils>c__AnonStorey.finalize.actor;
						ThingWithComps thingWithComps = (ThingWithComps)actor.CurJob.targetA.Thing;
						for (int i = 0; i < thingWithComps.AllComps.Count; i++)
						{
							CompFlickable compFlickable = thingWithComps.AllComps[i] as CompFlickable;
							if (compFlickable != null && compFlickable.WantsFlick())
							{
								compFlickable.DoFlick();
							}
						}
						actor.records.Increment(RecordDefOf.SwitchesFlicked);
						Designation designation = <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Map.designationManager.DesignationOn(thingWithComps, DesignationDefOf.Flick);
						if (designation != null)
						{
							designation.Delete();
						}
					};
					<MakeNewToils>c__AnonStorey.finalize.defaultCompleteMode = ToilCompleteMode.Instant;
					this.$current = <MakeNewToils>c__AnonStorey.finalize;
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
				JobDriver_Flick.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Flick.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil finalize;

				internal JobDriver_Flick.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal bool <>m__0()
				{
					Designation designation = this.<>f__ref$0.$this.Map.designationManager.DesignationOn(this.<>f__ref$0.$this.TargetThingA, DesignationDefOf.Flick);
					return designation == null;
				}

				internal void <>m__1()
				{
					Pawn actor = this.finalize.actor;
					ThingWithComps thingWithComps = (ThingWithComps)actor.CurJob.targetA.Thing;
					for (int i = 0; i < thingWithComps.AllComps.Count; i++)
					{
						CompFlickable compFlickable = thingWithComps.AllComps[i] as CompFlickable;
						if (compFlickable != null && compFlickable.WantsFlick())
						{
							compFlickable.DoFlick();
						}
					}
					actor.records.Increment(RecordDefOf.SwitchesFlicked);
					Designation designation = this.<>f__ref$0.$this.Map.designationManager.DesignationOn(thingWithComps, DesignationDefOf.Flick);
					if (designation != null)
					{
						designation.Delete();
					}
				}
			}
		}
	}
}
