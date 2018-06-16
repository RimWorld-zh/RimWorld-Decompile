using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200031B RID: 795
	public abstract class IncidentWorker_Ambush : IncidentWorker
	{
		// Token: 0x06000D81 RID: 3457
		protected abstract List<Pawn> GeneratePawns(IncidentParms parms);

		// Token: 0x06000D82 RID: 3458 RVA: 0x00073C43 File Offset: 0x00072043
		protected virtual void PostProcessGeneratedPawnsAfterSpawning(List<Pawn> generatedPawns)
		{
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x00073C48 File Offset: 0x00072048
		protected virtual LordJob CreateLordJob(List<Pawn> generatedPawns, IncidentParms parms)
		{
			return null;
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x00073C60 File Offset: 0x00072060
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = parms.target as Map;
			bool result;
			if (map != null)
			{
				IntVec3 intVec;
				result = this.TryFindEntryCell(map, out intVec);
			}
			else
			{
				result = CaravanIncidentUtility.CanFireIncidentWhichWantsToGenerateMapAt(parms.target.Tile);
			}
			return result;
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x00073CA8 File Offset: 0x000720A8
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = parms.target as Map;
			IntVec3 existingMapEdgeCell = IntVec3.Invalid;
			if (map != null)
			{
				if (!this.TryFindEntryCell(map, out existingMapEdgeCell))
				{
					return false;
				}
			}
			List<Pawn> generatedEnemies = this.GeneratePawns(parms);
			bool result;
			if (!generatedEnemies.Any<Pawn>())
			{
				result = false;
			}
			else if (map != null)
			{
				result = this.DoExecute(parms, generatedEnemies, existingMapEdgeCell);
			}
			else
			{
				LongEventHandler.QueueLongEvent(delegate()
				{
					this.DoExecute(parms, generatedEnemies, existingMapEdgeCell);
				}, "GeneratingMapForNewEncounter", false, null);
				result = true;
			}
			return result;
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00073D74 File Offset: 0x00072174
		private bool DoExecute(IncidentParms parms, List<Pawn> generatedEnemies, IntVec3 existingMapEdgeCell)
		{
			Map map = parms.target as Map;
			bool flag = false;
			if (map == null)
			{
				map = CaravanIncidentUtility.SetupCaravanAttackMap((Caravan)parms.target, generatedEnemies, false);
				flag = true;
			}
			else
			{
				for (int i = 0; i < generatedEnemies.Count; i++)
				{
					IntVec3 loc = CellFinder.RandomSpawnCellForPawnNear(existingMapEdgeCell, map, 4);
					GenSpawn.Spawn(generatedEnemies[i], loc, map, Rot4.Random, WipeMode.Vanish, false);
				}
			}
			this.PostProcessGeneratedPawnsAfterSpawning(generatedEnemies);
			LordJob lordJob = this.CreateLordJob(generatedEnemies, parms);
			if (lordJob != null)
			{
				LordMaker.MakeNewLord(parms.faction, lordJob, map, generatedEnemies);
			}
			string letterLabel = this.GetLetterLabel(generatedEnemies[0], parms);
			string letterText = this.GetLetterText(generatedEnemies[0], parms);
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(generatedEnemies, ref letterLabel, ref letterText, this.GetRelatedPawnsInfoLetterText(parms), true, true);
			Find.LetterStack.ReceiveLetter(letterLabel, letterText, this.GetLetterDef(generatedEnemies[0], parms), generatedEnemies[0], parms.faction, null);
			if (flag)
			{
				Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
			}
			return true;
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x00073E94 File Offset: 0x00072294
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => x.Standable(map) && map.reachability.CanReachColony(x), map, CellFinder.EdgeRoadChance_Hostile, out cell);
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x00073ED4 File Offset: 0x000722D4
		protected virtual string GetLetterLabel(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterLabel;
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x00073EF4 File Offset: 0x000722F4
		protected virtual string GetLetterText(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterText;
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x00073F14 File Offset: 0x00072314
		protected virtual LetterDef GetLetterDef(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterDef;
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x00073F34 File Offset: 0x00072334
		protected virtual string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsGroupGeneric".Translate(new object[]
			{
				Faction.OfPlayer.def.pawnsPlural
			});
		}
	}
}
