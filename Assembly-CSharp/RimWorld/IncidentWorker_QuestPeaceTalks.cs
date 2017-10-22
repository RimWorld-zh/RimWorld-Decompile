using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_QuestPeaceTalks : IncidentWorker
	{
		private const int MinDistance = 5;

		private const int MaxDistance = 15;

		private const int TimeoutDays = 15;

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			int num = default(int);
			Faction faction = default(Faction);
			return base.CanFireNowSub(target) && this.TryFindFaction(out faction) && this.TryFindTile(out num);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Faction faction = default(Faction);
			bool result;
			int tile = default(int);
			if (!this.TryFindFaction(out faction))
			{
				result = false;
			}
			else if (!this.TryFindTile(out tile))
			{
				result = false;
			}
			else
			{
				PeaceTalks peaceTalks = (PeaceTalks)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.PeaceTalks);
				peaceTalks.Tile = tile;
				peaceTalks.SetFaction(faction);
				((WorldObject)peaceTalks).GetComponent<TimeoutComp>().StartTimeout(900000);
				Find.WorldObjects.Add(peaceTalks);
				string text = string.Format(base.def.letterText.AdjustedFor(faction.leader), faction.def.leaderTitle, faction.Name, 15).CapitalizeFirst();
				Find.LetterStack.ReceiveLetter(base.def.letterLabel, text, base.def.letterDef, (WorldObject)peaceTalks, (string)null);
				result = true;
			}
			return result;
		}

		private bool TryFindFaction(out Faction faction)
		{
			return (from x in Find.FactionManager.AllFactions
			where !x.def.hidden && x.def.appreciative && !x.IsPlayer && x.HostileTo(Faction.OfPlayer) && !x.defeated && !SettlementUtility.IsPlayerAttackingAnySettlementOf(x) && !this.PeaceTalksExist(x)
			select x).TryRandomElement<Faction>(out faction);
		}

		private bool TryFindTile(out int tile)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 5, 15, false, false, -1);
		}

		private bool PeaceTalksExist(Faction faction)
		{
			List<PeaceTalks> peaceTalks = Find.WorldObjects.PeaceTalks;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < peaceTalks.Count)
				{
					if (peaceTalks[num].Faction == faction)
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
	}
}
