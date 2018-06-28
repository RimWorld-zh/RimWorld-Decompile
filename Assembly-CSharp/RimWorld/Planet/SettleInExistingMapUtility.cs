using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld.Planet
{
	public static class SettleInExistingMapUtility
	{
		private static List<Pawn> tmpPlayerPawns = new List<Pawn>();

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

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
			if (SettleUtility.PlayerSettlementsCountLimitReached)
			{
				if (Prefs.MaxNumberOfPlayerSettlements > 1)
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

		public static void Settle(Map map)
		{
			MapParent parent = map.Parent;
			Settlement settlement = SettleUtility.AddNewHome(map.Tile, Faction.OfPlayer);
			map.info.parent = settlement;
			if (parent != null)
			{
				Find.WorldObjects.Remove(parent);
			}
			Messages.Message("MessageSettledInExistingMap".Translate(), settlement, MessageTypeDefOf.PositiveEvent, false);
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

		// Note: this type is marked as 'beforefieldinit'.
		static SettleInExistingMapUtility()
		{
		}

		[CompilerGenerated]
		private static bool <Settle>m__0(Pawn x)
		{
			return x.Faction == Faction.OfPlayer || x.HostFaction == Faction.OfPlayer;
		}

		[CompilerGenerated]
		private sealed class <SettleCommand>c__AnonStorey0
		{
			internal Map map;

			public <SettleCommand>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				FactionBaseProximityGoodwillUtility.CheckConfirmSettle(this.map.Tile, delegate
				{
					SettleInExistingMapUtility.Settle(this.map);
				});
			}

			internal void <>m__1()
			{
				SettleInExistingMapUtility.Settle(this.map);
			}
		}
	}
}
