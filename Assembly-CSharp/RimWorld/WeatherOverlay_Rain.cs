using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200044E RID: 1102
	[StaticConstructorOnStartup]
	public class WeatherOverlay_Rain : SkyOverlay
	{
		// Token: 0x04000BAF RID: 2991
		private static readonly Material RainOverlayWorld = MatLoader.LoadMat("Weather/RainOverlayWorld", -1);

		// Token: 0x06001334 RID: 4916 RVA: 0x000A583C File Offset: 0x000A3C3C
		public WeatherOverlay_Rain()
		{
			this.worldOverlayMat = WeatherOverlay_Rain.RainOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.015f;
			this.worldPanDir1 = new Vector2(-0.25f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.022f;
			this.worldPanDir2 = new Vector2(-0.24f, -1f);
			this.worldPanDir2.Normalize();
		}
	}
}
