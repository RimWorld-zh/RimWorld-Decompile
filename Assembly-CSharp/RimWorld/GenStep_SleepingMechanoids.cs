using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class GenStep_SleepingMechanoids : GenStep
	{
		public FloatRange pointsRange;

		public GenStep_SleepingMechanoids()
		{
		}

		public override int SeedPart
		{
			get
			{
				return 341176078;
			}
		}

		public override void Generate(Map map, GenStepParams parms)
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
