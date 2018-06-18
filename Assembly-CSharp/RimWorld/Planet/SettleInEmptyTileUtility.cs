using System;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020005F9 RID: 1529
	public static class SettleInEmptyTileUtility
	{
		// Token: 0x06001E69 RID: 7785 RVA: 0x001070D4 File Offset: 0x001054D4
		public static void Settle(Caravan caravan)
		{
			Faction faction = caravan.Faction;
			if (faction != Faction.OfPlayer)
			{
				Log.Error("Cannot settle with non-player faction.", false);
			}
			else
			{
				FactionBase newHome = SettleUtility.AddNewHome(caravan.Tile, faction);
				Action action = delegate()
				{
					GetOrGenerateMapUtility.GetOrGenerateMap(caravan.Tile, Find.World.info.initialMapSize, null);
				};
				string textKey = "GeneratingMap";
				bool doAsynchronously = true;
				if (SettleInEmptyTileUtility.<>f__mg$cache0 == null)
				{
					SettleInEmptyTileUtility.<>f__mg$cache0 = new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap);
				}
				LongEventHandler.QueueLongEvent(action, textKey, doAsynchronously, SettleInEmptyTileUtility.<>f__mg$cache0);
				Action action2 = delegate()
				{
					Map map = newHome.Map;
					Pawn t = caravan.PawnsListForReading[0];
					CaravanEnterMapUtility.Enter(caravan, map, CaravanEnterMode.Center, CaravanDropInventoryMode.DropInstantly, false, (IntVec3 x) => x.GetRoom(map, RegionType.Set_Passable).CellCount >= 600);
					CameraJumper.TryJump(t);
				};
				string textKey2 = "SpawningColonists";
				bool doAsynchronously2 = true;
				if (SettleInEmptyTileUtility.<>f__mg$cache1 == null)
				{
					SettleInEmptyTileUtility.<>f__mg$cache1 = new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap);
				}
				LongEventHandler.QueueLongEvent(action2, textKey2, doAsynchronously2, SettleInEmptyTileUtility.<>f__mg$cache1);
			}
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x00107198 File Offset: 0x00105598
		public static Command SettleCommand(Caravan caravan)
		{
			Command_Settle command_Settle = new Command_Settle();
			command_Settle.defaultLabel = "CommandSettle".Translate();
			command_Settle.defaultDesc = "CommandSettleDesc".Translate();
			command_Settle.icon = SettleUtility.SettleCommandTex;
			command_Settle.action = delegate()
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				FactionBaseProximityGoodwillUtility.CheckConfirmSettle(caravan.Tile, delegate
				{
					SettleInEmptyTileUtility.Settle(caravan);
				});
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

		// Token: 0x0400120D RID: 4621
		private const int MinStartingLocCellsCount = 600;

		// Token: 0x0400120E RID: 4622
		private static StringBuilder tmpSettleFailReason = new StringBuilder();

		// Token: 0x0400120F RID: 4623
		[CompilerGenerated]
		private static Action<Exception> <>f__mg$cache0;

		// Token: 0x04001210 RID: 4624
		[CompilerGenerated]
		private static Action<Exception> <>f__mg$cache1;
	}
}
