using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	public static class SettleInEmptyTileUtility
	{
		private const int MinStartingLocCellsCount = 600;

		public static void Settle(Caravan caravan)
		{
			Faction faction = caravan.Faction;
			if (faction != Faction.OfPlayer)
			{
				Log.Error("Cannot settle with non-player faction.");
				return;
			}
			FactionBase newHome = SettleUtility.AddNewHome(caravan.Tile, faction);
			LongEventHandler.QueueLongEvent(delegate
			{
				GetOrGenerateMapUtility.GetOrGenerateMap(caravan.Tile, Find.World.info.initialMapSize, null);
			}, "GeneratingMap", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap));
			LongEventHandler.QueueLongEvent(delegate
			{
				Map map = newHome.Map;
				Pawn t = caravan.PawnsListForReading[0];
				Predicate<IntVec3> extraCellValidator = (IntVec3 x) => x.GetRoom(map, RegionType.Set_Passable).CellCount >= 600;
				CaravanEnterMapUtility.Enter(caravan, map, CaravanEnterMode.Center, CaravanDropInventoryMode.DropInstantly, false, extraCellValidator);
				CameraJumper.TryJump(t);
			}, "SpawningColonists", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap));
		}

		public static Command SettleCommand(Caravan caravan)
		{
			Command_Settle command_Settle = new Command_Settle();
			command_Settle.defaultLabel = "CommandSettle".Translate();
			command_Settle.defaultDesc = "CommandSettleDesc".Translate();
			command_Settle.icon = SettleUtility.SettleCommandTex;
			command_Settle.action = delegate
			{
				SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
				SettleInEmptyTileUtility.Settle(caravan);
			};
			if (Find.WorldObjects.AnyFactionBaseOrDestroyedFactionBaseAtOrAdjacent(caravan.Tile))
			{
				command_Settle.Disable("CommandSettleFailFactionBaseAdjacent".Translate());
			}
			else
			{
				bool flag = false;
				List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
				for (int i = 0; i < allWorldObjects.Count; i++)
				{
					WorldObject worldObject = allWorldObjects[i];
					if (worldObject.Tile == caravan.Tile && worldObject != caravan)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					command_Settle.Disable("CommandSettleFailOtherWorldObjectsHere".Translate());
				}
				else if (SettleUtility.PlayerHomesCountLimitReached)
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
				else if (!Find.WorldGrid[caravan.Tile].biome.implemented)
				{
					command_Settle.Disable("CommandSettleFailBiomeNotImplemented".Translate());
				}
			}
			return command_Settle;
		}
	}
}
