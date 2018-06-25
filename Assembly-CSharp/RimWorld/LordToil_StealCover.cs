using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class LordToil_StealCover : LordToil_DoOpportunisticTaskOrCover
	{
		public LordToil_StealCover()
		{
		}

		protected override DutyDef DutyDef
		{
			get
			{
				return DutyDefOf.Steal;
			}
		}

		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

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
