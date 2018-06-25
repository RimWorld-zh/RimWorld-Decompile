using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200031D RID: 797
	public abstract class IncidentWorker_Ambush : IncidentWorker
	{
		// Token: 0x06000D85 RID: 3461
		protected abstract List<Pawn> GeneratePawns(IncidentParms parms);

		// Token: 0x06000D86 RID: 3462 RVA: 0x00073E47 File Offset: 0x00072247
		protected virtual void PostProcessGeneratedPawnsAfterSpawning(List<Pawn> generatedPawns)
		{
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x00073E4C File Offset: 0x0007224C
		protected virtual LordJob CreateLordJob(List<Pawn> generatedPawns, IncidentParms parms)
		{
			return null;
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x00073E64 File Offset: 0x00072264
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

		// Token: 0x06000D89 RID: 3465 RVA: 0x00073EAC File Offset: 0x000722AC
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

		// Token: 0x06000D8A RID: 3466 RVA: 0x00073F78 File Offset: 0x00072378
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

		// Token: 0x06000D8B RID: 3467 RVA: 0x00074098 File Offset: 0x00072498
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => x.Standable(map) && map.reachability.CanReachColony(x), map, CellFinder.EdgeRoadChance_Hostile, out cell);
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x000740D8 File Offset: 0x000724D8
		protected virtual string GetLetterLabel(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterLabel;
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x000740F8 File Offset: 0x000724F8
		protected virtual string GetLetterText(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterText;
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x00074118 File Offset: 0x00072518
		protected virtual LetterDef GetLetterDef(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterDef;
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00074138 File Offset: 0x00072538
		protected virtual string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsGroupGeneric".Translate(new object[]
			{
				Faction.OfPlayer.def.pawnsPlural
			});
		}
	}
}
