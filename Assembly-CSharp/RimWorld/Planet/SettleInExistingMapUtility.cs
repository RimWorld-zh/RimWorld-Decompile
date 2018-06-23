using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020005F6 RID: 1526
	public static class SettleInExistingMapUtility
	{
		// Token: 0x0400120E RID: 4622
		private static List<Pawn> tmpPlayerPawns = new List<Pawn>();

		// Token: 0x06001E63 RID: 7779 RVA: 0x001073E4 File Offset: 0x001057E4
		public static Command SettleCommand(Map map, bool requiresNoEnemies)
		{
			Command_Settle command_Settle = new Command_Settle();
			command_Settle.defaultLabel = "CommandSettle".Translate();
			command_Settle.defaultDesc = "CommandSettleDesc".Translate();
			command_Settle.icon = SettleUtility.SettleCommandTex;
			command_Settle.action = delegate()
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				FactionBaseProximityGoodwillUtility.CheckConfirmSettle(map.Tile, delegate
				{
					SettleInExistingMapUtility.Settle(map);
				});
			};
			if (SettleUtility.PlayerHomesCountLimitReached)
			{
				if (Prefs.MaxNumberOfPlayerHomes > 1)
				{
					command_Settle.Disable("CommandSettleFailReachedMaximumNumberOfBases".Translate());
				}
				else
				{
					command_Settle.Disable("CommandSettleFailAlreadyHaveBase".Translate());
				}
			}
			if (!command_Settle.disabled)
			{
				if (map.mapPawns.FreeColonistsCount == 0)
				{
					command_Settle.Disable("CommandSettleFailNoColonists".Translate());
				}
				else if (requiresNoEnemies)
				{
					foreach (IAttackTarget target in map.attackTargetsCache.TargetsHostileToColony)
					{
						if (GenHostility.IsActiveThreatToPlayer(target))
						{
							command_Settle.Disable("CommandSettleFailEnemies".Translate());
							break;
						}
					}
				}
			}
			return command_Settle;
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x00107540 File Offset: 0x00105940
		public static void Settle(Map map)
		{
			MapParent parent = map.Parent;
			FactionBase factionBase = SettleUtility.AddNewHome(map.Tile, Faction.OfPlayer);
			map.info.parent = factionBase;
			if (parent != null)
			{
				Find.WorldObjects.Remove(parent);
			}
			Messages.Message("MessageSettledInExistingMap".Translate(), factionBase, MessageTypeDefOf.PositiveEvent, false);
			SettleInExistingMapUtility.tmpPlayerPawns.Clear();
			SettleInExistingMapUtility.tmpPlayerPawns.AddRange(from x in map.mapPawns.AllPawnsSpawned
			where x.Faction == Faction.OfPlayer || x.HostFaction == Faction.OfPlayer
			select x);
			CaravanEnterMapUtility.DropAllInventory(SettleInExistingMapUtility.tmpPlayerPawns);
			SettleInExistingMapUtility.tmpPlayerPawns.Clear();
			List<Pawn> prisonersOfColonySpawned = map.mapPawns.PrisonersOfColonySpawned;
			for (int i = 0; i < prisonersOfColonySpawned.Count; i++)
			{
				prisonersOfColonySpawned[i].guest.WaitInsteadOfEscapingForDefaultTicks();
			}
		}
	}
}
