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
		// Token: 0x04000F46 RID: 3910
		private const float MinDaysPassedToNameFaction = 3f;

		// Token: 0x04000F47 RID: 3911
		private const float MinDaysPassedToNameFactionBase = 3f;

		// Token: 0x04000F48 RID: 3912
		private const int SoonTicks = 30000;

		// Token: 0x06001A14 RID: 6676 RVA: 0x000E28BC File Offset: 0x000E0CBC
		public static bool CanNameFactionNow()
		{
			return NamePlayerFactionAndBaseUtility.CanNameFaction(Find.TickManager.TicksGame);
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x000E28E0 File Offset: 0x000E0CE0
		public static bool CanNameFactionBaseNow(FactionBase factionBase)
		{
			return NamePlayerFactionAndBaseUtility.CanNameFactionBase(factionBase, Find.TickManager.TicksGame - factionBase.creationGameTicks);
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x000E290C File Offset: 0x000E0D0C
		public static bool CanNameFactionSoon()
		{
			return NamePlayerFactionAndBaseUtility.CanNameFaction(Find.TickManager.TicksGame + 30000);
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x000E2938 File Offset: 0x000E0D38
		public static bool CanNameFactionBaseSoon(FactionBase factionBase)
		{
			return NamePlayerFactionAndBaseUtility.CanNameFactionBase(factionBase, Find.TickManager.TicksGame - factionBase.creationGameTicks + 30000);
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x000E296C File Offset: 0x000E0D6C
		private static bool CanNameFaction(int ticksPassed)
		{
			return !Faction.OfPlayer.HasName && (float)ticksPassed / 60000f >= 3f && NamePlayerFactionAndBaseUtility.CanNameAnythingNow();
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x000E29AC File Offset: 0x000E0DAC
		private static bool CanNameFactionBase(FactionBase factionBase, int ticksPassed)
		{
			return factionBase.Faction == Faction.OfPlayer && !factionBase.namedByPlayer && (float)ticksPassed / 60000f >= 3f && factionBase.HasMap && factionBase.Map.dangerWatcher.DangerRating != StoryDanger.High && factionBase.Map.mapPawns.FreeColonistsSpawnedCount != 0 && NamePlayerFactionAndBaseUtility.CanNameAnythingNow();
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x000E2A2C File Offset: 0x000E0E2C
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
