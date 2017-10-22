using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class NamePlayerFactionAndBaseUtility
	{
		private const float MinDaysPassedToNameFaction = 3f;

		private const float MinDaysPassedToNameFactionBase = 3f;

		private const int SoonTicks = 30000;

		public static bool CanNameFactionNow()
		{
			return NamePlayerFactionAndBaseUtility.CanNameFaction(Find.TickManager.TicksGame);
		}

		public static bool CanNameFactionBaseNow(FactionBase factionBase)
		{
			return NamePlayerFactionAndBaseUtility.CanNameFactionBase(factionBase, Find.TickManager.TicksGame - factionBase.creationGameTicks);
		}

		public static bool CanNameFactionSoon()
		{
			return NamePlayerFactionAndBaseUtility.CanNameFaction(Find.TickManager.TicksGame + 30000);
		}

		public static bool CanNameFactionBaseSoon(FactionBase factionBase)
		{
			return NamePlayerFactionAndBaseUtility.CanNameFactionBase(factionBase, Find.TickManager.TicksGame - factionBase.creationGameTicks + 30000);
		}

		private static bool CanNameFaction(int ticksPassed)
		{
			return !Faction.OfPlayer.HasName && (float)ticksPassed / 60000.0 >= 3.0 && NamePlayerFactionAndBaseUtility.CanNameAnythingNow();
		}

		private static bool CanNameFactionBase(FactionBase factionBase, int ticksPassed)
		{
			return ((factionBase.Faction == Faction.OfPlayer) ? ((!factionBase.namedByPlayer) ? (((float)ticksPassed / 60000.0 >= 3.0) ? (factionBase.HasMap ? ((factionBase.Map.dangerWatcher.DangerRating != StoryDanger.High) ? factionBase.Map.mapPawns.FreeColonistsSpawnedCount : 0) : 0) : 0) : 0) : 0) != 0 && NamePlayerFactionAndBaseUtility.CanNameAnythingNow();
		}

		private static bool CanNameAnythingNow()
		{
			bool result;
			if (Find.AnyPlayerHomeMap == null || Find.VisibleMap == null || !Find.VisibleMap.IsPlayerHome || Find.GameEnder.gameEnding)
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
						if (!maps[i].attackTargetsCache.TargetsHostileToColony.Any((Func<IAttackTarget, bool>)((IAttackTarget x) => GenHostility.IsActiveThreatToPlayer(x))))
						{
							flag2 = true;
						}
					}
				}
				result = ((byte)((flag && flag2) ? 1 : 0) != 0);
			}
			return result;
		}
	}
}
