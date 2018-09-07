using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class Toils_Tend
	{
		public const int MaxMedicineReservations = 10;

		public Toils_Tend()
		{
		}

		public static Toil ReserveMedicine(TargetIndex ind, Pawn injured)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(ind).Thing;
				int num = actor.Map.reservationManager.CanReserveStack(actor, thing, 10, null, false);
				if (num <= 0 || !actor.Reserve(thing, curJob, 10, Mathf.Min(num, Medicine.GetMedicineCountToFullyHeal(injured)), null, true))
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

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

		[CompilerGenerated]
		private sealed class <ReserveMedicine>c__AnonStorey0
		{
			internal Toil toil;

			internal TargetIndex ind;

			internal Pawn injured;

			public <ReserveMedicine>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(this.ind).Thing;
				int num = actor.Map.reservationManager.CanReserveStack(actor, thing, 10, null, false);
				if (num <= 0 || !actor.Reserve(thing, curJob, 10, Mathf.Min(num, Medicine.GetMedicineCountToFullyHeal(this.injured)), null, true))
				{
					this.toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <PickupMedicine>c__AnonStorey1
		{
			internal Toil toil;

			internal TargetIndex ind;

			internal Pawn injured;

			public <PickupMedicine>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(this.ind).Thing;
				int num = Medicine.GetMedicineCountToFullyHeal(this.injured);
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
					this.toil.actor.Map.reservationManager.Release(thing, actor, curJob);
				}
				curJob.SetTarget(this.ind, actor.carryTracker.CarriedThing);
			}
		}

		[CompilerGenerated]
		private sealed class <FinalizeTend>c__AnonStorey2
		{
			internal Toil toil;

			internal Pawn patient;

			public <FinalizeTend>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Medicine medicine = (Medicine)actor.CurJob.targetB.Thing;
				float num = (!this.patient.RaceProps.Animal) ? 500f : 175f;
				float num2 = (medicine != null) ? medicine.def.MedicineTendXpGainFactor : 0.5f;
				actor.skills.Learn(SkillDefOf.Medicine, num * num2, false);
				TendUtility.DoTend(actor, this.patient, medicine);
				if (medicine != null && medicine.Destroyed)
				{
					actor.CurJob.SetTarget(TargetIndex.B, LocalTargetInfo.Invalid);
				}
				if (this.toil.actor.CurJob.endAfterTendedOnce)
				{
					actor.jobs.EndCurrentJob(JobCondition.Succeeded, true);
				}
			}
		}
	}
}
