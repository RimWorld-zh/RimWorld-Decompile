using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class LordToil_KidnapCover : LordToil_DoOpportunisticTaskOrCover
	{
		public LordToil_KidnapCover()
		{
		}

		protected override DutyDef DutyDef
		{
			get
			{
				return DutyDefOf.Kidnap;
			}
		}

		public override bool ForceHighStoryDanger
		{
			get
			{
				return this.cover;
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
			if (pawn.mindState.duty != null && pawn.mindState.duty.def == this.DutyDef && pawn.carryTracker.CarriedThing is Pawn)
			{
				target = pawn.carryTracker.CarriedThing;
				result = true;
			}
			else
			{
				Pawn pawn2;
				bool flag = KidnapAIUtility.TryFindGoodKidnapVictim(pawn, 8f, out pawn2, alreadyTakenTargets);
				target = pawn2;
				result = flag;
			}
			return result;
		}
	}
}
