using RimWorld.Planet;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public abstract class IncidentWorker_Ambush : IncidentWorker
	{
		protected abstract List<Pawn> GeneratePawns(IncidentParms parms);

		protected virtual void PostProcessGeneratedPawnsAfterSpawning(List<Pawn> generatedPawns)
		{
		}

		protected virtual LordJob CreateLordJob(List<Pawn> generatedPawns, IncidentParms parms)
		{
			return null;
		}

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			return target is Map || CaravanIncidentUtility.CanFireIncidentWhichWantsToGenerateMapAt(target.Tile);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			bool result;
			if (parms.target is Map)
			{
				result = this.DoExecute(parms);
			}
			else
			{
				LongEventHandler.QueueLongEvent((Action)delegate()
				{
					this.DoExecute(parms);
				}, "GeneratingMapForNewEncounter", false, null);
				result = true;
			}
			return result;
		}

		private bool DoExecute(IncidentParms parms)
		{
			Map map = parms.target as Map;
			IntVec3 invalid = IntVec3.Invalid;
			bool result;
			if (map != null && !CellFinder.TryFindRandomEdgeCellWith((Predicate<IntVec3>)((IntVec3 x) => x.Standable(map) && map.reachability.CanReachColony(x)), map, CellFinder.EdgeRoadChance_Hostile, out invalid))
			{
				result = false;
			}
			else
			{
				List<Pawn> list = this.GeneratePawns(parms);
				if (!list.Any())
				{
					result = false;
				}
				else
				{
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
					LordJob lordJob = this.CreateLordJob(list, parms);
					if (lordJob != null)
					{
						LordMaker.MakeNewLord(parms.faction, lordJob, map, list);
					}
					this.SendAmbushLetter(list[0], parms);
					if (flag)
					{
						Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
					}
					result = true;
				}
			}
			return result;
		}

		protected abstract void SendAmbushLetter(Pawn anyPawn, IncidentParms parms);
	}
}
