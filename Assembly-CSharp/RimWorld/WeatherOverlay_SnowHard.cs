using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200044F RID: 1103
	[StaticConstructorOnStartup]
	public class WeatherOverlay_SnowHard : SkyOverlay
	{
		// Token: 0x04000BAD RID: 2989
		private static readonly Material SnowOverlayWorld = MatLoader.LoadMat("Weather/SnowOverlayWorld", -1);

		// Token: 0x06001337 RID: 4919 RVA: 0x000A56C4 File Offset: 0x000A3AC4
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
	}
}
