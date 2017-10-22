using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_QuestBanditCamp : IncidentWorker
	{
		private const float RelationsImprovement = 8f;

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			Faction faction = default(Faction);
			Faction faction2 = default(Faction);
			int num = default(int);
			return base.CanFireNowSub(target) && this.TryFindFactions(out faction, out faction2) && TileFinder.TryFindNewSiteTile(out num, 8, 30, false, true, -1);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Faction faction = default(Faction);
			Faction faction2 = default(Faction);
			bool result;
			int tile = default(int);
			if (!this.TryFindFactions(out faction, out faction2))
			{
				result = false;
			}
			else if (!TileFinder.TryFindNewSiteTile(out tile, 8, 30, false, true, -1))
			{
				result = false;
			}
			else
			{
				Site site = SiteMaker.MakeSite(SiteCoreDefOf.Nothing, SitePartDefOf.Outpost, faction2);
				site.Tile = tile;
				List<Thing> list = this.GenerateRewards(faction);
				((WorldObject)site).GetComponent<DefeatAllEnemiesQuestComp>().StartQuest(faction, 8f, list);
				Find.WorldObjects.Add(site);
				base.SendStandardLetter((WorldObject)site, faction.leader.LabelShort, faction.def.leaderTitle, faction.Name, list[0].LabelCap);
				result = true;
			}
			return result;
		}

		private List<Thing> GenerateRewards(Faction alliedFaction)
		{
			ItemCollectionGeneratorParams parms = new ItemCollectionGeneratorParams
			{
				techLevel = new TechLevel?(alliedFaction.def.techLevel)
			};
			return ItemCollectionGeneratorDefOf.BanditCampQuestRewards.Worker.Generate(parms);
		}

		private bool TryFindFactions(out Faction alliedFaction, out Faction enemyFaction)
		{
			bool result;
			if ((from x in Find.FactionManager.AllFactions
			where !x.def.hidden && !x.defeated && !x.IsPlayer && !x.HostileTo(Faction.OfPlayer) && this.CommonHumanlikeEnemyFactionExists(Faction.OfPlayer, x) && !this.AnyQuestExistsFrom(x)
			select x).TryRandomElement<Faction>(out alliedFaction))
			{
				enemyFaction = this.CommonHumanlikeEnemyFaction(Faction.OfPlayer, alliedFaction);
				result = true;
			}
			else
			{
				alliedFaction = null;
				enemyFaction = null;
				result = false;
			}
			return result;
		}

		private bool AnyQuestExistsFrom(Faction faction)
		{
			List<Site> sites = Find.WorldObjects.Sites;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < sites.Count)
				{
					DefeatAllEnemiesQuestComp component = ((WorldObject)sites[num]).GetComponent<DefeatAllEnemiesQuestComp>();
					if (component != null && component.Active && component.requestingFaction == faction)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		private bool CommonHumanlikeEnemyFactionExists(Faction f1, Faction f2)
		{
			return this.CommonHumanlikeEnemyFaction(f1, f2) != null;
		}

		private Faction CommonHumanlikeEnemyFaction(Faction f1, Faction f2)
		{
			Faction faction = default(Faction);
			return (!(from x in Find.FactionManager.AllFactions
			where x != f1 && x != f2 && !x.def.hidden && x.def.humanlikeFaction && !x.defeated && x.HostileTo(f1) && x.HostileTo(f2)
			select x).TryRandomElement<Faction>(out faction)) ? null : faction;
		}
	}
}
