using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	internal class LordToil_DefendTraderCaravan : LordToil_DefendPoint
	{
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		public LordToil_DefendTraderCaravan() : base(true)
		{
		}

		public LordToil_DefendTraderCaravan(IntVec3 defendPoint) : base(defendPoint, 28f)
		{
		}

		public override void UpdateAllDuties()
		{
			LordToilData_DefendPoint data = base.Data;
			Pawn pawn = TraderCaravanUtility.FindTrader(base.lord);
			if (pawn != null)
			{
				pawn.mindState.duty = new PawnDuty(DutyDefOf.Defend, data.defendPoint, data.defendRadius);
				for (int i = 0; i < base.lord.ownedPawns.Count; i++)
				{
					Pawn pawn2 = base.lord.ownedPawns[i];
					switch (pawn2.GetTraderCaravanRole())
					{
					case TraderCaravanRole.Carrier:
					{
						pawn2.mindState.duty = new PawnDuty(DutyDefOf.Follow, (Thing)pawn, 5f);
						pawn2.mindState.duty.locomotion = LocomotionUrgency.Walk;
						break;
					}
					case TraderCaravanRole.Chattel:
					{
						pawn2.mindState.duty = new PawnDuty(DutyDefOf.Escort, (Thing)pawn, 5f);
						pawn2.mindState.duty.locomotion = LocomotionUrgency.Walk;
						break;
					}
					case TraderCaravanRole.Guard:
					{
						pawn2.mindState.duty = new PawnDuty(DutyDefOf.Defend, data.defendPoint, data.defendRadius);
						break;
					}
					}
				}
			}
		}
	}
}
