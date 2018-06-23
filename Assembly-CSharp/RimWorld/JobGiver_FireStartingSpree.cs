using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200010C RID: 268
	internal class JobGiver_FireStartingSpree : ThinkNode_JobGiver
	{
		// Token: 0x040002EF RID: 751
		private IntRange waitTicks = new IntRange(80, 140);

		// Token: 0x040002F0 RID: 752
		private const float FireStartChance = 0.75f;

		// Token: 0x040002F1 RID: 753
		private static List<Thing> potentialTargets = new List<Thing>();

		// Token: 0x0600058D RID: 1421 RVA: 0x0003C1F0 File Offset: 0x0003A5F0
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_FireStartingSpree jobGiver_FireStartingSpree = (JobGiver_FireStartingSpree)base.DeepCopy(resolve);
			jobGiver_FireStartingSpree.waitTicks = this.waitTicks;
			return jobGiver_FireStartingSpree;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0003C220 File Offset: 0x0003A620
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.mindState.nextMoveOrderIsWait)
			{
				Job job = new Job(JobDefOf.Wait_Wander);
				job.expiryInterval = this.waitTicks.RandomInRange;
				pawn.mindState.nextMoveOrderIsWait = false;
				result = job;
			}
			else
			{
				if (Rand.Value < 0.75f)
				{
					Thing thing = this.TryFindRandomIgniteTarget(pawn);
					if (thing != null)
					{
						pawn.mindState.nextMoveOrderIsWait = true;
						return new Job(JobDefOf.Ignite, thing);
					}
				}
				IntVec3 c = RCellFinder.RandomWanderDestFor(pawn, pawn.Position, 10f, null, Danger.Deadly);
				if (c.IsValid)
				{
					pawn.mindState.nextMoveOrderIsWait = true;
					result = new Job(JobDefOf.GotoWander, c);
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0003C2FC File Offset: 0x0003A6FC
		private Thing TryFindRandomIgniteTarget(Pawn pawn)
		{
			Region region;
			Thing result;
			if (!CellFinder.TryFindClosestRegionWith(pawn.GetRegion(RegionType.Set_Passable), TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), (Region candidateRegion) => !candidateRegion.IsForbiddenEntirely(pawn), 100, out region, RegionType.Set_Passable))
			{
				result = null;
			}
			else
			{
				JobGiver_FireStartingSpree.potentialTargets.Clear();
				List<Thing> allThings = region.ListerThings.AllThings;
				for (int i = 0; i < allThings.Count; i++)
				{
					Thing thing = allThings[i];
					if ((thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Plant) && thing.FlammableNow && !thing.IsBurning() && !thing.OccupiedRect().Contains(pawn.Position))
					{
						JobGiver_FireStartingSpree.potentialTargets.Add(thing);
					}
				}
				if (JobGiver_FireStartingSpree.potentialTargets.NullOrEmpty<Thing>())
				{
					result = null;
				}
				else
				{
					result = JobGiver_FireStartingSpree.potentialTargets.RandomElement<Thing>();
				}
			}
			return result;
		}
	}
}
