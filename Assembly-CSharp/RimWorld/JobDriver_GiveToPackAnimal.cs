using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_GiveToPackAnimal.<MakeNewToils>c__Iterator8 <MakeNewToils>c__Iterator = new JobDriver_GiveToPackAnimal.<MakeNewToils>c__Iterator8();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_GiveToPackAnimal.<MakeNewToils>c__Iterator8 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		private Toil FindNearestCarrierToil()
		{
			return new Toil
			{
				initAction = delegate
				{
					Pawn pawn = this.FindNearestCarrier();
					if (pawn == null)
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
					}
					else
					{
						base.CurJob.SetTarget(TargetIndex.B, pawn);
					}
				}
			};
		}

		private Pawn FindNearestCarrier()
		{
			List<Pawn> list = base.Map.mapPawns.SpawnedPawnsInFaction(this.pawn.Faction);
			Pawn pawn = null;
			float num = -1f;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].RaceProps.packAnimal && this.CanCarryAtLeastOne(list[i]))
				{
					float num2 = (float)list[i].Position.DistanceToSquared(this.pawn.Position);
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
			return new Toil
			{
				initAction = delegate
				{
					if (this.Item == null)
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
					}
					else
					{
						int count = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(this.Animal, this.Item), this.Item.stackCount);
						this.pawn.carryTracker.innerContainer.TryTransferToContainer(this.Item, this.Animal.inventory.innerContainer, count, true);
					}
				}
			};
		}
	}
}
