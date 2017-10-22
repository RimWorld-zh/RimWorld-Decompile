using RimWorld.Planet;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public sealed class GameEnder : IExposable
	{
		public bool gameEnding = false;

		private int ticksToGameOver = -1;

		private const int GameEndCountdownDuration = 400;

		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.gameEnding, "gameEnding", false, false);
			Scribe_Values.Look<int>(ref this.ticksToGameOver, "ticksToGameOver", -1, false);
		}

		public void CheckGameOver()
		{
			if (Find.TickManager.TicksGame >= 300 && !this.gameEnding)
			{
				List<Map> maps = Find.Maps;
				int num = 0;
				while (num < maps.Count)
				{
					if (maps[num].mapPawns.FreeColonistsSpawnedOrInPlayerEjectablePodsCount < 1)
					{
						num++;
						continue;
					}
					return;
				}
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				int num2 = 0;
				while (num2 < caravans.Count)
				{
					if (!this.IsPlayerControlledWithFreeColonist(caravans[num2]))
					{
						num2++;
						continue;
					}
					return;
				}
				List<TravelingTransportPods> travelingTransportPods = Find.WorldObjects.TravelingTransportPods;
				int num3 = 0;
				while (num3 < travelingTransportPods.Count)
				{
					if (!travelingTransportPods[num3].PodsHaveAnyFreeColonist)
					{
						num3++;
						continue;
					}
					return;
				}
				this.gameEnding = true;
				this.ticksToGameOver = 400;
			}
		}

		public void GameEndTick()
		{
			if (this.gameEnding)
			{
				this.ticksToGameOver--;
				if (this.ticksToGameOver == 0)
				{
					GenGameEnd.EndGameDialogMessage("GameOverEveryoneDead".Translate(), true);
				}
			}
		}

		private bool IsPlayerControlledWithFreeColonist(Caravan caravan)
		{
			bool result;
			if (!caravan.IsPlayerControlled)
			{
				result = false;
			}
			else
			{
				List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					Pawn pawn = pawnsListForReading[i];
					if (pawn.IsColonist && pawn.HostFaction == null)
						goto IL_0040;
				}
				result = false;
			}
			goto IL_005f;
			IL_005f:
			return result;
			IL_0040:
			result = true;
			goto IL_005f;
		}
	}
}
