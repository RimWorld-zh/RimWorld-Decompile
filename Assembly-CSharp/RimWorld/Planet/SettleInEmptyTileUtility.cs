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
		private static Action<Exception> <>f__mg$cache0;

		[CompilerGenerated]
		private static Action<Exception> <>f__mg$cache1;

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

		// Note: this type is marked as 'beforefieldinit'.
		static SettleInEmptyTileUtility()
		{
		}

		[CompilerGenerated]
		private sealed class <Settle>c__AnonStorey0
		{
			internal Caravan caravan;

			internal FactionBase newHome;

			public <Settle>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				GetOrGenerateMapUtility.GetOrGenerateMap(this.caravan.Tile, Find.World.info.initialMapSize, null);
			}

			internal void <>m__1()
			{
				Map map = this.newHome.Map;
				Pawn t = this.caravan.PawnsListForReading[0];
				CaravanEnterMapUtility.Enter(this.caravan, map, CaravanEnterMode.Center, CaravanDropInventoryMode.DropInstantly, false, (IntVec3 x) => x.GetRoom(map, RegionType.Set_Passable).CellCount >= 600);
				CameraJumper.TryJump(t);
			}

			private sealed class <Settle>c__AnonStorey1
			{
				internal Map map;

				internal SettleInEmptyTileUtility.<Settle>c__AnonStorey0 <>f__ref$0;

				public <Settle>c__AnonStorey1()
				{
				}

				internal bool <>m__0(IntVec3 x)
				{
					return x.GetRoom(this.map, RegionType.Set_Passable).CellCount >= 600;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <SettleCommand>c__AnonStorey2
		{
			internal Caravan caravan;

			public <SettleCommand>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				FactionBaseProximityGoodwillUtility.CheckConfirmSettle(this.caravan.Tile, delegate
				{
					SettleInEmptyTileUtility.Settle(this.caravan);
				});
			}

			internal void <>m__1()
			{
				SettleInEmptyTileUtility.Settle(this.caravan);
			}
		}
	}
}
