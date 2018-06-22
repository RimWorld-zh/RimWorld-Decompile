using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020000D0 RID: 208
	internal class JobGiver_FightFiresNearPoint : ThinkNode_JobGiver
	{
		// Token: 0x060004B2 RID: 1202 RVA: 0x00035084 File Offset: 0x00033484
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_FightFiresNearPoint jobGiver_FightFiresNearPoint = (JobGiver_FightFiresNearPoint)base.DeepCopy(resolve);
			jobGiver_FightFiresNearPoint.maxDistFromPoint = this.maxDistFromPoint;
			return jobGiver_FightFiresNearPoint;
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x000350B4 File Offset: 0x000334B4
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

		// Token: 0x0400029E RID: 670
		public float maxDistFromPoint = -1f;
	}
}
