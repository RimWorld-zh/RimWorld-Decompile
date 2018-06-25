using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200074D RID: 1869
	public static class ShipCountdown
	{
		// Token: 0x04001691 RID: 5777
		private static float timeLeft = -1000f;

		// Token: 0x04001692 RID: 5778
		private static Building shipRoot;

		// Token: 0x04001693 RID: 5779
		private const float InitialTime = 7.2f;

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06002974 RID: 10612 RVA: 0x001606BC File Offset: 0x0015EABC
		public static bool CountingDown
		{
			get
			{
				return ShipCountdown.timeLeft >= 0f;
			}
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x001606E0 File Offset: 0x0015EAE0
		public static void InitiateCountdown(Building launchingShipRoot)
		{
			SoundDefOf.ShipTakeoff.PlayOneShotOnCamera(null);
			ShipCountdown.shipRoot = launchingShipRoot;
			ShipCountdown.timeLeft = 7.2f;
			ScreenFader.StartFade(Color.white, 7.2f);
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x0016070D File Offset: 0x0015EB0D
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

		// Token: 0x06002977 RID: 10615 RVA: 0x00160747 File Offset: 0x0015EB47
		public static void CancelCountdown()
		{
			ShipCountdown.timeLeft = -1000f;
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x00160754 File Offset: 0x0015EB54
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
	}
}
