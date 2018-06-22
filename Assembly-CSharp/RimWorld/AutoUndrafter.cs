using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020004E3 RID: 1251
	public class AutoUndrafter : IExposable
	{
		// Token: 0x06001653 RID: 5715 RVA: 0x000C65B5 File Offset: 0x000C49B5
		public AutoUndrafter(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x000C65C5 File Offset: 0x000C49C5
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastNonWaitingTick, "lastNonWaitingTick", 0, false);
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x000C65DC File Offset: 0x000C49DC
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

		// Token: 0x06001656 RID: 5718 RVA: 0x000C667B File Offset: 0x000C4A7B
		public void Notify_Drafted()
		{
			this.lastNonWaitingTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x000C6690 File Offset: 0x000C4A90
		private bool ShouldAutoUndraft()
		{
			return Find.TickManager.TicksGame - this.lastNonWaitingTick >= 6000 && !this.AnyHostilePreventingAutoUndraft();
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x000C66DC File Offset: 0x000C4ADC
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

		// Token: 0x04000D06 RID: 3334
		private Pawn pawn;

		// Token: 0x04000D07 RID: 3335
		private int lastNonWaitingTick;

		// Token: 0x04000D08 RID: 3336
		private const int UndraftDelay = 6000;
	}
}
