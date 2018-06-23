using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200074B RID: 1867
	public static class ShipCountdown
	{
		// Token: 0x04001691 RID: 5777
		private static float timeLeft = -1000f;

		// Token: 0x04001692 RID: 5778
		private static Building shipRoot;

		// Token: 0x04001693 RID: 5779
		private const float InitialTime = 7.2f;

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06002970 RID: 10608 RVA: 0x0016056C File Offset: 0x0015E96C
		public static bool CountingDown
		{
			get
			{
				return ShipCountdown.timeLeft >= 0f;
			}
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x00160590 File Offset: 0x0015E990
		public static void InitiateCountdown(Building launchingShipRoot)
		{
			SoundDefOf.ShipTakeoff.PlayOneShotOnCamera(null);
			ShipCountdown.shipRoot = launchingShipRoot;
			ShipCountdown.timeLeft = 7.2f;
			ScreenFader.StartFade(Color.white, 7.2f);
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x001605BD File Offset: 0x0015E9BD
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

		// Token: 0x06002973 RID: 10611 RVA: 0x001605F7 File Offset: 0x0015E9F7
		public static void CancelCountdown()
		{
			ShipCountdown.timeLeft = -1000f;
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x00160604 File Offset: 0x0015EA04
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
