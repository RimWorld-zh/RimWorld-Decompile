using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000565 RID: 1381
	public static class NamePlayerFactionAndBaseUtility
	{
		// Token: 0x04000F42 RID: 3906
		private const float MinDaysPassedToNameFaction = 3f;

		// Token: 0x04000F43 RID: 3907
		private const float MinDaysPassedToNameFactionBase = 3f;

		// Token: 0x04000F44 RID: 3908
		private const int SoonTicks = 30000;

		// Token: 0x06001A15 RID: 6677 RVA: 0x000E2654 File Offset: 0x000E0A54
		public static bool CanNameFactionNow()
		{
			return NamePlayerFactionAndBaseUtility.CanNameFaction(Find.TickManager.TicksGame);
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x000E2678 File Offset: 0x000E0A78
		public static bool CanNameFactionBaseNow(FactionBase factionBase)
		{
			return NamePlayerFactionAndBaseUtility.CanNameFactionBase(factionBase, Find.TickManager.TicksGame - factionBase.creationGameTicks);
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x000E26A4 File Offset: 0x000E0AA4
		public static bool CanNameFactionSoon()
		{
			return NamePlayerFactionAndBaseUtility.CanNameFaction(Find.TickManager.TicksGame + 30000);
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x000E26D0 File Offset: 0x000E0AD0
		public static bool CanNameFactionBaseSoon(FactionBase factionBase)
		{
			return NamePlayerFactionAndBaseUtility.CanNameFactionBase(factionBase, Find.TickManager.TicksGame - factionBase.creationGameTicks + 30000);
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x000E2704 File Offset: 0x000E0B04
		private static bool CanNameFaction(int ticksPassed)
		{
			return !Faction.OfPlayer.HasName && (float)ticksPassed / 60000f >= 3f && NamePlayerFactionAndBaseUtility.CanNameAnythingNow();
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x000E2744 File Offset: 0x000E0B44
		private static bool CanNameFactionBase(FactionBase factionBase, int ticksPassed)
		{
			return factionBase.Faction == Faction.OfPlayer && !factionBase.namedByPlayer && (float)ticksPassed / 60000f >= 3f && factionBase.HasMap && factionBase.Map.dangerWatcher.DangerRating != StoryDanger.High && factionBase.Map.mapPawns.FreeColonistsSpawnedCount != 0 && NamePlayerFactionAndBaseUtility.CanNameAnythingNow();
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x000E27C4 File Offset: 0x000E0BC4
		private static bool CanNameAnythingNow()
		{
			bool result;
			if (Find.AnyPlayerHomeMap == null || Find.CurrentMap == null || !Find.CurrentMap.IsPlayerHome || Find.GameEnder.gameEnding)
			{
				result = false;
			}
			else
			{
				bool flag = false;
				bool flag2 = false;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						if (maps[i].mapPawns.FreeColonistsSpawnedCount >= 2)
						{
							flag = true;
						}
						if (!maps[i].attackTargetsCache.TargetsHostileToColony.Any((IAttackTarget x) => GenHostility.IsActiveThreatToPlayer(x)))
						{
							flag2 = true;
						}
					}
				}
				result = (flag && flag2);
			}
			return result;
		}
	}
}
