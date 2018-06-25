using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200019F RID: 415
	public class LordToil_StealCover : LordToil_DoOpportunisticTaskOrCover
	{
		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x00051944 File Offset: 0x0004FD44
		protected override DutyDef DutyDef
		{
			get
			{
				return DutyDefOf.Steal;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x00051960 File Offset: 0x0004FD60
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x00051978 File Offset: 0x0004FD78
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00051990 File Offset: 0x0004FD90
		protected override bool TryFindGoodOpportunisticTaskTarget(Pawn pawn, out Thing target, List<Thing> alreadyTakenTargets)
		{
			bool result;
			if (pawn.mindState.duty != null && pawn.mindState.duty.def == this.DutyDef && pawn.carryTracker.CarriedThing != null)
			{
				target = pawn.carryTracker.CarriedThing;
				result = true;
			}
			else
			{
				result = StealAIUtility.TryFindBestItemToSteal(pawn.Position, pawn.Map, 7f, out target, pawn, alreadyTakenTargets);
			}
			return result;
		}
	}
}
