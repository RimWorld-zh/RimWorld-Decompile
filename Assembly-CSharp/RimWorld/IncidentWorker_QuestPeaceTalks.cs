using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_QuestPeaceTalks : IncidentWorker
	{
		private const int MinDistance = 5;

		private const int MaxDistance = 13;

		private static readonly IntRange TimeoutDaysRange = new IntRange(21, 23);

		public IncidentWorker_QuestPeaceTalks()
		{
		}

		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Faction faction;
			int num;
			return base.CanFireNowSub(parms) && this.TryFindFaction(out faction) && this.TryFindTile(out num);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Faction faction;
			bool result;
			int tile;
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
				int randomInRange = IncidentWorker_QuestPeaceTalks.TimeoutDaysRange.RandomInRange;
				peaceTalks.GetComponent<TimeoutComp>().StartTimeout(randomInRange * 60000);
				Find.WorldObjects.Add(peaceTalks);
				string text = string.Format(this.def.letterText.AdjustedFor(faction.leader, "PAWN"), faction.def.leaderTitle, faction.Name, randomInRange).CapitalizeFirst();
				Find.LetterStack.ReceiveLetter(this.def.letterLabel, text, this.def.letterDef, peaceTalks, faction, null);
				result = true;
			}
			return result;
		}

		private bool TryFindFaction(out Faction faction)
		{
			return (from x in Find.FactionManager.AllFactions
			where !x.def.hidden && !x.def.permanentEnemy && !x.IsPlayer && x.HostileTo(Faction.OfPlayer) && !x.defeated && !SettlementUtility.IsPlayerAttackingAnySettlementOf(x) && !this.PeaceTalksExist(x) && x.leader != null && !x.leader.IsPrisoner && !x.leader.Spawned
			select x).TryRandomElement(out faction);
		}

		private bool TryFindTile(out int tile)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 5, 13, false, false, -1);
		}

		private bool PeaceTalksExist(Faction faction)
		{
			List<PeaceTalks> peaceTalks = Find.WorldObjects.PeaceTalks;
			for (int i = 0; i < peaceTalks.Count; i++)
			{
				if (peaceTalks[i].Faction == faction)
				{
					return true;
				}
			}
			return false;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static IncidentWorker_QuestPeaceTalks()
		{
		}

		[CompilerGenerated]
		private bool <TryFindFaction>m__0(Faction x)
		{
			return !x.def.hidden && !x.def.permanentEnemy && !x.IsPlayer && x.HostileTo(Faction.OfPlayer) && !x.defeated && !SettlementUtility.IsPlayerAttackingAnySettlementOf(x) && !this.PeaceTalksExist(x) && x.leader != null && !x.leader.IsPrisoner && !x.leader.Spawned;
		}
	}
}
