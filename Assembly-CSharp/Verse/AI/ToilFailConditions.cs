using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A4A RID: 2634
	public static class ToilFailConditions
	{
		// Token: 0x06003A95 RID: 14997 RVA: 0x001F0860 File Offset: 0x001EEC60
		public static Toil FailOn(this Toil toil, Func<Toil, bool> condition)
		{
			toil.AddEndCondition(delegate
			{
				JobCondition result;
				if (condition(toil))
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			return toil;
		}

		// Token: 0x06003A96 RID: 14998 RVA: 0x001F08A8 File Offset: 0x001EECA8
		public static T FailOn<T>(this T f, Func<bool> condition) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				JobCondition result;
				if (condition())
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			return f;
		}

		// Token: 0x06003A97 RID: 14999 RVA: 0x001F08E4 File Offset: 0x001EECE4
		public static T FailOnDestroyedOrNull<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				JobCondition result;
				if (f.GetActor().jobs.curJob.GetTarget(ind).Thing.DestroyedOrNull())
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			return f;
		}

		// Token: 0x06003A98 RID: 15000 RVA: 0x001F0930 File Offset: 0x001EED30
		public static T FailOnDespawnedOrNull<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				LocalTargetInfo target = f.GetActor().jobs.curJob.GetTarget(ind);
				Thing thing = target.Thing;
				JobCondition result;
				if (thing == null && target.IsValid)
				{
					result = JobCondition.Ongoing;
				}
				else if (thing == null || !thing.Spawned || thing.Map != f.GetActor().Map)
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			return f;
		}

		// Token: 0x06003A99 RID: 15001 RVA: 0x001F097C File Offset: 0x001EED7C
		public static T EndOnDespawnedOrNull<T>(this T f, TargetIndex ind, JobCondition endCondition = JobCondition.Incompletable) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				LocalTargetInfo target = f.GetActor().jobs.curJob.GetTarget(ind);
				Thing thing = target.Thing;
				JobCondition result;
				if (thing == null && target.IsValid)
				{
					result = JobCondition.Ongoing;
				}
				else if (thing == null || !thing.Spawned || thing.Map != f.GetActor().Map)
				{
					result = endCondition;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			return f;
		}

		// Token: 0x06003A9A RID: 15002 RVA: 0x001F09D0 File Offset: 0x001EEDD0
		public static T EndOnNoTargetInQueue<T>(this T f, TargetIndex ind, JobCondition endCondition = JobCondition.Incompletable) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				JobCondition result;
				if (targetQueue.NullOrEmpty<LocalTargetInfo>())
				{
					result = endCondition;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			return f;
		}

		// Token: 0x06003A9B RID: 15003 RVA: 0x001F0A24 File Offset: 0x001EEE24
		public static T FailOnDowned<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Thing thing = f.GetActor().jobs.curJob.GetTarget(ind).Thing;
				JobCondition result;
				if (((Pawn)thing).Downed)
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			return f;
		}

		// Token: 0x06003A9C RID: 15004 RVA: 0x001F0A70 File Offset: 0x001EEE70
		public static T FailOnNotDowned<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Thing thing = f.GetActor().jobs.curJob.GetTarget(ind).Thing;
				JobCondition result;
				if (!((Pawn)thing).Downed)
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			return f;
		}

		// Token: 0x06003A9D RID: 15005 RVA: 0x001F0ABC File Offset: 0x001EEEBC
		public static T FailOnNotAwake<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Thing thing = f.GetActor().jobs.curJob.GetTarget(ind).Thing;
				JobCondition result;
				if (!((Pawn)thing).Awake())
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			return f;
		}

		// Token: 0x06003A9E RID: 15006 RVA: 0x001F0B08 File Offset: 0x001EEF08
		public static T FailOnNotCasualInterruptible<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Thing thing = f.GetActor().jobs.curJob.GetTarget(ind).Thing;
				JobCondition result;
				if (!((Pawn)thing).CanCasuallyInteractNow(false))
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			return f;
		}

		// Token: 0x06003A9F RID: 15007 RVA: 0x001F0B54 File Offset: 0x001EEF54
		public static T FailOnMentalState<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn pawn = f.GetActor().jobs.curJob.GetTarget(ind).Thing as Pawn;
				JobCondition result;
				if (pawn != null && pawn.InMentalState)
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			return f;
		}

		// Token: 0x06003AA0 RID: 15008 RVA: 0x001F0BA0 File Offset: 0x001EEFA0
		public static T FailOnAggroMentalState<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn pawn = f.GetActor().jobs.curJob.GetTarget(ind).Thing as Pawn;
				JobCondition result;
				if (pawn != null && pawn.InAggroMentalState)
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			return f;
		}

		// Token: 0x06003AA1 RID: 15009 RVA: 0x001F0BEC File Offset: 0x001EEFEC
		public static T FailOnAggroMentalStateAndHostile<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn pawn = f.GetActor().jobs.curJob.GetTarget(ind).Thing as Pawn;
				JobCondition result;
				if (pawn != null && pawn.InAggroMentalState && pawn.HostileTo(f.GetActor()))
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			return f;
		}

		// Token: 0x06003AA2 RID: 15010 RVA: 0x001F0C38 File Offset: 0x001EF038
		public static T FailOnSomeonePhysicallyInteracting<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				Thing thing = actor.jobs.curJob.GetTarget(ind).Thing;
				JobCondition result;
				if (thing != null && actor.Map.physicalInteractionReservationManager.IsReserved(thing) && !actor.Map.physicalInteractionReservationManager.IsReservedBy(actor, thing))
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			return f;
		}

		// Token: 0x06003AA3 RID: 15011 RVA: 0x001F0C84 File Offset: 0x001EF084
		public static T FailOnForbidden<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
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
					if (thing == null)
					{
						result = JobCondition.Ongoing;
					}
					else if (thing.IsForbidden(actor))
					{
						result = JobCondition.Incompletable;
					}
					else
					{
						result = JobCondition.Ongoing;
					}
				}
				return result;
			});
			return f;
		}

		// Token: 0x06003AA4 RID: 15012 RVA: 0x001F0CD0 File Offset: 0x001EF0D0
		public static T FailOnDespawnedNullOrForbidden<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.FailOnDespawnedOrNull(ind);
			f.FailOnForbidden(ind);
			return f;
		}

		// Token: 0x06003AA5 RID: 15013 RVA: 0x001F0CF8 File Offset: 0x001EF0F8
		public static T FailOnDestroyedNullOrForbidden<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.FailOnDestroyedOrNull(ind);
			f.FailOnForbidden(ind);
			return f;
		}

		// Token: 0x06003AA6 RID: 15014 RVA: 0x001F0D20 File Offset: 0x001EF120
		public static T FailOnThingMissingDesignation<T>(this T f, TargetIndex ind, DesignationDef desDef) where T : IJobEndable
		{
			f.AddEndCondition(delegate
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
					if (thing == null || actor.Map.designationManager.DesignationOn(thing, desDef) == null)
					{
						result = JobCondition.Incompletable;
					}
					else
					{
						result = JobCondition.Ongoing;
					}
				}
				return result;
			});
			return f;
		}

		// Token: 0x06003AA7 RID: 15015 RVA: 0x001F0D74 File Offset: 0x001EF174
		public static T FailOnCellMissingDesignation<T>(this T f, TargetIndex ind, DesignationDef desDef) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				Job curJob = actor.jobs.curJob;
				JobCondition result;
				if (curJob.ignoreDesignations)
				{
					result = JobCondition.Ongoing;
				}
				else if (actor.Map.designationManager.DesignationAt(curJob.GetTarget(ind).Cell, desDef) == null)
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			return f;
		}

		// Token: 0x06003AA8 RID: 15016 RVA: 0x001F0DC8 File Offset: 0x001EF1C8
		public static T FailOnBurningImmobile<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				JobCondition result;
				if (f.GetActor().jobs.curJob.GetTarget(ind).ToTargetInfo(f.GetActor().Map).IsBurning())
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			return f;
		}

		// Token: 0x06003AA9 RID: 15017 RVA: 0x001F0E14 File Offset: 0x001EF214
		public static T FailOnCannotTouch<T>(this T f, TargetIndex ind, PathEndMode peMode) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				JobCondition result;
				if (!f.GetActor().CanReachImmediate(f.GetActor().jobs.curJob.GetTarget(ind), peMode))
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			return f;
		}

		// Token: 0x06003AAA RID: 15018 RVA: 0x001F0E68 File Offset: 0x001EF268
		public static T FailOnIncapable<T>(this T f, PawnCapacityDef pawnCapacity) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				JobCondition result;
				if (!f.GetActor().health.capacities.CapableOf(pawnCapacity))
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			return f;
		}

		// Token: 0x06003AAB RID: 15019 RVA: 0x001F0EB4 File Offset: 0x001EF2B4
		public static Toil FailOnDespawnedNullOrForbiddenPlacedThings(this Toil toil)
		{
			toil.AddFailCondition(delegate
			{
				bool result;
				if (toil.actor.jobs.curJob.placedThings == null)
				{
					result = false;
				}
				else
				{
					for (int i = 0; i < toil.actor.jobs.curJob.placedThings.Count; i++)
					{
						ThingCountClass thingCountClass = toil.actor.jobs.curJob.placedThings[i];
						if (thingCountClass.thing == null || !thingCountClass.thing.Spawned || thingCountClass.thing.Map != toil.actor.Map || (!toil.actor.CurJob.ignoreForbidden && thingCountClass.thing.IsForbidden(toil.actor)))
						{
							return true;
						}
					}
					result = false;
				}
				return result;
			});
			return toil;
		}
	}
}
