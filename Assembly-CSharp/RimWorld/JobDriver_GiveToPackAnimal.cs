using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000038 RID: 56
	public class JobDriver_GiveToPackAnimal : JobDriver
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x00014960 File Offset: 0x00012D60
		private Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0001498C File Offset: 0x00012D8C
		private Pawn Animal
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x000149BC File Offset: 0x00012DBC
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Item, this.job, 1, -1, null);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000149F0 File Offset: 0x00012DF0
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			Toil findNearestCarrier = this.FindCarrierToil();
			yield return findNearestCarrier;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.B).JumpIf(() => !this.CanCarryAtLeastOne(this.Animal), findNearestCarrier);
			yield return this.GiveToCarrierAsMuchAsPossibleToil();
			yield return Toils_Jump.JumpIf(findNearestCarrier, () => this.pawn.carryTracker.CarriedThing != null);
			yield break;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00014A1C File Offset: 0x00012E1C
		private Toil FindCarrierToil()
		{
			return new Toil
			{
				initAction = delegate()
				{
					Pawn pawn = this.FindCarrier();
					if (pawn == null)
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
					}
					else
					{
						this.job.SetTarget(TargetIndex.B, pawn);
					}
				}
			};
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00014A4C File Offset: 0x00012E4C
		private Pawn FindCarrier()
		{
			IEnumerable<Pawn> enumerable = GiveToPackAnimalUtility.CarrierCandidatesFor(this.pawn);
			Pawn animal = this.Animal;
			Pawn result;
			if (animal != null && enumerable.Contains(animal) && animal.RaceProps.packAnimal && this.CanCarryAtLeastOne(animal))
			{
				result = animal;
			}
			else
			{
				Pawn pawn = null;
				float num = -1f;
				foreach (Pawn pawn2 in enumerable)
				{
					if (pawn2.RaceProps.packAnimal && this.CanCarryAtLeastOne(pawn2))
					{
						float num2 = (float)pawn2.Position.DistanceToSquared(this.pawn.Position);
						if (pawn == null || num2 < num)
						{
							pawn = pawn2;
							num = num2;
						}
					}
				}
				result = pawn;
			}
			return result;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00014B50 File Offset: 0x00012F50
		private bool CanCarryAtLeastOne(Pawn carrier)
		{
			return !MassUtility.WillBeOverEncumberedAfterPickingUp(carrier, this.Item, 1);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00014B78 File Offset: 0x00012F78
		private Toil GiveToCarrierAsMuchAsPossibleToil()
		{
			return new Toil
			{
				initAction = delegate()
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

		// Token: 0x040001C2 RID: 450
		private const TargetIndex ItemInd = TargetIndex.A;

		// Token: 0x040001C3 RID: 451
		private const TargetIndex AnimalInd = TargetIndex.B;
	}
}
