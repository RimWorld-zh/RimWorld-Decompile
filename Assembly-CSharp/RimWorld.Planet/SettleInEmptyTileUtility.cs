using System;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	public static class SettleInEmptyTileUtility
	{
		private const int MinStartingLocCellsCount = 600;

		private static StringBuilder tmpSettleFailReason = new StringBuilder();

		[CompilerGenerated]
		private static Action<Exception> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static Action<Exception> _003C_003Ef__mg_0024cache1;

		public static void Settle(Caravan caravan)
		{
			Faction faction = caravan.Faction;
			if (faction != Faction.OfPlayer)
			{
				Log.Error("Cannot settle with non-player faction.");
			}
			else
			{
				FactionBase newHome = SettleUtility.AddNewHome(caravan.Tile, faction);
				LongEventHandler.QueueLongEvent((Action)delegate()
				{
					GetOrGenerateMapUtility.GetOrGenerateMap(caravan.Tile, Find.World.info.initialMapSize, null);
				}, "GeneratingMap", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap));
				LongEventHandler.QueueLongEvent((Action)delegate()
				{
					Map map = newHome.Map;
					Pawn t = caravan.PawnsListForReading[0];
					CaravanEnterMapUtility.Enter(caravan, map, CaravanEnterMode.Center, CaravanDropInventoryMode.DropInstantly, false, (Predicate<IntVec3>)((IntVec3 x) => x.GetRoom(map, RegionType.Set_Passable).CellCount >= 600));
					CameraJumper.TryJump((Thing)t);
				}, "SpawningColonists", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap));
			}
		}

		public static Command SettleCommand(Caravan caravan)
		{
			Command_Settle command_Settle = new Command_Settle();
			command_Settle.defaultLabel = "CommandSettle".Translate();
			command_Settle.defaultDesc = "CommandSettleDesc".Translate();
			command_Settle.icon = SettleUtility.SettleCommandTex;
			command_Settle.action = (Action)delegate()
			{
				SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
				SettleInEmptyTileUtility.Settle(caravan);
			};
			SettleInEmptyTileUtility.tmpSettleFailReason.Length = 0;
			if (!TileFinder.IsValidTileForNewSettlement(caravan.Tile, SettleInEmptyTileUtility.tmpSettleFailReason))
			{
				command_Settle.Disable(SettleInEmptyTileUtility.tmpSettleFailReason.ToString());
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
			return command_Settle;
		}
	}
}
