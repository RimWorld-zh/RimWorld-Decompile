using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public abstract class IncidentWorker_Ambush : IncidentWorker
	{
		protected IncidentWorker_Ambush()
		{
		}

		protected abstract List<Pawn> GeneratePawns(IncidentParms parms);

		protected virtual void PostProcessGeneratedPawnsAfterSpawning(List<Pawn> generatedPawns)
		{
		}

		protected virtual LordJob CreateLordJob(List<Pawn> generatedPawns, IncidentParms parms)
		{
			return null;
		}

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

		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => x.Standable(map) && map.reachability.CanReachColony(x), map, CellFinder.EdgeRoadChance_Hostile, out cell);
		}

		protected virtual string GetLetterLabel(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterLabel;
		}

		protected virtual string GetLetterText(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterText;
		}

		protected virtual LetterDef GetLetterDef(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterDef;
		}

		protected virtual string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsGroupGeneric".Translate(new object[]
			{
				Faction.OfPlayer.def.pawnsPlural
			});
		}

		[CompilerGenerated]
		private sealed class <TryExecuteWorker>c__AnonStorey0
		{
			internal IncidentParms parms;

			internal List<Pawn> generatedEnemies;

			internal IntVec3 existingMapEdgeCell;

			internal IncidentWorker_Ambush $this;

			public <TryExecuteWorker>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				this.$this.DoExecute(this.parms, this.generatedEnemies, this.existingMapEdgeCell);
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindEntryCell>c__AnonStorey1
		{
			internal Map map;

			public <TryFindEntryCell>c__AnonStorey1()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return x.Standable(this.map) && this.map.reachability.CanReachColony(x);
			}
		}
	}
}
