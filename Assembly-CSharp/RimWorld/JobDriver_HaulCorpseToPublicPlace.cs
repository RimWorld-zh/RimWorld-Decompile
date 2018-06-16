using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200006C RID: 108
	public class JobDriver_HaulCorpseToPublicPlace : JobDriver
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x0001FF78 File Offset: 0x0001E378
		private Corpse Corpse
		{
			get
			{
				return (Corpse)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0001FFA8 File Offset: 0x0001E3A8
		private Building_Grave Grave
		{
			get
			{
				return (Building_Grave)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x0001FFD8 File Offset: 0x0001E3D8
		private bool InGrave
		{
			get
			{
				return this.Grave != null;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0001FFFC File Offset: 0x0001E3FC
		private Thing Target
		{
			get
			{
				return this.Grave ?? this.Corpse;
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00020024 File Offset: 0x0001E424
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00020058 File Offset: 0x0001E458
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

		// Token: 0x060002F9 RID: 761 RVA: 0x000200A4 File Offset: 0x0001E4A4
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

		// Token: 0x060002FA RID: 762 RVA: 0x000200D0 File Offset: 0x0001E4D0
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

		// Token: 0x060002FB RID: 763 RVA: 0x00020108 File Offset: 0x0001E508
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

		// Token: 0x060002FC RID: 764 RVA: 0x00020140 File Offset: 0x0001E540
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

		// Token: 0x0400020F RID: 527
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x04000210 RID: 528
		private const TargetIndex GraveInd = TargetIndex.B;

		// Token: 0x04000211 RID: 529
		private const TargetIndex CellInd = TargetIndex.C;

		// Token: 0x04000212 RID: 530
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
