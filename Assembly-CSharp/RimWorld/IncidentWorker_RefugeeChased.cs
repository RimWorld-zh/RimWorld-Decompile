using System;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_RefugeeChased : IncidentWorker
	{
		private static readonly IntRange RaidDelay = new IntRange(1000, 2500);

		private const float RaidPointsFactor = 1.35f;

		private const float RelationWithColonistWeight = 20f;

		[CompilerGenerated]
		private static Func<Faction, bool> <>f__am$cache0;

		public IncidentWorker_RefugeeChased()
		{
		}

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

		private bool TryFindSpawnSpot(Map map, out IntVec3 spawnSpot)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Neutral, out spawnSpot);
		}

		private bool TryFindEnemyFaction(out Faction enemyFac)
		{
			return (from f in Find.FactionManager.AllFactions
			where !f.def.hidden && !f.defeated && f.HostileTo(Faction.OfPlayer)
			select f).TryRandomElement(out enemyFac);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static IncidentWorker_RefugeeChased()
		{
		}

		[CompilerGenerated]
		private static bool <TryFindEnemyFaction>m__0(Faction f)
		{
			return !f.def.hidden && !f.defeated && f.HostileTo(Faction.OfPlayer);
		}

		[CompilerGenerated]
		private sealed class <TryExecuteWorker>c__AnonStorey0
		{
			internal Pawn refugee;

			internal IntVec3 spawnSpot;

			internal Map map;

			internal Faction enemyFac;

			public <TryExecuteWorker>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				GenSpawn.Spawn(this.refugee, this.spawnSpot, this.map, WipeMode.Vanish);
				this.refugee.SetFaction(Faction.OfPlayer, null);
				CameraJumper.TryJump(this.refugee);
				IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, this.map);
				incidentParms.forced = true;
				incidentParms.faction = this.enemyFac;
				incidentParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
				incidentParms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
				incidentParms.spawnCenter = this.spawnSpot;
				incidentParms.points *= 1.35f;
				QueuedIncident qi = new QueuedIncident(new FiringIncident(IncidentDefOf.RaidEnemy, null, incidentParms), Find.TickManager.TicksGame + IncidentWorker_RefugeeChased.RaidDelay.RandomInRange);
				Find.Storyteller.incidentQueue.Add(qi);
			}

			internal void <>m__1()
			{
				Find.WorldPawns.PassToWorld(this.refugee, PawnDiscardDecideMode.Decide);
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindSpawnSpot>c__AnonStorey1
		{
			internal Map map;

			public <TryFindSpawnSpot>c__AnonStorey1()
			{
			}

			internal bool <>m__0(IntVec3 c)
			{
				return this.map.reachability.CanReachColony(c);
			}
		}
	}
}
