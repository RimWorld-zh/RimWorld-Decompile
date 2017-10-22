using RimWorld;
using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public static class ToilFailConditions
	{
		public static Toil FailOn(this Toil toil, Func<Toil, bool> condition)
		{
			toil.AddEndCondition((Func<JobCondition>)(() => (JobCondition)((!condition(toil)) ? 1 : 3)));
			return toil;
		}

		public static T FailOn<T>(this T f, Func<bool> condition) where T : IJobEndable
		{
			f.AddEndCondition((Func<JobCondition>)(() => (JobCondition)((!condition()) ? 1 : 3)));
			return f;
		}

		public static T FailOnDestroyedOrNull<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition((Func<JobCondition>)(() => (JobCondition)((!f.GetActor().jobs.curJob.GetTarget(ind).Thing.DestroyedOrNull()) ? 1 : 3)));
			return f;
		}

		public static T FailOnDespawnedOrNull<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition((Func<JobCondition>)delegate()
			{
				LocalTargetInfo target = f.GetActor().jobs.curJob.GetTarget(ind);
				Thing thing = target.Thing;
				return (JobCondition)((thing == null && target.IsValid) ? 1 : ((thing != null && thing.Spawned && thing.Map == f.GetActor().Map) ? 1 : 3));
			});
			return f;
		}

		public static T EndOnDespawnedOrNull<T>(this T f, TargetIndex ind, JobCondition endCondition = JobCondition.Incompletable) where T : IJobEndable
		{
			f.AddEndCondition((Func<JobCondition>)delegate()
			{
				LocalTargetInfo target = f.GetActor().jobs.curJob.GetTarget(ind);
				Thing thing = target.Thing;
				return (thing == null && target.IsValid) ? JobCondition.Ongoing : ((thing != null && thing.Spawned && thing.Map == f.GetActor().Map) ? JobCondition.Ongoing : endCondition);
			});
			return f;
		}

		public static T EndOnNoTargetInQueue<T>(this T f, TargetIndex ind, JobCondition endCondition = JobCondition.Incompletable) where T : IJobEndable
		{
			f.AddEndCondition((Func<JobCondition>)delegate()
			{
				Pawn actor = f.GetActor();
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				return (!((IList<LocalTargetInfo>)targetQueue).NullOrEmpty<LocalTargetInfo>()) ? JobCondition.Ongoing : endCondition;
			});
			return f;
		}

		public static T FailOnDowned<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition((Func<JobCondition>)delegate()
			{
				Thing thing = f.GetActor().jobs.curJob.GetTarget(ind).Thing;
				return (JobCondition)((!((Pawn)thing).Downed) ? 1 : 3);
			});
			return f;
		}

		public static T FailOnNotDowned<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition((Func<JobCondition>)delegate()
			{
				Thing thing = f.GetActor().jobs.curJob.GetTarget(ind).Thing;
				return (JobCondition)(((Pawn)thing).Downed ? 1 : 3);
			});
			return f;
		}

		public static T FailOnNotAwake<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition((Func<JobCondition>)delegate()
			{
				Thing thing = f.GetActor().jobs.curJob.GetTarget(ind).Thing;
				return (JobCondition)(((Pawn)thing).Awake() ? 1 : 3);
			});
			return f;
		}

		public static T FailOnNotCasualInterruptible<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition((Func<JobCondition>)delegate()
			{
				Thing thing = f.GetActor().jobs.curJob.GetTarget(ind).Thing;
				return (JobCondition)(((Pawn)thing).CanCasuallyInteractNow(false) ? 1 : 3);
			});
			return f;
		}

		public static T FailOnMentalState<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition((Func<JobCondition>)delegate()
			{
				Pawn pawn = f.GetActor().jobs.curJob.GetTarget(ind).Thing as Pawn;
				return (JobCondition)((pawn == null || !pawn.InMentalState) ? 1 : 3);
			});
			return f;
		}

		public static T FailOnAggroMentalState<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition((Func<JobCondition>)delegate()
			{
				Pawn pawn = f.GetActor().jobs.curJob.GetTarget(ind).Thing as Pawn;
				return (JobCondition)((pawn == null || !pawn.InAggroMentalState) ? 1 : 3);
			});
			return f;
		}

		public static T FailOnAggroMentalStateAndHostile<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition((Func<JobCondition>)delegate()
			{
				Pawn pawn = f.GetActor().jobs.curJob.GetTarget(ind).Thing as Pawn;
				return (JobCondition)((pawn == null || !pawn.InAggroMentalState || !pawn.HostileTo(f.GetActor())) ? 1 : 3);
			});
			return f;
		}

		public static T FailOnSomeonePhysicallyInteracting<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition((Func<JobCondition>)delegate()
			{
				Pawn actor = f.GetActor();
				Thing thing = actor.jobs.curJob.GetTarget(ind).Thing;
				return (JobCondition)((thing == null || !actor.Map.physicalInteractionReservationManager.IsReserved(thing) || actor.Map.physicalInteractionReservationManager.IsReservedBy(actor, thing)) ? 1 : 3);
			});
			return f;
		}

		public static T FailOnForbidden<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition((Func<JobCondition>)delegate()
			{
				Pawn actor = f.GetActor();
				JobCondition result;
				if (actor.Faction != Faction.OfPlayer)
				{
					result = JobCondition.Ongoing;
				}
				else if (actor.jobs.curJob.ignoreForbidden)
				{
					result = JobCondition.Ongoing;
				}
				else
				{
					Thing thing = actor.jobs.curJob.GetTarget(ind).Thing;
					result = (JobCondition)((thing == null) ? 1 : ((!thing.IsForbidden(actor)) ? 1 : 3));
				}
				return result;
			});
			return f;
		}

		public static T FailOnDespawnedNullOrForbidden<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.FailOnDespawnedOrNull<T>(ind);
			f.FailOnForbidden<T>(ind);
			return f;
		}

		public static T FailOnDestroyedNullOrForbidden<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.FailOnDestroyedOrNull<T>(ind);
			f.FailOnForbidden<T>(ind);
			return f;
		}

		public static T FailOnThingMissingDesignation<T>(this T f, TargetIndex ind, DesignationDef desDef) where T : IJobEndable
		{
			f.AddEndCondition((Func<JobCondition>)delegate()
			{
				Pawn actor = f.GetActor();
				Job curJob = actor.jobs.curJob;
				JobCondition result;
				if (curJob.ignoreDesignations)
				{
					result = JobCondition.Ongoing;
				}
				else
				{
					Thing thing = curJob.GetTarget(ind).Thing;
					result = (JobCondition)((thing != null && actor.Map.designationManager.DesignationOn(thing, desDef) != null) ? 1 : 3);
				}
				return result;
			});
			return f;
		}

		public static T FailOnCellMissingDesignation<T>(this T f, TargetIndex ind, DesignationDef desDef) where T : IJobEndable
		{
			f.AddEndCondition((Func<JobCondition>)delegate()
			{
				Pawn actor = f.GetActor();
				Job curJob = actor.jobs.curJob;
				return (JobCondition)(curJob.ignoreDesignations ? 1 : ((actor.Map.designationManager.DesignationAt(curJob.GetTarget(ind).Cell, desDef) != null) ? 1 : 3));
			});
			return f;
		}

		public static T FailOnBurningImmobile<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition((Func<JobCondition>)(() => (JobCondition)((!f.GetActor().jobs.curJob.GetTarget(ind).ToTargetInfo(f.GetActor().Map).IsBurning()) ? 1 : 3)));
			return f;
		}

		public static T FailOnCannotTouch<T>(this T f, TargetIndex ind, PathEndMode peMode) where T : IJobEndable
		{
			f.AddEndCondition((Func<JobCondition>)(() => (JobCondition)(f.GetActor().CanReachImmediate(f.GetActor().jobs.curJob.GetTarget(ind), peMode) ? 1 : 3)));
			return f;
		}

		public static Toil FailOnDespawnedNullOrForbiddenPlacedThings(this Toil toil)
		{
			toil.AddFailCondition((Func<bool>)delegate()
			{
				bool result;
				if (toil.actor.jobs.curJob.placedThings == null)
				{
					result = false;
				}
				else
				{
					int num = 0;
					while (num < toil.actor.jobs.curJob.placedThings.Count)
					{
						ThingStackPartClass thingStackPartClass = toil.actor.jobs.curJob.placedThings[num];
						if (thingStackPartClass.thing != null && thingStackPartClass.thing.Spawned && thingStackPartClass.thing.Map == toil.actor.Map && (toil.actor.CurJob.ignoreForbidden || !thingStackPartClass.thing.IsForbidden(toil.actor)))
						{
							num++;
							continue;
						}
						goto IL_00c0;
					}
					result = false;
				}
				goto IL_00f9;
				IL_00c0:
				result = true;
				goto IL_00f9;
				IL_00f9:
				return result;
			});
			return toil;
		}
	}
}
