using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_TakeBeerOutOfFermentingBarrel : JobDriver
	{
		private const TargetIndex BarrelInd = TargetIndex.A;

		private const TargetIndex BeerToHaulInd = TargetIndex.B;

		private const TargetIndex StorageCellInd = TargetIndex.C;

		private const int Duration = 200;

		protected Building_FermentingBarrel Barrel
		{
			get
			{
				return (Building_FermentingBarrel)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected Thing Beer
		{
			get
			{
				return base.CurJob.GetTarget(TargetIndex.B).Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(200).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator3D)/*Error near IL_00ab: stateMachine*/)._003C_003Ef__this.Barrel.Fermented)).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					Thing thing = ((_003CMakeNewToils_003Ec__Iterator3D)/*Error near IL_00ea: stateMachine*/)._003C_003Ef__this.Barrel.TakeOutBeer();
					GenPlace.TryPlaceThing(thing, ((_003CMakeNewToils_003Ec__Iterator3D)/*Error near IL_00ea: stateMachine*/)._003C_003Ef__this.pawn.Position, ((_003CMakeNewToils_003Ec__Iterator3D)/*Error near IL_00ea: stateMachine*/)._003C_003Ef__this.Map, ThingPlaceMode.Near, null);
					StoragePriority currentPriority = HaulAIUtility.StoragePriorityAtFor(thing.Position, thing);
					IntVec3 c = default(IntVec3);
					if (StoreUtility.TryFindBestBetterStoreCellFor(thing, ((_003CMakeNewToils_003Ec__Iterator3D)/*Error near IL_00ea: stateMachine*/)._003C_003Ef__this.pawn, ((_003CMakeNewToils_003Ec__Iterator3D)/*Error near IL_00ea: stateMachine*/)._003C_003Ef__this.Map, currentPriority, ((_003CMakeNewToils_003Ec__Iterator3D)/*Error near IL_00ea: stateMachine*/)._003C_003Ef__this.pawn.Faction, out c, true))
					{
						((_003CMakeNewToils_003Ec__Iterator3D)/*Error near IL_00ea: stateMachine*/)._003C_003Ef__this.CurJob.SetTarget(TargetIndex.C, c);
						((_003CMakeNewToils_003Ec__Iterator3D)/*Error near IL_00ea: stateMachine*/)._003C_003Ef__this.CurJob.SetTarget(TargetIndex.B, thing);
						((_003CMakeNewToils_003Ec__Iterator3D)/*Error near IL_00ea: stateMachine*/)._003C_003Ef__this.CurJob.count = thing.stackCount;
					}
					else
					{
						((_003CMakeNewToils_003Ec__Iterator3D)/*Error near IL_00ea: stateMachine*/)._003C_003Ef__this.EndJobWith(JobCondition.Incompletable);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.C);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true);
		}
	}
}
