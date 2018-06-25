using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A4D RID: 2637
	public static class Toils_General
	{
		// Token: 0x06003AB7 RID: 15031 RVA: 0x001F28F4 File Offset: 0x001F0CF4
		public static Toil Wait(int ticks)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.pather.StopDead();
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = ticks;
			return toil;
		}

		// Token: 0x06003AB8 RID: 15032 RVA: 0x001F2950 File Offset: 0x001F0D50
		public static Toil WaitWith(TargetIndex targetInd, int ticks, bool useProgressBar = false, bool maintainPosture = false)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.pather.StopDead();
				Pawn pawn = toil.actor.CurJob.GetTarget(targetInd).Thing as Pawn;
				if (pawn != null)
				{
					if (pawn == toil.actor)
					{
						Log.Warning("Executing WaitWith toil but otherPawn is the same as toil.actor", false);
					}
					else
					{
						Pawn pawn2 = pawn;
						int ticks2 = ticks;
						bool maintainPosture2 = maintainPosture;
						PawnUtility.ForceWait(pawn2, ticks2, null, maintainPosture2);
					}
				}
			};
			toil.FailOnDespawnedOrNull(targetInd);
			toil.FailOnCannotTouch(targetInd, PathEndMode.Touch);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = ticks;
			if (useProgressBar)
			{
				toil.WithProgressBarToilDelay(targetInd, false, -0.5f);
			}
			return toil;
		}

		// Token: 0x06003AB9 RID: 15033 RVA: 0x001F2A08 File Offset: 0x001F0E08
		public static Toil RemoveDesignationsOnThing(TargetIndex ind, DesignationDef def)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.Map.designationManager.RemoveAllDesignationsOn(toil.actor.jobs.curJob.GetTarget(ind).Thing, false);
			};
			return toil;
		}

		// Token: 0x06003ABA RID: 15034 RVA: 0x001F2A54 File Offset: 0x001F0E54
		public static Toil ClearTarget(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.GetActor().CurJob.SetTarget(ind, null);
			};
			return toil;
		}

		// Token: 0x06003ABB RID: 15035 RVA: 0x001F2AA0 File Offset: 0x001F0EA0
		public static Toil PutCarriedThingInInventory()
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.GetActor();
				if (actor.carryTracker.CarriedThing != null)
				{
					if (!actor.carryTracker.innerContainer.TryTransferToContainer(actor.carryTracker.CarriedThing, actor.inventory.innerContainer, true))
					{
						Thing thing;
						actor.carryTracker.TryDropCarriedThing(actor.Position, actor.carryTracker.CarriedThing.stackCount, ThingPlaceMode.Near, out thing, null);
					}
				}
			};
			return toil;
		}

		// Token: 0x06003ABC RID: 15036 RVA: 0x001F2AE4 File Offset: 0x001F0EE4
		public static Toil Do(Action action)
		{
			return new Toil
			{
				initAction = action
			};
		}

		// Token: 0x06003ABD RID: 15037 RVA: 0x001F2B08 File Offset: 0x001F0F08
		public static Toil DoAtomic(Action action)
		{
			return new Toil
			{
				initAction = action,
				atomicWithPrevious = true
			};
		}

		// Token: 0x06003ABE RID: 15038 RVA: 0x001F2B34 File Offset: 0x001F0F34
		public static Toil Open(TargetIndex openableInd)
		{
			Toil open = new Toil();
			open.initAction = delegate()
			{
				Pawn actor = open.actor;
				Thing thing = actor.CurJob.GetTarget(openableInd).Thing;
				Designation designation = actor.Map.designationManager.DesignationOn(thing, DesignationDefOf.Open);
				if (designation != null)
				{
					designation.Delete();
				}
				IOpenable openable = (IOpenable)thing;
				if (openable.CanOpen)
				{
					openable.Open();
					actor.records.Increment(RecordDefOf.ContainersOpened);
				}
			};
			open.defaultCompleteMode = ToilCompleteMode.Instant;
			return open;
		}

		// Token: 0x06003ABF RID: 15039 RVA: 0x001F2B8C File Offset: 0x001F0F8C
		public static Toil Label()
		{
			return new Toil
			{
				atomicWithPrevious = true,
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}
