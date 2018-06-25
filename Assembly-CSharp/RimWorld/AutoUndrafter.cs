using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020004E5 RID: 1253
	public class AutoUndrafter : IExposable
	{
		// Token: 0x04000D09 RID: 3337
		private Pawn pawn;

		// Token: 0x04000D0A RID: 3338
		private int lastNonWaitingTick;

		// Token: 0x04000D0B RID: 3339
		private const int UndraftDelay = 6000;

		// Token: 0x06001656 RID: 5718 RVA: 0x000C6905 File Offset: 0x000C4D05
		public AutoUndrafter(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x000C6915 File Offset: 0x000C4D15
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastNonWaitingTick, "lastNonWaitingTick", 0, false);
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x000C692C File Offset: 0x000C4D2C
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

		// Token: 0x06001659 RID: 5721 RVA: 0x000C69CB File Offset: 0x000C4DCB
		public void Notify_Drafted()
		{
			this.lastNonWaitingTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x000C69E0 File Offset: 0x000C4DE0
		private bool ShouldAutoUndraft()
		{
			return Find.TickManager.TicksGame - this.lastNonWaitingTick >= 6000 && !this.AnyHostilePreventingAutoUndraft();
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x000C6A2C File Offset: 0x000C4E2C
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
	}
}
