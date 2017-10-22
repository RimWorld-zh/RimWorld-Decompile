using RimWorld.Planet;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public abstract class IncidentWorker_Ambush : IncidentWorker
	{
		protected abstract List<Pawn> GeneratePawns(IIncidentTarget target, float points, int tile);

		protected virtual void PostProcessGeneratedPawnsAfterSpawning(List<Pawn> generatedPawns)
		{
		}

		protected virtual LordJob CreateLordJob(List<Pawn> generatedPawns, out Faction faction)
		{
			faction = null;
			return null;
		}

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			if (target is Map)
			{
				return true;
			}
			return CaravanIncidentUtility.CanFireIncidentWhichWantsToGenerateMapAt(target.Tile);
		}

		public override bool TryExecute(IncidentParms parms)
		{
			if (parms.target is Map)
			{
				return this.DoExecute(parms);
			}
			LongEventHandler.QueueLongEvent((Action)delegate()
			{
				this.DoExecute(parms);
			}, "GeneratingMapForNewEncounter", false, null);
			return true;
		}

		private bool DoExecute(IncidentParms parms)
		{
			Map map = parms.target as Map;
			IntVec3 invalid = IntVec3.Invalid;
			if (map != null && !CellFinder.TryFindRandomEdgeCellWith((Predicate<IntVec3>)((IntVec3 x) => x.Standable(map) && map.reachability.CanReachColony(x)), map, CellFinder.EdgeRoadChance_Hostile, out invalid))
			{
				return false;
			}
			List<Pawn> list = this.GeneratePawns(parms.target, parms.points, parms.target.Tile);
			bool flag = false;
			if (map == null)
			{
				map = CaravanIncidentUtility.SetupCaravanAttackMap((Caravan)parms.target, list);
				flag = true;
			}
			else
			{
				for (int i = 0; i < list.Count; i++)
				{
					IntVec3 loc = CellFinder.RandomSpawnCellForPawnNear(invalid, map, 4);
					GenSpawn.Spawn(list[i], loc, map, Rot4.Random, false);
				}
			}
			this.PostProcessGeneratedPawnsAfterSpawning(list);
			Faction faction = default(Faction);
			LordJob lordJob = this.CreateLordJob(list, out faction);
			if (lordJob != null && list.Any())
			{
				LordMaker.MakeNewLord(faction, lordJob, map, list);
			}
			this.SendAmbushLetter(list[0], faction);
			if (flag)
			{
				Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
			}
			return true;
		}

		protected abstract void SendAmbushLetter(Pawn anyPawn, Faction enemyFaction);
	}
}
