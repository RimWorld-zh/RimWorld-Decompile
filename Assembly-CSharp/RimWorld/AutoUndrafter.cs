using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020004E7 RID: 1255
	public class AutoUndrafter : IExposable
	{
		// Token: 0x0600165B RID: 5723 RVA: 0x000C656D File Offset: 0x000C496D
		public AutoUndrafter(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x000C657D File Offset: 0x000C497D
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastNonWaitingTick, "lastNonWaitingTick", 0, false);
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x000C6594 File Offset: 0x000C4994
		public void AutoUndraftTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0 && this.pawn.Drafted)
			{
				if ((this.pawn.jobs.curJob != null && this.pawn.jobs.curJob.def != JobDefOf.Wait_Combat) || this.AnyHostilePreventingAutoUndraft())
				{
					this.lastNonWaitingTick = Find.TickManager.TicksGame;
				}
				if (this.ShouldAutoUndraft())
				{
					this.pawn.drafter.Drafted = false;
				}
			}
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x000C6633 File Offset: 0x000C4A33
		public void Notify_Drafted()
		{
			this.lastNonWaitingTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x000C6648 File Offset: 0x000C4A48
		private bool ShouldAutoUndraft()
		{
			return Find.TickManager.TicksGame - this.lastNonWaitingTick >= 6000 && !this.AnyHostilePreventingAutoUndraft();
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x000C6694 File Offset: 0x000C4A94
		private bool AnyHostilePreventingAutoUndraft()
		{
			List<IAttackTarget> potentialTargetsFor = this.pawn.Map.attackTargetsCache.GetPotentialTargetsFor(this.pawn);
			for (int i = 0; i < potentialTargetsFor.Count; i++)
			{
				if (GenHostility.IsActiveThreatToPlayer(potentialTargetsFor[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000D09 RID: 3337
		private Pawn pawn;

		// Token: 0x04000D0A RID: 3338
		private int lastNonWaitingTick;

		// Token: 0x04000D0B RID: 3339
		private const int UndraftDelay = 6000;
	}
}
