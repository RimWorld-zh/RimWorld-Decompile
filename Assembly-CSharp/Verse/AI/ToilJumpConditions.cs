using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A47 RID: 2631
	public static class ToilJumpConditions
	{
		// Token: 0x06003AA9 RID: 15017 RVA: 0x001F1CB8 File Offset: 0x001F00B8
		public static Toil JumpIf(this Toil toil, Func<bool> jumpCondition, Toil jumpToil)
		{
			toil.AddPreTickAction(delegate
			{
				if (jumpCondition())
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			});
			return toil;
		}

		// Token: 0x06003AAA RID: 15018 RVA: 0x001F1D08 File Offset: 0x001F0108
		public static Toil JumpIfDespawnedOrNull(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return thing == null || !thing.Spawned;
			}, jumpToil);
		}

		// Token: 0x06003AAB RID: 15019 RVA: 0x001F1D4C File Offset: 0x001F014C
		public static Toil JumpIfDespawnedOrNullOrForbidden(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return thing == null || !thing.Spawned || thing.IsForbidden(toil.actor);
			}, jumpToil);
		}

		// Token: 0x06003AAC RID: 15020 RVA: 0x001F1D90 File Offset: 0x001F0190
		public static Toil JumpIfOutsideHomeArea(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return !toil.actor.Map.areaManager.Home[thing.Position];
			}, jumpToil);
		}
	}
}
