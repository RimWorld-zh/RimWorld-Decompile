using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A49 RID: 2633
	public static class ToilFailConditions
	{
		// Token: 0x06003A96 RID: 14998 RVA: 0x001F0FCC File Offset: 0x001EF3CC
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

		// Token: 0x06003A97 RID: 14999 RVA: 0x001F1014 File Offset: 0x001EF414
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

		// Token: 0x06003A98 RID: 15000 RVA: 0x001F1050 File Offset: 0x001EF450
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

		// Token: 0x06003A99 RID: 15001 RVA: 0x001F109C File Offset: 0x001EF49C
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

		// Token: 0x06003A9A RID: 15002 RVA: 0x001F10E8 File Offset: 0x001EF4E8
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

		// Token: 0x06003A9B RID: 15003 RVA: 0x001F113C File Offset: 0x001EF53C
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

		// Token: 0x06003A9C RID: 15004 RVA: 0x001F1190 File Offset: 0x001EF590
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

		// Token: 0x06003A9D RID: 15005 RVA: 0x001F11DC File Offset: 0x001EF5DC
		public static T FailOnMobile<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Thing thing = f.GetActor().jobs.curJob.GetTarget(ind).Thing;
				JobCondition result;
				if (((Pawn)thing).health.State == PawnHealthState.Mobile)
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

		// Token: 0x06003A9E RID: 15006 RVA: 0x001F1228 File Offset: 0x001EF628
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

		// Token: 0x06003A9F RID: 15007 RVA: 0x001F1274 File Offset: 0x001EF674
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

		// Token: 0x06003AA0 RID: 15008 RVA: 0x001F12C0 File Offset: 0x001EF6C0
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

		// Token: 0x06003AA1 RID: 15009 RVA: 0x001F130C File Offset: 0x001EF70C
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

		// Token: 0x06003AA2 RID: 15010 RVA: 0x001F1358 File Offset: 0x001EF758
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

		// Token: 0x06003AA3 RID: 15011 RVA: 0x001F13A4 File Offset: 0x001EF7A4
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

		// Token: 0x06003AA4 RID: 15012 RVA: 0x001F13F0 File Offset: 0x001EF7F0
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

		// Token: 0x06003AA5 RID: 15013 RVA: 0x001F143C File Offset: 0x001EF83C
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

		// Token: 0x06003AA6 RID: 15014 RVA: 0x001F1488 File Offset: 0x001EF888
		public static T FailOnDespawnedNullOrForbidden<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.FailOnDespawnedOrNull(ind);
			f.FailOnForbidden(ind);
			return f;
		}

		// Token: 0x06003AA7 RID: 15015 RVA: 0x001F14B0 File Offset: 0x001EF8B0
		public static T FailOnDestroyedNullOrForbidden<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.FailOnDestroyedOrNull(ind);
			f.FailOnForbidden(ind);
			return f;
		}

		// Token: 0x06003AA8 RID: 15016 RVA: 0x001F14D8 File Offset: 0x001EF8D8
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

		// Token: 0x06003AA9 RID: 15017 RVA: 0x001F152C File Offset: 0x001EF92C
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

		// Token: 0x06003AAA RID: 15018 RVA: 0x001F1580 File Offset: 0x001EF980
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

		// Token: 0x06003AAB RID: 15019 RVA: 0x001F15CC File Offset: 0x001EF9CC
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

		// Token: 0x06003AAC RID: 15020 RVA: 0x001F1620 File Offset: 0x001EFA20
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

		// Token: 0x06003AAD RID: 15021 RVA: 0x001F166C File Offset: 0x001EFA6C
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
