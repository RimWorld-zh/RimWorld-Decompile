using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000567 RID: 1383
	public static class NamePlayerFactionAndBaseUtility
	{
		// Token: 0x06001A19 RID: 6681 RVA: 0x000E245C File Offset: 0x000E085C
		public static bool CanNameFactionNow()
		{
			return NamePlayerFactionAndBaseUtility.CanNameFaction(Find.TickManager.TicksGame);
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x000E2480 File Offset: 0x000E0880
		public static bool CanNameFactionBaseNow(FactionBase factionBase)
		{
			return NamePlayerFactionAndBaseUtility.CanNameFactionBase(factionBase, Find.TickManager.TicksGame - factionBase.creationGameTicks);
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x000E24AC File Offset: 0x000E08AC
		public static bool CanNameFactionSoon()
		{
			return NamePlayerFactionAndBaseUtility.CanNameFaction(Find.TickManager.TicksGame + 30000);
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x000E24D8 File Offset: 0x000E08D8
		public static bool CanNameFactionBaseSoon(FactionBase factionBase)
		{
			return NamePlayerFactionAndBaseUtility.CanNameFactionBase(factionBase, Find.TickManager.TicksGame - factionBase.creationGameTicks + 30000);
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x000E250C File Offset: 0x000E090C
		private static bool CanNameFaction(int ticksPassed)
		{
			return !Faction.OfPlayer.HasName && (float)ticksPassed / 60000f >= 3f && NamePlayerFactionAndBaseUtility.CanNameAnythingNow();
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x000E254C File Offset: 0x000E094C
		private static bool CanNameFactionBase(FactionBase factionBase, int ticksPassed)
		{
			return factionBase.Faction == Faction.OfPlayer && !factionBase.namedByPlayer && (float)ticksPassed / 60000f >= 3f && factionBase.HasMap && factionBase.Map.dangerWatcher.DangerRating != StoryDanger.High && factionBase.Map.mapPawns.FreeColonistsSpawnedCount != 0 && NamePlayerFactionAndBaseUtility.CanNameAnythingNow();
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x000E25CC File Offset: 0x000E09CC
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

		// Token: 0x04000F45 RID: 3909
		private const float MinDaysPassedToNameFaction = 3f;

		// Token: 0x04000F46 RID: 3910
		private const float MinDaysPassedToNameFactionBase = 3f;

		// Token: 0x04000F47 RID: 3911
		private const int SoonTicks = 30000;
	}
}
