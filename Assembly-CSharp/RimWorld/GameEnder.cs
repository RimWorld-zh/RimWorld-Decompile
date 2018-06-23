using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020002F1 RID: 753
	public sealed class GameEnder : IExposable
	{
		// Token: 0x0400082D RID: 2093
		public bool gameEnding = false;

		// Token: 0x0400082E RID: 2094
		private int ticksToGameOver = -1;

		// Token: 0x0400082F RID: 2095
		private const int GameEndCountdownDuration = 400;

		// Token: 0x06000C7A RID: 3194 RVA: 0x0006EDA7 File Offset: 0x0006D1A7
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.gameEnding, "gameEnding", false, false);
			Scribe_Values.Look<int>(ref this.ticksToGameOver, "ticksToGameOver", -1, false);
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x0006EDD0 File Offset: 0x0006D1D0
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

		// Token: 0x06000C7C RID: 3196 RVA: 0x0006EF89 File Offset: 0x0006D389
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

		// Token: 0x06000C7D RID: 3197 RVA: 0x0006EFC4 File Offset: 0x0006D3C4
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
