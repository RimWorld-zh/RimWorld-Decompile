using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000450 RID: 1104
	[StaticConstructorOnStartup]
	public class WeatherOverlay_SnowGentle : SkyOverlay
	{
		// Token: 0x04000BAE RID: 2990
		private static readonly Material SnowGentleOverlayWorld = MatLoader.LoadMat("Weather/SnowOverlayWorld", -1);

		// Token: 0x06001339 RID: 4921 RVA: 0x000A574C File Offset: 0x000A3B4C
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
	}
}
