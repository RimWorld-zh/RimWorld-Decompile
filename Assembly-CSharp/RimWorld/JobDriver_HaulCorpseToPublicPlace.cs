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
	public class JobDriver_HaulCorpseToPublicPlace : JobDriver
	{
		private const TargetIndex CorpseInd = TargetIndex.A;

		private const TargetIndex GraveInd = TargetIndex.B;

		private const TargetIndex CellInd = TargetIndex.C;

		private static List<IntVec3> tmpCells = new List<IntVec3>();

		public JobDriver_HaulCorpseToPublicPlace()
		{
		}

		private Corpse Corpse
		{
			get
			{
				return (Corpse)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		private Building_Grave Grave
		{
			get
			{
				return (Building_Grave)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		private bool InGrave
		{
			get
			{
				return this.Grave != null;
			}
		}

		private Thing Target
		{
			get
			{
				return this.Grave ?? this.Corpse;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null);
		}

		public override string GetReport()
		{
			string result;
			if (this.InGrave && this.Grave.def == ThingDefOf.Grave)
			{
				result = "ReportDiggingUpCorpse".Translate();
			}
			else
			{
				result = base.GetReport();
			}
			return result;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			Toil gotoCorpse = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Jump.JumpIfTargetInvalid(TargetIndex.B, gotoCorpse);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell).FailOnDespawnedOrNull(TargetIndex.B);
			yield return Toils_General.Wait(300).WithProgressBarToilDelay(TargetIndex.B, false, -0.5f).FailOnDespawnedOrNull(TargetIndex.B).FailOnCannotTouch(TargetIndex.B, PathEndMode.InteractionCell);
			yield return Toils_General.Open(TargetIndex.B);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return gotoCorpse;
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			yield return this.FindCellToDropCorpseToil();
			yield return Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
			yield return Toils_Goto.GotoCell(TargetIndex.C, PathEndMode.Touch);
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, null, false);
			yield return this.ForbidAndNotifyMentalStateToil();
			yield break;
		}

		private Toil FindCellToDropCorpseToil()
		{
			return new Toil
			{
				initAction = delegate()
				{
					IntVec3 c = IntVec3.Invalid;
					if (!Rand.Chance(0.8f) || !this.TryFindTableCell(out c))
					{
						bool flag = false;
						IntVec3 root;
						if (RCellFinder.TryFindRandomSpotJustOutsideColony(this.pawn, out root) && CellFinder.TryRandomClosewalkCellNear(root, this.pawn.Map, 5, out c, (IntVec3 x) => this.pawn.CanReserve(x, 1, -1, null, false) && x.GetFirstItem(this.pawn.Map) == null))
						{
							flag = true;
						}
						if (!flag)
						{
							c = CellFinder.RandomClosewalkCellNear(this.pawn.Position, this.pawn.Map, 10, (IntVec3 x) => this.pawn.CanReserve(x, 1, -1, null, false) && x.GetFirstItem(this.pawn.Map) == null);
						}
					}
					this.job.SetTarget(TargetIndex.C, c);
				},
				atomicWithPrevious = true
			};
		}

		private Toil ForbidAndNotifyMentalStateToil()
		{
			return new Toil
			{
				initAction = delegate()
				{
					Corpse corpse = this.Corpse;
					if (corpse != null)
					{
						corpse.SetForbidden(true, true);
					}
					MentalState_CorpseObsession mentalState_CorpseObsession = this.pawn.MentalState as MentalState_CorpseObsession;
					if (mentalState_CorpseObsession != null)
					{
						mentalState_CorpseObsession.Notify_CorpseHauled();
					}
				},
				atomicWithPrevious = true
			};
		}

		private bool TryFindTableCell(out IntVec3 cell)
		{
			JobDriver_HaulCorpseToPublicPlace.tmpCells.Clear();
			List<Building> allBuildingsColonist = this.pawn.Map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building building = allBuildingsColonist[i];
				if (building.def.IsTable)
				{
					CellRect.CellRectIterator iterator = building.OccupiedRect().GetIterator();
					while (!iterator.Done())
					{
						IntVec3 intVec = iterator.Current;
						if (this.pawn.CanReserveAndReach(intVec, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, false) && intVec.GetFirstItem(this.pawn.Map) == null)
						{
							JobDriver_HaulCorpseToPublicPlace.tmpCells.Add(intVec);
						}
						iterator.MoveNext();
					}
				}
			}
			bool result;
			if (!JobDriver_HaulCorpseToPublicPlace.tmpCells.Any<IntVec3>())
			{
				cell = IntVec3.Invalid;
				result = false;
			}
			else
			{
				cell = JobDriver_HaulCorpseToPublicPlace.tmpCells.RandomElement<IntVec3>();
				result = true;
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static JobDriver_HaulCorpseToPublicPlace()
		{
		}

		[CompilerGenerated]
		private void <FindCellToDropCorpseToil>m__0()
		{
			IntVec3 c = IntVec3.Invalid;
			if (!Rand.Chance(0.8f) || !this.TryFindTableCell(out c))
			{
				bool flag = false;
				IntVec3 root;
				if (RCellFinder.TryFindRandomSpotJustOutsideColony(this.pawn, out root) && CellFinder.TryRandomClosewalkCellNear(root, this.pawn.Map, 5, out c, (IntVec3 x) => this.pawn.CanReserve(x, 1, -1, null, false) && x.GetFirstItem(this.pawn.Map) == null))
				{
					flag = true;
				}
				if (!flag)
				{
					c = CellFinder.RandomClosewalkCellNear(this.pawn.Position, this.pawn.Map, 10, (IntVec3 x) => this.pawn.CanReserve(x, 1, -1, null, false) && x.GetFirstItem(this.pawn.Map) == null);
				}
			}
			this.job.SetTarget(TargetIndex.C, c);
		}

		[CompilerGenerated]
		private void <ForbidAndNotifyMentalStateToil>m__1()
		{
			Corpse corpse = this.Corpse;
			if (corpse != null)
			{
				corpse.SetForbidden(true, true);
			}
			MentalState_CorpseObsession mentalState_CorpseObsession = this.pawn.MentalState as MentalState_CorpseObsession;
			if (mentalState_CorpseObsession != null)
			{
				mentalState_CorpseObsession.Notify_CorpseHauled();
			}
		}

		[CompilerGenerated]
		private bool <FindCellToDropCorpseToil>m__2(IntVec3 x)
		{
			return this.pawn.CanReserve(x, 1, -1, null, false) && x.GetFirstItem(this.pawn.Map) == null;
		}

		[CompilerGenerated]
		private bool <FindCellToDropCorpseToil>m__3(IntVec3 x)
		{
			return this.pawn.CanReserve(x, 1, -1, null, false) && x.GetFirstItem(this.pawn.Map) == null;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <gotoCorpse>__0;

			internal JobDriver_HaulCorpseToPublicPlace $this;

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
					this.FailOnDestroyedOrNull(TargetIndex.A);
					gotoCorpse = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
					this.$current = Toils_Jump.JumpIfTargetInvalid(TargetIndex.B, gotoCorpse);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell).FailOnDespawnedOrNull(TargetIndex.B);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = Toils_General.Wait(300).WithProgressBarToilDelay(TargetIndex.B, false, -0.5f).FailOnDespawnedOrNull(TargetIndex.B).FailOnCannotTouch(TargetIndex.B, PathEndMode.InteractionCell);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = Toils_General.Open(TargetIndex.B);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = gotoCorpse;
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$current = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
					this.$current = base.FindCellToDropCorpseToil();
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				case 8u:
					this.$current = Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
					if (!this.$disposing)
					{
						this.$PC = 9;
					}
					return true;
				case 9u:
					this.$current = Toils_Goto.GotoCell(TargetIndex.C, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 10;
					}
					return true;
				case 10u:
					this.$current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, null, false);
					if (!this.$disposing)
					{
						this.$PC = 11;
					}
					return true;
				case 11u:
					this.$current = base.ForbidAndNotifyMentalStateToil();
					if (!this.$disposing)
					{
						this.$PC = 12;
					}
					return true;
				case 12u:
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
				JobDriver_HaulCorpseToPublicPlace.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_HaulCorpseToPublicPlace.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}
		}
	}
}
