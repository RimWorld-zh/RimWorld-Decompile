using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200040F RID: 1039
	public class GenStep_SleepingMechanoids : GenStep
	{
		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060011E3 RID: 4579 RVA: 0x0009B470 File Offset: 0x00099870
		public override int SeedPart
		{
			get
			{
				return 341176078;
			}
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x0009B48C File Offset: 0x0009988C
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

		// Token: 0x060011E5 RID: 4581 RVA: 0x0009B594 File Offset: 0x00099994
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

		// Token: 0x04000AE2 RID: 2786
		public FloatRange pointsRange;
	}
}
