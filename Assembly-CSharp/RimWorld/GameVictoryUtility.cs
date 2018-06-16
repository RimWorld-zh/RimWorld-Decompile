using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000750 RID: 1872
	public static class GameVictoryUtility
	{
		// Token: 0x0600297B RID: 10619 RVA: 0x001604AC File Offset: 0x0015E8AC
		public static void ShowCredits(string victoryText)
		{
			Screen_Credits screen_Credits = new Screen_Credits(victoryText);
			screen_Credits.wonGame = true;
			Find.WindowStack.Add(screen_Credits);
			Find.MusicManagerPlay.ForceSilenceFor(999f);
			ScreenFader.StartFade(Color.clear, 3f);
		}

		// Token: 0x0600297C RID: 10620 RVA: 0x001604F4 File Offset: 0x0015E8F4
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
