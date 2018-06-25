using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A49 RID: 2633
	public static class ToilJumpConditions
	{
		// Token: 0x06003AAD RID: 15021 RVA: 0x001F1DE4 File Offset: 0x001F01E4
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

		// Token: 0x06003AAE RID: 15022 RVA: 0x001F1E34 File Offset: 0x001F0234
		public static Toil JumpIfDespawnedOrNull(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return thing == null || !thing.Spawned;
			}, jumpToil);
		}

		// Token: 0x06003AAF RID: 15023 RVA: 0x001F1E78 File Offset: 0x001F0278
		public static Toil JumpIfDespawnedOrNullOrForbidden(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return thing == null || !thing.Spawned || thing.IsForbidden(toil.actor);
			}, jumpToil);
		}

		// Token: 0x06003AB0 RID: 15024 RVA: 0x001F1EBC File Offset: 0x001F02BC
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
