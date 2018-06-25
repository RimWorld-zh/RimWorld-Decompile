using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_Steal : ThinkNode_JobGiver
	{
		public const float ItemsSearchRadiusInitial = 7f;

		private const float ItemsSearchRadiusOngoing = 12f;

		public JobGiver_Steal()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			Job result;
			Thing thing;
			if (!RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn))
			{
				result = null;
			}
			else if (StealAIUtility.TryFindBestItemToSteal(pawn.Position, pawn.Map, 12f, out thing, pawn, null) && !GenAI.InDangerousCombat(pawn))
			{
				result = new Job(JobDefOf.Steal)
				{
					targetA = thing,
					targetB = c,
					count = Mathf.Min(thing.stackCount, (int)(pawn.GetStatValue(StatDefOf.CarryingCapacity, true) / thing.def.VolumePerUnit))
				};
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
