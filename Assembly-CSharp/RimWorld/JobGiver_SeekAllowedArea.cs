using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000EE RID: 238
	public class JobGiver_SeekAllowedArea : ThinkNode_JobGiver
	{
		// Token: 0x06000510 RID: 1296 RVA: 0x000382E8 File Offset: 0x000366E8
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (!pawn.Position.IsForbidden(pawn))
			{
				result = null;
			}
			else if (this.HasJobWithSpawnedAllowedTarget(pawn))
			{
				result = null;
			}
			else
			{
				Region region = pawn.GetRegion(RegionType.Set_Passable);
				if (region == null)
				{
					result = null;
				}
				else
				{
					TraverseParms traverseParms = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
					RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParms, false);
					Region reg = null;
					RegionProcessor regionProcessor = delegate(Region r)
					{
						bool result2;
						if (r.portal != null)
						{
							result2 = false;
						}
						else if (!r.IsForbiddenEntirely(pawn))
						{
							reg = r;
							result2 = true;
						}
						else
						{
							result2 = false;
						}
						return result2;
					};
					RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, 9999, RegionType.Set_Passable);
					if (reg != null)
					{
						IntVec3 c;
						if (!reg.TryFindRandomCellInRegionUnforbidden(pawn, null, out c))
						{
							result = null;
						}
						else
						{
							result = new Job(JobDefOf.Goto, c);
						}
					}
					else
					{
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x000383E8 File Offset: 0x000367E8
		private bool HasJobWithSpawnedAllowedTarget(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			return curJob != null && (this.IsSpawnedAllowedTarget(curJob.targetA, pawn) || this.IsSpawnedAllowedTarget(curJob.targetB, pawn) || this.IsSpawnedAllowedTarget(curJob.targetC, pawn));
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00038448 File Offset: 0x00036848
		private bool IsSpawnedAllowedTarget(LocalTargetInfo target, Pawn pawn)
		{
			bool result;
			if (!target.IsValid)
			{
				result = false;
			}
			else if (target.HasThing)
			{
				result = (target.Thing.Spawned && !target.Thing.Position.IsForbidden(pawn));
			}
			else
			{
				result = (target.Cell.InBounds(pawn.Map) && !target.Cell.IsForbidden(pawn));
			}
			return result;
		}
	}
}
