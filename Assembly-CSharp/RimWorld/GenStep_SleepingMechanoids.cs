using RimWorld.Planet;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class GenStep_SleepingMechanoids : GenStep
	{
		public FloatRange pointsRange = new FloatRange(340f, 1000f);

		public override void Generate(Map map)
		{
			CellRect around = default(CellRect);
			IntVec3 near = default(IntVec3);
			if (SiteGenStepUtility.TryFindRootToSpawnAroundRectOfInterest(out around, out near, map))
			{
				List<Pawn> list = new List<Pawn>();
				foreach (Pawn item in this.GeneratePawns(map))
				{
					IntVec3 loc = default(IntVec3);
					if (!SiteGenStepUtility.TryFindSpawnCellAroundOrNear(around, near, map, out loc))
					{
						Find.WorldPawns.PassToWorld(item, PawnDiscardDecideMode.Discard);
						break;
					}
					GenSpawn.Spawn(item, loc, map);
					list.Add(item);
				}
				if (list.Any())
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
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
			pawnGroupMakerParms.tile = map.Tile;
			pawnGroupMakerParms.faction = Faction.OfMechanoids;
			pawnGroupMakerParms.points = this.pointsRange.RandomInRange;
			return PawnGroupMakerUtility.GeneratePawns(PawnGroupKindDefOf.Normal, pawnGroupMakerParms, true);
		}
	}
}
