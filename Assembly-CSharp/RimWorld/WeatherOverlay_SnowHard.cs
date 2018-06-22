using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200044D RID: 1101
	[StaticConstructorOnStartup]
	public class WeatherOverlay_SnowHard : SkyOverlay
	{
		// Token: 0x06001333 RID: 4915 RVA: 0x000A5574 File Offset: 0x000A3974
		public WeatherOverlay_SnowHard()
		{
			this.worldOverlayMat = WeatherOverlay_SnowHard.SnowOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.008f;
			this.worldPanDir1 = new Vector2(-0.5f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.009f;
			this.worldPanDir2 = new Vector2(-0.48f, -1f);
			this.worldPanDir2.Normalize();
		}

		// Token: 0x04000BAD RID: 2989
		private static readonly Material SnowOverlayWorld = MatLoader.LoadMat("Weather/SnowOverlayWorld", -1);
	}
}
