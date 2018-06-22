using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000D9 RID: 217
	public class JobGiver_Steal : ThinkNode_JobGiver
	{
		// Token: 0x060004C9 RID: 1225 RVA: 0x000358CC File Offset: 0x00033CCC
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

		// Token: 0x040002A8 RID: 680
		public const float ItemsSearchRadiusInitial = 7f;

		// Token: 0x040002A9 RID: 681
		private const float ItemsSearchRadiusOngoing = 12f;
	}
}
