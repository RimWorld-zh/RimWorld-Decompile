using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000051 RID: 81
	public class Toils_Tend
	{
		// Token: 0x06000280 RID: 640 RVA: 0x0001AA08 File Offset: 0x00018E08
		public static Toil ReserveMedicine(TargetIndex ind, Pawn injured)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(ind).Thing;
				int num = actor.Map.reservationManager.CanReserveStack(actor, thing, 10, null, false);
				if (num <= 0 || !actor.Reserve(thing, curJob, 10, Mathf.Min(num, Medicine.GetMedicineCountToFullyHeal(injured)), null))
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0001AA74 File Offset: 0x00018E74
		public static Toil PickupMedicine(TargetIndex ind, Pawn injured)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(ind).Thing;
				int num = Medicine.GetMedicineCountToFullyHeal(injured);
				if (actor.carryTracker.CarriedThing != null)
				{
					num -= actor.carryTracker.CarriedThing.stackCount;
				}
				int num2 = Mathf.Min(actor.Map.reservationManager.CanReserveStack(actor, thing, 10, null, false), num);
				if (num2 > 0)
				{
					actor.carryTracker.TryStartCarry(thing, num2, true);
				}
				curJob.count = num - num2;
				if (thing.Spawned)
				{
					toil.actor.Map.reservationManager.Release(thing, actor, curJob);
				}
				curJob.SetTarget(ind, actor.carryTracker.CarriedThing);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0001AAD4 File Offset: 0x00018ED4
		public static Toil FinalizeTend(Pawn patient)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Medicine medicine = (Medicine)actor.CurJob.targetB.Thing;
				float num = (!patient.RaceProps.Animal) ? 500f : 175f;
				float num2 = (medicine != null) ? medicine.def.MedicineTendXpGainFactor : 0.5f;
				actor.skills.Learn(SkillDefOf.Medicine, num * num2, false);
				TendUtility.DoTend(actor, patient, medicine);
				if (medicine != null && medicine.Destroyed)
				{
					actor.CurJob.SetTarget(TargetIndex.B, LocalTargetInfo.Invalid);
				}
				if (toil.actor.CurJob.endAfterTendedOnce)
				{
					actor.jobs.EndCurrentJob(JobCondition.Succeeded, true);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x040001E6 RID: 486
		public const int MaxMedicineReservations = 10;
	}
}
