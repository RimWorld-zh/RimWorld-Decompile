using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200044E RID: 1102
	[StaticConstructorOnStartup]
	public class WeatherOverlay_SnowGentle : SkyOverlay
	{
		// Token: 0x06001335 RID: 4917 RVA: 0x000A55FC File Offset: 0x000A39FC
		public WeatherOverlay_SnowGentle()
		{
			this.worldOverlayMat = WeatherOverlay_SnowGentle.SnowGentleOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.002f;
			this.worldPanDir1 = new Vector2(-0.25f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.003f;
			this.worldPanDir2 = new Vector2(-0.24f, -1f);
			this.worldPanDir2.Normalize();
		}

		// Token: 0x04000BAE RID: 2990
		private static readonly Material SnowGentleOverlayWorld = MatLoader.LoadMat("Weather/SnowOverlayWorld", -1);
	}
}
