using System;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200033D RID: 829
	public class IncidentWorker_RefugeeChased : IncidentWorker
	{
		// Token: 0x040008E3 RID: 2275
		private static readonly IntRange RaidDelay = new IntRange(1000, 2500);

		// Token: 0x040008E4 RID: 2276
		private const float RaidPointsFactor = 1.35f;

		// Token: 0x040008E5 RID: 2277
		private const float RelationWithColonistWeight = 20f;

		// Token: 0x06000E24 RID: 3620 RVA: 0x00078544 File Offset: 0x00076944
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			bool result;
			if (!base.CanFireNowSub(parms))
			{
				result = false;
			}
			else
			{
				Map map = (Map)parms.target;
				IntVec3 intVec;
				Faction faction;
				result = (this.TryFindSpawnSpot(map, out intVec) && this.TryFindEnemyFaction(out faction));
			}
			return result;
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x00078594 File Offset: 0x00076994
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 spawnSpot;
			Faction enemyFac;
			bool result;
			if (!this.TryFindSpawnSpot(map, out spawnSpot))
			{
				result = false;
			}
			else if (!this.TryFindEnemyFaction(out enemyFac))
			{
				result = false;
			}
			else
			{
				PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, null, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 20f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
				Pawn refugee = PawnGenerator.GeneratePawn(request);
				refugee.relations.everSeenByPlayer = true;
				string text = "RefugeeChasedInitial".Translate(new object[]
				{
					refugee.Name.ToStringFull,
					refugee.story.Title,
					enemyFac.def.pawnsPlural,
					enemyFac.Name,
					refugee.ageTracker.AgeBiologicalYears
				});
				text = text.AdjustedFor(refugee, "PAWN");
				PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, refugee);
				DiaNode diaNode = new DiaNode(text);
				DiaOption diaOption = new DiaOption("RefugeeChasedInitial_Accept".Translate());
				diaOption.action = delegate()
				{
					GenSpawn.Spawn(refugee, spawnSpot, map, WipeMode.Vanish);
					refugee.SetFaction(Faction.OfPlayer, null);
					CameraJumper.TryJump(refugee);
					IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, map);
					incidentParms.forced = true;
					incidentParms.faction = enemyFac;
					incidentParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
					incidentParms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
					incidentParms.spawnCenter = spawnSpot;
					incidentParms.points *= 1.35f;
					QueuedIncident qi = new QueuedIncident(new FiringIncident(IncidentDefOf.RaidEnemy, null, incidentParms), Find.TickManager.TicksGame + IncidentWorker_RefugeeChased.RaidDelay.RandomInRange);
					Find.Storyteller.incidentQueue.Add(qi);
				};
				diaOption.resolveTree = true;
				diaNode.options.Add(diaOption);
				string text2 = "RefugeeChasedRejected".Translate(new object[]
				{
					refugee.LabelShort
				});
				DiaNode diaNode2 = new DiaNode(text2);
				DiaOption diaOption2 = new DiaOption("OK".Translate());
				diaOption2.resolveTree = true;
				diaNode2.options.Add(diaOption2);
				DiaOption diaOption3 = new DiaOption("RefugeeChasedInitial_Reject".Translate());
				diaOption3.action = delegate()
				{
					Find.WorldPawns.PassToWorld(refugee, PawnDiscardDecideMode.Decide);
				};
				diaOption3.link = diaNode2;
				diaNode.options.Add(diaOption3);
				string title = "RefugeeChasedTitle".Translate(new object[]
				{
					map.Parent.Label
				});
				Find.WindowStack.Add(new Dialog_NodeTreeWithFactionInfo(diaNode, enemyFac, true, true, title));
				Find.Archive.Add(new ArchivedDialog(diaNode.text, title, enemyFac));
				result = true;
			}
			return result;
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x00078828 File Offset: 0x00076C28
		private bool TryFindSpawnSpot(Map map, out IntVec3 spawnSpot)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Neutral, out spawnSpot);
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x00078868 File Offset: 0x00076C68
		private bool TryFindEnemyFaction(out Faction enemyFac)
		{
			return (from f in Find.FactionManager.AllFactions
			where !f.def.hidden && !f.defeated && f.HostileTo(Faction.OfPlayer)
			select f).TryRandomElement(out enemyFac);
		}
	}
}
