using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020005F8 RID: 1528
	public static class SettleInExistingMapUtility
	{
		// Token: 0x04001212 RID: 4626
		private static List<Pawn> tmpPlayerPawns = new List<Pawn>();

		// Token: 0x06001E66 RID: 7782 RVA: 0x0010779C File Offset: 0x00105B9C
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

		// Token: 0x06001E67 RID: 7783 RVA: 0x001078F8 File Offset: 0x00105CF8
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
