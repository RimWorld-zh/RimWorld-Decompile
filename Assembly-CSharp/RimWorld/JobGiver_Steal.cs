using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_Steal : ThinkNode_JobGiver
	{
		public const float ItemsSearchRadiusInitial = 7f;

		private const float ItemsSearchRadiusOngoing = 12f;

		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c = default(IntVec3);
			Job result;
			Thing thing = default(Thing);
			if (!RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn))
			{
				result = null;
			}
			else if (StealAIUtility.TryFindBestItemToSteal(pawn.Position, pawn.Map, 12f, out thing, pawn, (List<Thing>)null) && !GenAI.InDangerousCombat(pawn))
			{
				Job job = new Job(JobDefOf.Steal);
				job.targetA = thing;
				job.targetB = c;
				job.count = Mathf.Min(thing.stackCount, (int)(pawn.GetStatValue(StatDefOf.CarryingCapacity, true) / thing.def.VolumePerUnit));
				result = job;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
