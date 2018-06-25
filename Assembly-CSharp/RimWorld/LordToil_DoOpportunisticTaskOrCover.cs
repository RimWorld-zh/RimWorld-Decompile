using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200018F RID: 399
	public abstract class LordToil_DoOpportunisticTaskOrCover : LordToil
	{
		// Token: 0x04000386 RID: 902
		public bool cover = true;

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x0004F39C File Offset: 0x0004D79C
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600083E RID: 2110
		protected abstract DutyDef DutyDef { get; }

		// Token: 0x0600083F RID: 2111
		protected abstract bool TryFindGoodOpportunisticTaskTarget(Pawn pawn, out Thing target, List<Thing> alreadyTakenTargets);

		// Token: 0x06000840 RID: 2112 RVA: 0x0004F3B4 File Offset: 0x0004D7B4
		public override void UpdateAllDuties()
		{
			List<Thing> list = null;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				Thing item = null;
				if (!this.cover || (this.TryFindGoodOpportunisticTaskTarget(pawn, out item, list) && !GenAI.InDangerousCombat(pawn)))
				{
					if (pawn.mindState.duty == null || pawn.mindState.duty.def != this.DutyDef)
					{
						pawn.mindState.duty = new PawnDuty(this.DutyDef);
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
					}
					if (list == null)
					{
						list = new List<Thing>();
					}
					list.Add(item);
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
				}
			}
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0004F4A0 File Offset: 0x0004D8A0
		public override void LordToilTick()
		{
			if (this.cover && Find.TickManager.TicksGame % 181 == 0)
			{
				List<Thing> list = null;
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					if (!pawn.Downed && pawn.mindState.duty.def == DutyDefOf.AssaultColony)
					{
						Thing thing = null;
						if (this.TryFindGoodOpportunisticTaskTarget(pawn, out thing, list) && !base.Map.reservationManager.IsReservedByAnyoneOf(thing, this.lord.faction) && !GenAI.InDangerousCombat(pawn))
						{
							pawn.mindState.duty = new PawnDuty(this.DutyDef);
							pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
							if (list == null)
							{
								list = new List<Thing>();
							}
							list.Add(thing);
						}
					}
				}
			}
		}
	}
}
