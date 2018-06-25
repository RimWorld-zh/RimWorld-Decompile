using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000411 RID: 1041
	public class GenStep_SleepingMechanoids : GenStep
	{
		// Token: 0x04000AE2 RID: 2786
		public FloatRange pointsRange;

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060011E7 RID: 4583 RVA: 0x0009B5C0 File Offset: 0x000999C0
		public override int SeedPart
		{
			get
			{
				return 341176078;
			}
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x0009B5DC File Offset: 0x000999DC
		public override void Generate(Map map)
		{
			CellRect around;
			IntVec3 near;
			if (SiteGenStepUtility.TryFindRootToSpawnAroundRectOfInterest(out around, out near, map))
			{
				List<Pawn> list = new List<Pawn>();
				foreach (Pawn pawn in this.GeneratePawns(map))
				{
					IntVec3 loc;
					if (!SiteGenStepUtility.TryFindSpawnCellAroundOrNear(around, near, map, out loc))
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						break;
					}
					GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
					list.Add(pawn);
				}
				if (list.Any<Pawn>())
				{
					LordMaker.MakeNewLord(Faction.OfMechanoids, new LordJob_SleepThenAssaultColony(Faction.OfMechanoids, Rand.Bool), map, list);
					for (int i = 0; i < list.Count; i++)
					{
						list[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true);
					}
				}
			}
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x0009B6E4 File Offset: 0x00099AE4
		private IEnumerable<Pawn> GeneratePawns(Map map)
		{
			return PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Combat,
				tile = map.Tile,
				faction = Faction.OfMechanoids,
				points = this.pointsRange.RandomInRange
			}, true);
		}
	}
}
