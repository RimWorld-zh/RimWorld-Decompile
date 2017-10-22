using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_GiveToPackAnimal : JobDriver
	{
		private const TargetIndex ItemInd = TargetIndex.A;

		private const TargetIndex AnimalInd = TargetIndex.B;

		private Thing Item
		{
			get
			{
				return base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		private Pawn Animal
		{
			get
			{
				return (Pawn)base.CurJob.GetTarget(TargetIndex.B).Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false);
			Toil findNearestCarrier = this.FindNearestCarrierToil();
			yield return findNearestCarrier;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.B).JumpIf((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator8)/*Error near IL_00be: stateMachine*/)._003C_003Ef__this.CanCarryAtLeastOne(((_003CMakeNewToils_003Ec__Iterator8)/*Error near IL_00be: stateMachine*/)._003C_003Ef__this.Animal)), findNearestCarrier);
			yield return this.GiveToCarrierAsMuchAsPossibleToil();
			yield return Toils_Jump.JumpIf(findNearestCarrier, (Func<bool>)(() => ((_003CMakeNewToils_003Ec__Iterator8)/*Error near IL_010a: stateMachine*/)._003C_003Ef__this.pawn.carryTracker.CarriedThing != null));
		}

		private Toil FindNearestCarrierToil()
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate
			{
				Pawn pawn = this.FindNearestCarrier();
				if (pawn == null)
				{
					base.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
				else
				{
					base.CurJob.SetTarget(TargetIndex.B, (Thing)pawn);
				}
			};
			return toil;
		}

		private Pawn FindNearestCarrier()
		{
			List<Pawn> list = base.Map.mapPawns.SpawnedPawnsInFaction(base.pawn.Faction);
			Pawn pawn = null;
			float num = -1f;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].RaceProps.packAnimal && this.CanCarryAtLeastOne(list[i]))
				{
					float num2 = (float)list[i].Position.DistanceToSquared(base.pawn.Position);
					if (pawn == null || num2 < num)
					{
						pawn = list[i];
						num = num2;
					}
				}
			}
			return pawn;
		}

		private bool CanCarryAtLeastOne(Pawn carrier)
		{
			return !MassUtility.WillBeOverEncumberedAfterPickingUp(carrier, this.Item, 1);
		}

		private Toil GiveToCarrierAsMuchAsPossibleToil()
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate
			{
				if (this.Item == null)
				{
					base.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
				else
				{
					int count = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(this.Animal, this.Item), this.Item.stackCount);
					base.pawn.carryTracker.innerContainer.TryTransferToContainer(this.Item, this.Animal.inventory.innerContainer, count, true);
				}
			};
			return toil;
		}
	}
}
