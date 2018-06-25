using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020002F3 RID: 755
	public sealed class GameEnder : IExposable
	{
		// Token: 0x04000830 RID: 2096
		public bool gameEnding = false;

		// Token: 0x04000831 RID: 2097
		private int ticksToGameOver = -1;

		// Token: 0x04000832 RID: 2098
		private const int GameEndCountdownDuration = 400;

		// Token: 0x06000C7D RID: 3197 RVA: 0x0006EEFF File Offset: 0x0006D2FF
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.gameEnding, "gameEnding", false, false);
			Scribe_Values.Look<int>(ref this.ticksToGameOver, "ticksToGameOver", -1, false);
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x0006EF28 File Offset: 0x0006D328
		public void CheckOrUpdateGameOver()
		{
			if (Find.TickManager.TicksGame >= 300)
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].mapPawns.FreeColonistsSpawnedOrInPlayerEjectablePodsCount >= 1)
					{
						this.gameEnding = false;
						return;
					}
				}
				for (int j = 0; j < maps.Count; j++)
				{
					List<Pawn> allPawnsSpawned = maps[j].mapPawns.AllPawnsSpawned;
					for (int k = 0; k < allPawnsSpawned.Count; k++)
					{
						if (allPawnsSpawned[k].carryTracker != null)
						{
							Pawn pawn = allPawnsSpawned[k].carryTracker.CarriedThing as Pawn;
							if (pawn != null && pawn.IsFreeColonist)
							{
								this.gameEnding = false;
								return;
							}
						}
					}
				}
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int l = 0; l < caravans.Count; l++)
				{
					if (this.IsPlayerControlledWithFreeColonist(caravans[l]))
					{
						this.gameEnding = false;
						return;
					}
				}
				List<TravelingTransportPods> travelingTransportPods = Find.WorldObjects.TravelingTransportPods;
				for (int m = 0; m < travelingTransportPods.Count; m++)
				{
					if (travelingTransportPods[m].PodsHaveAnyFreeColonist)
					{
						this.gameEnding = false;
						return;
					}
				}
				if (!this.gameEnding)
				{
					this.gameEnding = true;
					this.ticksToGameOver = 400;
				}
			}
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x0006F0E1 File Offset: 0x0006D4E1
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

		// Token: 0x06000C80 RID: 3200 RVA: 0x0006F11C File Offset: 0x0006D51C
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
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}
	}
}
