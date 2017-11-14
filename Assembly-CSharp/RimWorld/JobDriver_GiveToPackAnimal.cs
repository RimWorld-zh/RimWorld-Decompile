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
				return base.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		private Pawn Animal
		{
			get
			{
				return (Pawn)base.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(this.Item, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private Toil FindNearestCarrierToil()
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn pawn = this.FindNearestCarrier();
				if (pawn == null)
				{
					base.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
				else
				{
					base.job.SetTarget(TargetIndex.B, pawn);
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
			toil.initAction = delegate
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
