using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_QuestBanditCamp : IncidentWorker
	{
		public IncidentWorker_QuestBanditCamp()
		{
		}

		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Faction faction;
			Faction faction2;
			int num;
			return base.CanFireNowSub(parms) && this.TryFindFactions(out faction, out faction2) && TileFinder.TryFindNewSiteTile(out num, 7, 27, false, true, -1);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Faction faction;
			Faction faction2;
			bool result;
			int tile;
			if (!this.TryFindFactions(out faction, out faction2))
			{
				result = false;
			}
			else if (!TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, true, -1))
			{
				result = false;
			}
			else
			{
				Site site = SiteMaker.MakeSite(SiteCoreDefOf.Nothing, SitePartDefOf.Outpost, tile, faction2, true);
				site.sitePartsKnown = true;
				List<Thing> list = this.GenerateRewards(faction);
				site.GetComponent<DefeatAllEnemiesQuestComp>().StartQuest(faction, 10, list);
				Find.WorldObjects.Add(site);
				base.SendStandardLetter(site, faction, new string[]
				{
					faction.leader.LabelShort,
					faction.def.leaderTitle,
					faction.Name,
					list[0].LabelCap
				});
				result = true;
			}
			return result;
		}

		private List<Thing> GenerateRewards(Faction alliedFaction)
		{
			return ThingSetMakerDefOf.Reward_StandardByDropPod.root.Generate();
		}

		private bool TryFindFactions(out Faction alliedFaction, out Faction enemyFaction)
		{
			bool result;
			if ((from x in Find.FactionManager.AllFactions
			where !x.def.hidden && !x.defeated && !x.IsPlayer && !x.HostileTo(Faction.OfPlayer) && this.CommonHumanlikeEnemyFactionExists(Faction.OfPlayer, x) && !this.AnyQuestExistsFrom(x)
			select x).TryRandomElement(out alliedFaction))
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
			for (int i = 0; i < sites.Count; i++)
			{
				DefeatAllEnemiesQuestComp component = sites[i].GetComponent<DefeatAllEnemiesQuestComp>();
				if (component != null && component.Active && component.requestingFaction == faction)
				{
					return true;
				}
			}
			return false;
		}

		private bool CommonHumanlikeEnemyFactionExists(Faction f1, Faction f2)
		{
			return this.CommonHumanlikeEnemyFaction(f1, f2) != null;
		}

		private Faction CommonHumanlikeEnemyFaction(Faction f1, Faction f2)
		{
			Faction faction;
			Faction result;
			if ((from x in Find.FactionManager.AllFactions
			where x != f1 && x != f2 && !x.def.hidden && x.def.humanlikeFaction && !x.defeated && x.HostileTo(f1) && x.HostileTo(f2)
			select x).TryRandomElement(out faction))
			{
				result = faction;
			}
			else
			{
				result = null;
			}
			return result;
		}

		[CompilerGenerated]
		private bool <TryFindFactions>m__0(Faction x)
		{
			return !x.def.hidden && !x.defeated && !x.IsPlayer && !x.HostileTo(Faction.OfPlayer) && this.CommonHumanlikeEnemyFactionExists(Faction.OfPlayer, x) && !this.AnyQuestExistsFrom(x);
		}

		[CompilerGenerated]
		private sealed class <CommonHumanlikeEnemyFaction>c__AnonStorey0
		{
			internal Faction f1;

			internal Faction f2;

			public <CommonHumanlikeEnemyFaction>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Faction x)
			{
				return x != this.f1 && x != this.f2 && !x.def.hidden && x.def.humanlikeFaction && !x.defeated && x.HostileTo(this.f1) && x.HostileTo(this.f2);
			}
		}
	}
}
