using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200074F RID: 1871
	public static class ShipCountdown
	{
		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06002977 RID: 10615 RVA: 0x00160394 File Offset: 0x0015E794
		public static bool CountingDown
		{
			get
			{
				return ShipCountdown.timeLeft >= 0f;
			}
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x001603B8 File Offset: 0x0015E7B8
		public static void InitiateCountdown(Building launchingShipRoot)
		{
			SoundDefOf.ShipTakeoff.PlayOneShotOnCamera(null);
			ShipCountdown.shipRoot = launchingShipRoot;
			ShipCountdown.timeLeft = 7.2f;
			ScreenFader.StartFade(Color.white, 7.2f);
		}

		// Token: 0x06002979 RID: 10617 RVA: 0x001603E5 File Offset: 0x0015E7E5
		public static void ShipCountdownUpdate()
		{
			if (ShipCountdown.timeLeft > 0f)
			{
				ShipCountdown.timeLeft -= Time.deltaTime;
				if (ShipCountdown.timeLeft <= 0f)
				{
					ShipCountdown.CountdownEnded();
				}
			}
		}

		// Token: 0x0600297A RID: 10618 RVA: 0x0016041F File Offset: 0x0015E81F
		public static void CancelCountdown()
		{
			ShipCountdown.timeLeft = -1000f;
		}

		// Token: 0x0600297B RID: 10619 RVA: 0x0016042C File Offset: 0x0015E82C
		private static void CountdownEnded()
		{
			List<Building> list = ShipUtility.ShipBuildingsAttachedTo(ShipCountdown.shipRoot).ToList<Building>();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Building building in list)
			{
				Building_CryptosleepCasket building_CryptosleepCasket = building as Building_CryptosleepCasket;
				if (building_CryptosleepCasket != null && building_CryptosleepCasket.HasAnyContents)
				{
					stringBuilder.AppendLine("   " + building_CryptosleepCasket.ContainedThing.LabelCap);
					Find.StoryWatcher.statsRecord.colonistsLaunched++;
					TaleRecorder.RecordTale(TaleDefOf.LaunchedShip, new object[]
					{
						building_CryptosleepCasket.ContainedThing
					});
				}
				building.Destroy(DestroyMode.Vanish);
			}
			string victoryText = "GameOverShipLaunched".Translate(new object[]
			{
				stringBuilder.ToString(),
				GameVictoryUtility.PawnsLeftBehind()
			});
			GameVictoryUtility.ShowCredits(victoryText);
		}

		// Token: 0x04001693 RID: 5779
		private static float timeLeft = -1000f;

		// Token: 0x04001694 RID: 5780
		private static Building shipRoot;

		// Token: 0x04001695 RID: 5781
		private const float InitialTime = 7.2f;
	}
}
