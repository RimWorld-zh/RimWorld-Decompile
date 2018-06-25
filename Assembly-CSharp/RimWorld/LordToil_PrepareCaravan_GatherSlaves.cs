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
		private static Predicate<Pawn> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache1;

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
				GatherAnimalsAndSlavesForCaravanUtility.CheckArrived(this.lord, this.meetingPoint, "AllSlavesGathered", (Pawn x) => !x.IsColonist && !x.RaceProps.Animal, (Pawn x) => GatherAnimalsAndSlavesForCaravanUtility.IsFollowingAnyone(x));
			}
		}

		[CompilerGenerated]
		private static bool <LordToilTick>m__0(Pawn x)
		{
			return !x.IsColonist && !x.RaceProps.Animal;
		}

		[CompilerGenerated]
		private static bool <LordToilTick>m__1(Pawn x)
		{
			return GatherAnimalsAndSlavesForCaravanUtility.IsFollowingAnyone(x);
		}
	}
}
