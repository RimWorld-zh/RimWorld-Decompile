using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200074E RID: 1870
	public static class GameVictoryUtility
	{
		// Token: 0x0600297A RID: 10618 RVA: 0x00160868 File Offset: 0x0015EC68
		public static void ShowCredits(string victoryText)
		{
			Screen_Credits screen_Credits = new Screen_Credits(victoryText);
			screen_Credits.wonGame = true;
			Find.WindowStack.Add(screen_Credits);
			Find.MusicManagerPlay.ForceSilenceFor(999f);
			ScreenFader.StartFade(Color.clear, 3f);
		}

		// Token: 0x0600297B RID: 10619 RVA: 0x001608B0 File Offset: 0x0015ECB0
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
