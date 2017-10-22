using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_UnloadInventory : JobDriver
	{
		private const TargetIndex OtherPawnInd = TargetIndex.A;

		private const TargetIndex ItemToHaulInd = TargetIndex.B;

		private const TargetIndex StoreCellInd = TargetIndex.C;

		private const int UnloadDuration = 10;

		private Pawn OtherPawn
		{
			get
			{
				return (Pawn)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(10);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					Pawn otherPawn = ((_003CMakeNewToils_003Ec__Iterator40)/*Error near IL_00a4: stateMachine*/)._003C_003Ef__this.OtherPawn;
					if (!otherPawn.inventory.UnloadEverything)
					{
						((_003CMakeNewToils_003Ec__Iterator40)/*Error near IL_00a4: stateMachine*/)._003C_003Ef__this.EndJobWith(JobCondition.Succeeded);
					}
					else
					{
						ThingStackPart firstUnloadableThing = otherPawn.inventory.FirstUnloadableThing;
						IntVec3 c = default(IntVec3);
						if (!firstUnloadableThing.Thing.def.EverStoreable || !((_003CMakeNewToils_003Ec__Iterator40)/*Error near IL_00a4: stateMachine*/)._003C_003Ef__this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || !StoreUtility.TryFindStoreCellNearColonyDesperate(firstUnloadableThing.Thing, ((_003CMakeNewToils_003Ec__Iterator40)/*Error near IL_00a4: stateMachine*/)._003C_003Ef__this.pawn, out c))
						{
							Thing thing = default(Thing);
							((ThingOwner)otherPawn.inventory.innerContainer).TryDrop(firstUnloadableThing.Thing, ThingPlaceMode.Near, firstUnloadableThing.Count, out thing, (Action<Thing, int>)null);
							((_003CMakeNewToils_003Ec__Iterator40)/*Error near IL_00a4: stateMachine*/)._003C_003Ef__this.EndJobWith(JobCondition.Succeeded);
							if (thing != null)
							{
								thing.SetForbidden(false, false);
							}
						}
						else
						{
							Thing thing2 = default(Thing);
							otherPawn.inventory.innerContainer.TryTransferToContainer(firstUnloadableThing.Thing, (ThingOwner)((_003CMakeNewToils_003Ec__Iterator40)/*Error near IL_00a4: stateMachine*/)._003C_003Ef__this.pawn.carryTracker.innerContainer, firstUnloadableThing.Count, out thing2, true);
							((_003CMakeNewToils_003Ec__Iterator40)/*Error near IL_00a4: stateMachine*/)._003C_003Ef__this.CurJob.count = thing2.stackCount;
							((_003CMakeNewToils_003Ec__Iterator40)/*Error near IL_00a4: stateMachine*/)._003C_003Ef__this.CurJob.SetTarget(TargetIndex.B, thing2);
							((_003CMakeNewToils_003Ec__Iterator40)/*Error near IL_00a4: stateMachine*/)._003C_003Ef__this.CurJob.SetTarget(TargetIndex.C, c);
							firstUnloadableThing.Thing.SetForbidden(false, false);
						}
					}
				}
			};
			yield return Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.C);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true);
		}
	}
}
