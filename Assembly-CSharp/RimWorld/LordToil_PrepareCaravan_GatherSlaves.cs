using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class LordToil_PrepareCaravan_GatherSlaves : LordToil
	{
		private IntVec3 meetingPoint;

		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		public LordToil_PrepareCaravan_GatherSlaves(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

		public override void UpdateAllDuties()
		{
			for (int i = 0; i < base.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = base.lord.ownedPawns[i];
				if (!pawn.RaceProps.Animal)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_GatherPawns, this.meetingPoint, -1f);
					pawn.mindState.duty.pawnsToGather = PawnsToGather.Slaves;
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.meetingPoint, -1f);
				}
			}
		}

		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				GatherAnimalsAndSlavesForCaravanUtility.CheckArrived(base.lord, this.meetingPoint, "AllSlavesGathered", (Predicate<Pawn>)((Pawn x) => !x.IsColonist && !x.RaceProps.Animal), (Predicate<Pawn>)((Pawn x) => GatherAnimalsAndSlavesForCaravanUtility.IsFollowingAnyone(x)));
			}
		}
	}
}
