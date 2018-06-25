using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A50 RID: 2640
	public static class Toils_Jump
	{
		// Token: 0x06003ACA RID: 15050 RVA: 0x001F388C File Offset: 0x001F1C8C
		public static Toil Jump(Toil jumpTarget)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.jobs.curDriver.JumpToToil(jumpTarget);
			};
			return toil;
		}

		// Token: 0x06003ACB RID: 15051 RVA: 0x001F38D8 File Offset: 0x001F1CD8
		public static Toil JumpIf(Toil jumpTarget, Func<bool> condition)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				if (condition())
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpTarget);
				}
			};
			return toil;
		}

		// Token: 0x06003ACC RID: 15052 RVA: 0x001F392C File Offset: 0x001F1D2C
		public static Toil JumpIfTargetDespawnedOrNull(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				if (thing == null || !thing.Spawned)
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x06003ACD RID: 15053 RVA: 0x001F3980 File Offset: 0x001F1D80
		public static Toil JumpIfTargetInvalid(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				if (!toil.actor.jobs.curJob.GetTarget(ind).IsValid)
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x06003ACE RID: 15054 RVA: 0x001F39D4 File Offset: 0x001F1DD4
		public static Toil JumpIfTargetNotHittable(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				LocalTargetInfo target = curJob.GetTarget(ind);
				if (curJob.verbToUse == null || !curJob.verbToUse.IsStillUsableBy(actor) || !curJob.verbToUse.CanHitTarget(target))
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x06003ACF RID: 15055 RVA: 0x001F3A28 File Offset: 0x001F1E28
		public static Toil JumpIfTargetDowned(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Pawn pawn = curJob.GetTarget(ind).Thing as Pawn;
				if (pawn != null && pawn.Downed)
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x06003AD0 RID: 15056 RVA: 0x001F3A7C File Offset: 0x001F1E7C
		public static Toil JumpIfHaveTargetInQueue(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				if (!targetQueue.NullOrEmpty<LocalTargetInfo>())
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x06003AD1 RID: 15057 RVA: 0x001F3AD0 File Offset: 0x001F1ED0
		public static Toil JumpIfCannotTouch(TargetIndex ind, PathEndMode peMode, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				LocalTargetInfo target = curJob.GetTarget(ind);
				if (!actor.CanReachImmediate(target, peMode))
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}
	}
}
