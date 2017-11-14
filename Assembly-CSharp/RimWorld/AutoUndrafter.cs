using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class AutoUndrafter : IExposable
	{
		private Pawn pawn;

		private int lastNonWaitingTick;

		private const int UndraftDelay = 5000;

		public AutoUndrafter(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastNonWaitingTick, "lastNonWaitingTick", 0, false);
		}

		public void AutoUndraftTick()
		{
			if (Find.TickManager.TicksGame % 100 != 0)
				return;
			if (!this.pawn.Drafted)
				return;
			if (this.pawn.jobs.curJob != null && this.pawn.jobs.curJob.def != JobDefOf.WaitCombat)
			{
				goto IL_0061;
			}
			if (this.AnyHostilePreventingAutoUndraft())
				goto IL_0061;
			goto IL_0071;
			IL_0071:
			if (this.ShouldAutoUndraft())
			{
				this.pawn.drafter.Drafted = false;
			}
			return;
			IL_0061:
			this.lastNonWaitingTick = Find.TickManager.TicksGame;
			goto IL_0071;
		}

		public void Notify_Drafted()
		{
			this.lastNonWaitingTick = Find.TickManager.TicksGame;
		}

		private bool ShouldAutoUndraft()
		{
			if (Find.TickManager.TicksGame - this.lastNonWaitingTick < 5000)
			{
				return false;
			}
			if (this.AnyHostilePreventingAutoUndraft())
			{
				return false;
			}
			return true;
		}

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
