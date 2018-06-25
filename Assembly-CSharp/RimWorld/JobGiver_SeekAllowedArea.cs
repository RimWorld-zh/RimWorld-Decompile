using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_SeekAllowedArea : ThinkNode_JobGiver
	{
		public JobGiver_SeekAllowedArea()
		{
		}

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

		private bool HasJobWithSpawnedAllowedTarget(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			return curJob != null && (this.IsSpawnedAllowedTarget(curJob.targetA, pawn) || this.IsSpawnedAllowedTarget(curJob.targetB, pawn) || this.IsSpawnedAllowedTarget(curJob.targetC, pawn));
		}

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

		[CompilerGenerated]
		private sealed class <TryGiveJob>c__AnonStorey0
		{
			internal TraverseParms traverseParms;

			internal Pawn pawn;

			internal Region reg;

			public <TryGiveJob>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Region from, Region r)
			{
				return r.Allows(this.traverseParms, false);
			}

			internal bool <>m__1(Region r)
			{
				bool result;
				if (r.portal != null)
				{
					result = false;
				}
				else if (!r.IsForbiddenEntirely(this.pawn))
				{
					this.reg = r;
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
			}
		}
	}
}
