using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class Toils_Tend
	{
		public static Toil PickupMedicine(TargetIndex ind, Pawn injured)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(ind).Thing;
				int num = Medicine.GetMedicineCountToFullyHeal(injured);
				if (actor.carryTracker.CarriedThing != null)
				{
					num -= actor.carryTracker.CarriedThing.stackCount;
				}
				int num2 = Mathf.Min(thing.stackCount, num);
				if (num2 > 0)
				{
					actor.carryTracker.TryStartCarry(thing, num2);
				}
				curJob.count = num - num2;
				if (thing.Spawned)
				{
					toil.actor.Map.reservationManager.Release(thing, actor);
				}
				curJob.SetTarget(ind, actor.carryTracker.CarriedThing);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		public static Toil FinalizeTend(Pawn patient)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				Pawn actor = toil.actor;
				Medicine medicine = (Medicine)actor.jobs.curJob.targetB.Thing;
				float num = (float)((!patient.RaceProps.Animal) ? 500.0 : 175.0);
				float num2 = (float)((medicine != null) ? medicine.def.MedicineTendXpGainFactor : 0.5);
				actor.skills.Learn(SkillDefOf.Medicine, num * num2, false);
				TendUtility.DoTend(actor, patient, medicine);
				if (medicine != null && medicine.Destroyed)
				{
					actor.CurJob.SetTarget(TargetIndex.B, LocalTargetInfo.Invalid);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}
	}
}
