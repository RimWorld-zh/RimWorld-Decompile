using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class LordToil_PrepareCaravan_GatherSlaves : LordToil
	{
		private IntVec3 meetingPoint;

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__mg$cache0;

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache0;

		public LordToil_PrepareCaravan_GatherSlaves(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

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

		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
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
				Lord lord = this.lord;
				IntVec3 intVec = this.meetingPoint;
				string memo = "AllSlavesGathered";
				Predicate<Pawn> shouldCheckIfArrived = (Pawn x) => !x.IsColonist && !x.RaceProps.Animal;
				if (LordToil_PrepareCaravan_GatherSlaves.<>f__mg$cache0 == null)
				{
					LordToil_PrepareCaravan_GatherSlaves.<>f__mg$cache0 = new Predicate<Pawn>(GatherAnimalsAndSlavesForCaravanUtility.IsFollowingAnyone);
				}
				GatherAnimalsAndSlavesForCaravanUtility.CheckArrived(lord, intVec, memo, shouldCheckIfArrived, LordToil_PrepareCaravan_GatherSlaves.<>f__mg$cache0);
			}
		}

		[CompilerGenerated]
		private static bool <LordToilTick>m__0(Pawn x)
		{
			return !x.IsColonist && !x.RaceProps.Animal;
		}
	}
}
