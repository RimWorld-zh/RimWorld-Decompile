using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000563 RID: 1379
	public static class NamePlayerFactionAndBaseUtility
	{
		// Token: 0x06001A11 RID: 6673 RVA: 0x000E2504 File Offset: 0x000E0904
		public static bool CanNameFactionNow()
		{
			return NamePlayerFactionAndBaseUtility.CanNameFaction(Find.TickManager.TicksGame);
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x000E2528 File Offset: 0x000E0928
		public static bool CanNameFactionBaseNow(FactionBase factionBase)
		{
			return NamePlayerFactionAndBaseUtility.CanNameFactionBase(factionBase, Find.TickManager.TicksGame - factionBase.creationGameTicks);
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x000E2554 File Offset: 0x000E0954
		public static bool CanNameFactionSoon()
		{
			return NamePlayerFactionAndBaseUtility.CanNameFaction(Find.TickManager.TicksGame + 30000);
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x000E2580 File Offset: 0x000E0980
		public static bool CanNameFactionBaseSoon(FactionBase factionBase)
		{
			return NamePlayerFactionAndBaseUtility.CanNameFactionBase(factionBase, Find.TickManager.TicksGame - factionBase.creationGameTicks + 30000);
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x000E25B4 File Offset: 0x000E09B4
		private static bool CanNameFaction(int ticksPassed)
		{
			return !Faction.OfPlayer.HasName && (float)ticksPassed / 60000f >= 3f && NamePlayerFactionAndBaseUtility.CanNameAnythingNow();
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x000E25F4 File Offset: 0x000E09F4
		private static bool CanNameFactionBase(FactionBase factionBase, int ticksPassed)
		{
			return factionBase.Faction == Faction.OfPlayer && !factionBase.namedByPlayer && (float)ticksPassed / 60000f >= 3f && factionBase.HasMap && factionBase.Map.dangerWatcher.DangerRating != StoryDanger.High && factionBase.Map.mapPawns.FreeColonistsSpawnedCount != 0 && NamePlayerFactionAndBaseUtility.CanNameAnythingNow();
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x000E2674 File Offset: 0x000E0A74
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

		// Token: 0x04000F42 RID: 3906
		private const float MinDaysPassedToNameFaction = 3f;

		// Token: 0x04000F43 RID: 3907
		private const float MinDaysPassedToNameFactionBase = 3f;

		// Token: 0x04000F44 RID: 3908
		private const int SoonTicks = 30000;
	}
}
