using System;
using System.Collections.Generic;
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

		private Corpse Corpse
		{
			get
			{
				return (Corpse)base.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		private Building_Grave Grave
		{
			get
			{
				return (Building_Grave)base.job.GetTarget(TargetIndex.B).Thing;
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
				return (Thing)(((object)this.Grave) ?? ((object)this.Corpse));
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(this.Target, base.job, 1, -1, null);
		}

		public override string GetReport()
		{
			return (!this.InGrave || this.Grave.def != ThingDefOf.Grave) ? base.GetReport() : "ReportDiggingUpCorpse".Translate();
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			Toil gotoCorpse = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Jump.JumpIfTargetInvalid(TargetIndex.B, gotoCorpse);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private Toil FindCellToDropCorpseToil()
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				IntVec3 c = IntVec3.Invalid;
				if (!Rand.Chance(0.8f) || !this.TryFindTableCell(out c))
				{
					bool flag = false;
					IntVec3 root = default(IntVec3);
					if (RCellFinder.TryFindRandomSpotJustOutsideColony(base.pawn, out root) && CellFinder.TryRandomClosewalkCellNear(root, base.pawn.Map, 5, out c, (Predicate<IntVec3>)((IntVec3 x) => base.pawn.CanReserve(x, 1, -1, null, false) && x.GetFirstItem(base.pawn.Map) == null)))
					{
						flag = true;
					}
					if (!flag)
					{
						c = CellFinder.RandomClosewalkCellNear(base.pawn.Position, base.pawn.Map, 10, (Predicate<IntVec3>)((IntVec3 x) => base.pawn.CanReserve(x, 1, -1, null, false) && x.GetFirstItem(base.pawn.Map) == null));
					}
				}
				base.job.SetTarget(TargetIndex.C, c);
			};
			toil.atomicWithPrevious = true;
			return toil;
		}

		private Toil ForbidAndNotifyMentalStateToil()
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate
			{
				Corpse corpse = this.Corpse;
				if (corpse != null)
				{
					corpse.SetForbidden(true, true);
				}
				MentalState_CorpseObsession mentalState_CorpseObsession = base.pawn.MentalState as MentalState_CorpseObsession;
				if (mentalState_CorpseObsession != null)
				{
					mentalState_CorpseObsession.Notify_CorpseHauled();
				}
			};
			toil.atomicWithPrevious = true;
			return toil;
		}

		private bool TryFindTableCell(out IntVec3 cell)
		{
			JobDriver_HaulCorpseToPublicPlace.tmpCells.Clear();
			List<Building> allBuildingsColonist = base.pawn.Map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building building = allBuildingsColonist[i];
				if (building.def.IsTable)
				{
					CellRect.CellRectIterator iterator = building.OccupiedRect().GetIterator();
					while (!iterator.Done())
					{
						IntVec3 current = iterator.Current;
						if (base.pawn.CanReserveAndReach(current, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, false) && current.GetFirstItem(base.pawn.Map) == null)
						{
							JobDriver_HaulCorpseToPublicPlace.tmpCells.Add(current);
						}
						iterator.MoveNext();
					}
				}
			}
			bool result;
			if (!JobDriver_HaulCorpseToPublicPlace.tmpCells.Any())
			{
				cell = IntVec3.Invalid;
				result = false;
			}
			else
			{
				cell = JobDriver_HaulCorpseToPublicPlace.tmpCells.RandomElement();
				result = true;
			}
			return result;
		}
	}
}
