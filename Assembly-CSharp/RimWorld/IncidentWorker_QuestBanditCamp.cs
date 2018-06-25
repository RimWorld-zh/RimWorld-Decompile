using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000350 RID: 848
	public class IncidentWorker_QuestBanditCamp : IncidentWorker
	{
		// Token: 0x06000E9B RID: 3739 RVA: 0x0007BC04 File Offset: 0x0007A004
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Faction faction;
			Faction faction2;
			int num;
			return base.CanFireNowSub(parms) && this.TryFindFactions(out faction, out faction2) && TileFinder.TryFindNewSiteTile(out num, 7, 27, false, true, -1);
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x0007BC44 File Offset: 0x0007A044
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
				Site site = SiteMaker.MakeSite(SiteCoreDefOf.Nothing, SitePartDefOf.Outpost, faction2, true);
				site.Tile = tile;
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

		// Token: 0x06000E9D RID: 3741 RVA: 0x0007BD14 File Offset: 0x0007A114
		private List<Thing> GenerateRewards(Faction alliedFaction)
		{
			return ThingSetMakerDefOf.Reward_StandardByDropPod.root.Generate();
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x0007BD38 File Offset: 0x0007A138
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

		// Token: 0x06000E9F RID: 3743 RVA: 0x0007BD94 File Offset: 0x0007A194
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

		// Token: 0x06000EA0 RID: 3744 RVA: 0x0007BE00 File Offset: 0x0007A200
		private bool CommonHumanlikeEnemyFactionExists(Faction f1, Faction f2)
		{
			return this.CommonHumanlikeEnemyFaction(f1, f2) != null;
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x0007BE24 File Offset: 0x0007A224
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
	}
}
