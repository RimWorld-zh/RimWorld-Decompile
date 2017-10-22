using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_UnloadYourInventory : JobDriver
	{
		private const TargetIndex ItemToHaulInd = TargetIndex.A;

		private const TargetIndex StoreCellInd = TargetIndex.B;

		private const int UnloadDuration = 10;

		private int countToDrop = -1;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.countToDrop, "countToDrop", -1, false);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_General.Wait(10);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					if (!((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_0063: stateMachine*/)._003C_003Ef__this.pawn.inventory.UnloadEverything)
					{
						((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_0063: stateMachine*/)._003C_003Ef__this.EndJobWith(JobCondition.Succeeded);
					}
					else
					{
						ThingStackPart firstUnloadableThing = ((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_0063: stateMachine*/)._003C_003Ef__this.pawn.inventory.FirstUnloadableThing;
						IntVec3 c = default(IntVec3);
						if (!StoreUtility.TryFindStoreCellNearColonyDesperate(firstUnloadableThing.Thing, ((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_0063: stateMachine*/)._003C_003Ef__this.pawn, out c))
						{
							Thing thing2 = default(Thing);
							((ThingOwner)((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_0063: stateMachine*/)._003C_003Ef__this.pawn.inventory.innerContainer).TryDrop(firstUnloadableThing.Thing, ThingPlaceMode.Near, firstUnloadableThing.Count, out thing2, (Action<Thing, int>)null);
							((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_0063: stateMachine*/)._003C_003Ef__this.EndJobWith(JobCondition.Succeeded);
						}
						else
						{
							((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_0063: stateMachine*/)._003C_003Ef__this.CurJob.SetTarget(TargetIndex.A, firstUnloadableThing.Thing);
							((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_0063: stateMachine*/)._003C_003Ef__this.CurJob.SetTarget(TargetIndex.B, c);
							((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_0063: stateMachine*/)._003C_003Ef__this.countToDrop = firstUnloadableThing.Count;
						}
					}
				}
			};
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.Touch);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					Thing thing = ((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_00d1: stateMachine*/)._003C_003Ef__this.CurJob.GetTarget(TargetIndex.A).Thing;
					if (thing == null || !((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_00d1: stateMachine*/)._003C_003Ef__this.pawn.inventory.innerContainer.Contains(thing))
					{
						((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_00d1: stateMachine*/)._003C_003Ef__this.EndJobWith(JobCondition.Incompletable);
					}
					else
					{
						if (!((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_00d1: stateMachine*/)._003C_003Ef__this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || !thing.def.EverStoreable)
						{
							((ThingOwner)((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_00d1: stateMachine*/)._003C_003Ef__this.pawn.inventory.innerContainer).TryDrop(thing, ThingPlaceMode.Near, ((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_00d1: stateMachine*/)._003C_003Ef__this.countToDrop, out thing, (Action<Thing, int>)null);
							((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_00d1: stateMachine*/)._003C_003Ef__this.EndJobWith(JobCondition.Succeeded);
						}
						else
						{
							((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_00d1: stateMachine*/)._003C_003Ef__this.pawn.inventory.innerContainer.TryTransferToContainer(thing, (ThingOwner)((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_00d1: stateMachine*/)._003C_003Ef__this.pawn.carryTracker.innerContainer, ((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_00d1: stateMachine*/)._003C_003Ef__this.countToDrop, out thing, true);
							((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_00d1: stateMachine*/)._003C_003Ef__this.CurJob.count = ((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_00d1: stateMachine*/)._003C_003Ef__this.countToDrop;
							((_003CMakeNewToils_003Ec__Iterator41)/*Error near IL_00d1: stateMachine*/)._003C_003Ef__this.CurJob.SetTarget(TargetIndex.A, thing);
						}
						thing.SetForbidden(false, false);
					}
				}
			};
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true);
		}
	}
}
