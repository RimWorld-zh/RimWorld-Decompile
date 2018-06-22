using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000354 RID: 852
	public class IncidentWorker_QuestPeaceTalks : IncidentWorker
	{
		// Token: 0x06000EB9 RID: 3769 RVA: 0x0007C89C File Offset: 0x0007AC9C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Faction faction;
			int num;
			return base.CanFireNowSub(parms) && this.TryFindFaction(out faction) && this.TryFindTile(out num);
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x0007C8DC File Offset: 0x0007ACDC
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

		// Token: 0x06000EBB RID: 3771 RVA: 0x0007C9D0 File Offset: 0x0007ADD0
		private bool TryFindFaction(out Faction faction)
		{
			return (from x in Find.FactionManager.AllFactions
			where !x.def.hidden && !x.def.permanentEnemy && !x.IsPlayer && x.HostileTo(Faction.OfPlayer) && !x.defeated && !SettlementUtility.IsPlayerAttackingAnySettlementOf(x) && !this.PeaceTalksExist(x) && x.leader != null && !x.leader.IsPrisoner && !x.leader.Spawned
			select x).TryRandomElement(out faction);
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x0007CA08 File Offset: 0x0007AE08
		private bool TryFindTile(out int tile)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 5, 13, false, false, -1);
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x0007CA2C File Offset: 0x0007AE2C
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

		// Token: 0x0400090B RID: 2315
		private const int MinDistance = 5;

		// Token: 0x0400090C RID: 2316
		private const int MaxDistance = 13;

		// Token: 0x0400090D RID: 2317
		private static readonly IntRange TimeoutDaysRange = new IntRange(21, 23);
	}
}
