using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	internal class JobGiver_FightFiresNearPoint : ThinkNode_JobGiver
	{
		public float maxDistFromPoint = -1f;

		public JobGiver_FightFiresNearPoint()
		{
		}

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_FightFiresNearPoint jobGiver_FightFiresNearPoint = (JobGiver_FightFiresNearPoint)base.DeepCopy(resolve);
			jobGiver_FightFiresNearPoint.maxDistFromPoint = this.maxDistFromPoint;
			return jobGiver_FightFiresNearPoint;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> validator = delegate(Thing t)
			{
				Pawn pawn2 = ((AttachableThing)t).parent as Pawn;
				return pawn2 == null && pawn.CanReserve(t, 1, -1, null, false) && !pawn.story.WorkTagIsDisabled(WorkTags.Firefighting);
			};
			Thing thing = GenClosest.ClosestThingReachable(pawn.GetLord().CurLordToil.FlagLoc, pawn.Map, ThingRequest.ForDef(ThingDefOf.Fire), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), this.maxDistFromPoint, validator, null, 0, -1, false, RegionType.Set_Passable, false);
			Job result;
			if (thing != null)
			{
				result = new Job(JobDefOf.BeatFire, thing);
			}
			else
			{
				result = null;
			}
			return result;
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
				Pawn pawn = ((AttachableThing)t).parent as Pawn;
				return pawn == null && this.pawn.CanReserve(t, 1, -1, null, false) && !this.pawn.story.WorkTagIsDisabled(WorkTags.Firefighting);
			}
		}
	}
}
