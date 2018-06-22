using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200074C RID: 1868
	public static class GameVictoryUtility
	{
		// Token: 0x06002976 RID: 10614 RVA: 0x00160718 File Offset: 0x0015EB18
		public static void ShowCredits(string victoryText)
		{
			Screen_Credits screen_Credits = new Screen_Credits(victoryText);
			screen_Credits.wonGame = true;
			Find.WindowStack.Add(screen_Credits);
			Find.MusicManagerPlay.ForceSilenceFor(999f);
			ScreenFader.StartFade(Color.clear, 3f);
		}

		// Token: 0x06002977 RID: 10615 RVA: 0x00160760 File Offset: 0x0015EB60
		public static string PawnsLeftBehind()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
			{
				stringBuilder.AppendLine("   " + pawn.LabelCap);
			}
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				Caravan caravan = caravans[i];
				if (caravan.IsPlayerControlled)
				{
					List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
					for (int j = 0; j < pawnsListForReading.Count; j++)
					{
						Pawn pawn2 = pawnsListForReading[j];
						if (pawn2.IsColonist && pawn2.HostFaction == null)
						{
							stringBuilder.AppendLine("   " + pawn2.LabelCap);
						}
					}
				}
			}
			if (stringBuilder.Length == 0)
			{
				stringBuilder.AppendLine("Nobody".Translate().ToLower());
			}
			return stringBuilder.ToString();
		}
	}
}
