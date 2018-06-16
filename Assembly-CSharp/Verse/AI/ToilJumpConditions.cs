using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A4B RID: 2635
	public static class ToilJumpConditions
	{
		// Token: 0x06003AAC RID: 15020 RVA: 0x001F18EC File Offset: 0x001EFCEC
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

		// Token: 0x06003AAD RID: 15021 RVA: 0x001F193C File Offset: 0x001EFD3C
		public static Toil JumpIfDespawnedOrNull(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return thing == null || !thing.Spawned;
			}, jumpToil);
		}

		// Token: 0x06003AAE RID: 15022 RVA: 0x001F1980 File Offset: 0x001EFD80
		public static Toil JumpIfDespawnedOrNullOrForbidden(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return thing == null || !thing.Spawned || thing.IsForbidden(toil.actor);
			}, jumpToil);
		}

		// Token: 0x06003AAF RID: 15023 RVA: 0x001F19C4 File Offset: 0x001EFDC4
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
