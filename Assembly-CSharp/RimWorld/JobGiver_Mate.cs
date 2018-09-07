using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_Mate : ThinkNode_JobGiver
	{
		public JobGiver_Mate()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.gender != Gender.Male || !pawn.ageTracker.CurLifeStage.reproductive)
			{
				return null;
			}
			Predicate<Thing> validator = delegate(Thing t)
			{
				Pawn pawn3 = t as Pawn;
				return !pawn3.Downed && pawn3.CanCasuallyInteractNow(false) && !pawn3.IsForbidden(pawn) && pawn3.Faction == pawn.Faction && PawnUtility.FertileMateTarget(pawn, pawn3);
			};
			Pawn pawn2 = (Pawn)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(pawn.def), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 30f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
			if (pawn2 == null)
			{
				return null;
			}
			return new Job(JobDefOf.Mate, pawn2);
		}

		[CompilerGenerated]
		private sealed class <TryGiveJob>c__AnonStorey0
		{
			internal Pawn pawn;

			public <TryGiveJob>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing t)
			{
				Pawn pawn = t as Pawn;
				return !pawn.Downed && pawn.CanCasuallyInteractNow(false) && !pawn.IsForbidden(this.pawn) && pawn.Faction == this.pawn.Faction && PawnUtility.FertileMateTarget(this.pawn, pawn);
			}
		}
	}
}
