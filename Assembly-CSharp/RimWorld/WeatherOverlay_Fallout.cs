using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200044F RID: 1103
	[StaticConstructorOnStartup]
	public class WeatherOverlay_Fallout : SkyOverlay
	{
		// Token: 0x06001337 RID: 4919 RVA: 0x000A5684 File Offset: 0x000A3A84
		public WeatherOverlay_Fallout()
		{
			this.worldOverlayMat = WeatherOverlay_Fallout.FalloutOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.0008f;
			this.worldPanDir1 = new Vector2(-0.25f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.0012f;
			this.worldPanDir2 = new Vector2(-0.24f, -1f);
			this.worldPanDir2.Normalize();
		}

		// Token: 0x04000BAF RID: 2991
		private static readonly Material FalloutOverlayWorld = MatLoader.LoadMat("Weather/SnowOverlayWorld", -1);
	}
}
