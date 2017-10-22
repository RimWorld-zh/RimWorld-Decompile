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
				goto IL_0063;
			}
			if (this.AnyHostilePreventingAutoUndraft())
				goto IL_0063;
			goto IL_0075;
			IL_0075:
			if (this.ShouldAutoUndraft())
			{
				this.pawn.drafter.Drafted = false;
			}
			return;
			IL_0063:
			this.lastNonWaitingTick = Find.TickManager.TicksGame;
			goto IL_0075;
		}

		public void Notify_Drafted()
		{
			this.lastNonWaitingTick = Find.TickManager.TicksGame;
		}

		private bool ShouldAutoUndraft()
		{
			return (byte)((Find.TickManager.TicksGame - this.lastNonWaitingTick >= 5000) ? ((!this.AnyHostilePreventingAutoUndraft()) ? 1 : 0) : 0) != 0;
		}

		private bool AnyHostilePreventingAutoUndraft()
		{
			List<IAttackTarget> potentialTargetsFor = this.pawn.Map.attackTargetsCache.GetPotentialTargetsFor(this.pawn);
			int num = 0;
			bool result;
			while (true)
			{
				if (num < potentialTargetsFor.Count)
				{
					if (GenHostility.IsActiveThreatToPlayer(potentialTargetsFor[num]))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}
	}
}
