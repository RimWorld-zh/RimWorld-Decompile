using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;

namespace Verse.AI
{
	public class JobDriver_HaulToCell : JobDriver
	{
		private bool forbiddenInitially;

		private const TargetIndex HaulableInd = TargetIndex.A;

		private const TargetIndex StoreCellInd = TargetIndex.B;

		public JobDriver_HaulToCell()
		{
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.forbiddenInitially, "forbiddenInitially", false, false);
		}

		public override string GetReport()
		{
			IntVec3 cell = this.job.targetB.Cell;
			Thing thing = null;
			if (this.pawn.CurJob == this.job && this.pawn.carryTracker.CarriedThing != null)
			{
				thing = this.pawn.carryTracker.CarriedThing;
			}
			else if (base.TargetThingA != null && base.TargetThingA.Spawned)
			{
				thing = base.TargetThingA;
			}
			string result;
			if (thing == null)
			{
				result = "ReportHaulingUnknown".Translate();
			}
			else
			{
				string text = null;
				SlotGroup slotGroup = cell.GetSlotGroup(base.Map);
				if (slotGroup != null)
				{
					text = slotGroup.parent.SlotYielderLabel();
				}
				if (text != null)
				{
					result = "ReportHaulingTo".Translate(new object[]
					{
						thing.Label,
						text
					});
				}
				else
				{
					result = "ReportHauling".Translate(new object[]
					{
						thing.Label
					});
				}
			}
			return result;
		}

		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null);
		}

		public override void Notify_Starting()
		{
			base.Notify_Starting();
			if (base.TargetThingA != null)
			{
				this.forbiddenInitially = base.TargetThingA.IsForbidden(this.pawn);
			}
			else
			{
				this.forbiddenInitially = false;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.B);
			if (!this.forbiddenInitially)
			{
				this.FailOnForbidden(TargetIndex.A);
			}
			Toil reserveTargetA = Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return reserveTargetA;
			Toil toilGoto = null;
			toilGoto = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A).FailOn(delegate()
			{
				Pawn actor = toilGoto.actor;
				Job curJob = actor.jobs.curJob;
				if (curJob.haulMode == HaulMode.ToCellStorage)
				{
					Thing thing = curJob.GetTarget(TargetIndex.A).Thing;
					IntVec3 cell = actor.jobs.curJob.GetTarget(TargetIndex.B).Cell;
					if (!cell.IsValidStorageFor(this.Map, thing))
					{
						return true;
					}
				}
				return false;
			});
			yield return toilGoto;
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, true, false);
			if (this.job.haulOpportunisticDuplicates)
			{
				yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveTargetA, TargetIndex.A, TargetIndex.B, false, null);
			}
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true);
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <reserveTargetA>__0;

			internal Toil <carryToCell>__0;

			internal JobDriver_HaulToCell $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_HaulToCell.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

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
					this.FailOnDestroyedOrNull(TargetIndex.A);
					this.FailOnBurningImmobile(TargetIndex.B);
					if (!this.forbiddenInitially)
					{
						this.FailOnForbidden(TargetIndex.A);
					}
					reserveTargetA = Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
					this.$current = reserveTargetA;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					<MakeNewToils>c__AnonStorey.toilGoto = null;
					<MakeNewToils>c__AnonStorey.toilGoto = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A).FailOn(delegate()
					{
						Pawn actor = <MakeNewToils>c__AnonStorey.toilGoto.actor;
						Job curJob = actor.jobs.curJob;
						if (curJob.haulMode == HaulMode.ToCellStorage)
						{
							Thing thing = curJob.GetTarget(TargetIndex.A).Thing;
							IntVec3 cell = actor.jobs.curJob.GetTarget(TargetIndex.B).Cell;
							if (!cell.IsValidStorageFor(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Map, thing))
							{
								return true;
							}
						}
						return false;
					});
					this.$current = <MakeNewToils>c__AnonStorey.toilGoto;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = Toils_Haul.StartCarryThing(TargetIndex.A, false, true, false);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					if (this.job.haulOpportunisticDuplicates)
					{
						this.$current = Toils_Haul.CheckForGetOpportunityDuplicate(reserveTargetA, TargetIndex.A, TargetIndex.B, false, null);
						if (!this.$disposing)
						{
							this.$PC = 4;
						}
						return true;
					}
					break;
				case 4u:
					break;
				case 5u:
					this.$current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true);
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
				this.$current = carryToCell;
				if (!this.$disposing)
				{
					this.$PC = 5;
				}
				return true;
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
				JobDriver_HaulToCell.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_HaulToCell.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil toilGoto;

				internal JobDriver_HaulToCell.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal bool <>m__0()
				{
					Pawn actor = this.toilGoto.actor;
					Job curJob = actor.jobs.curJob;
					if (curJob.haulMode == HaulMode.ToCellStorage)
					{
						Thing thing = curJob.GetTarget(TargetIndex.A).Thing;
						IntVec3 cell = actor.jobs.curJob.GetTarget(TargetIndex.B).Cell;
						if (!cell.IsValidStorageFor(this.<>f__ref$0.$this.Map, thing))
						{
							return true;
						}
					}
					return false;
				}
			}
		}
	}
}
