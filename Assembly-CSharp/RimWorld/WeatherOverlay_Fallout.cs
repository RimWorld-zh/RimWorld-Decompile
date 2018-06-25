using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000451 RID: 1105
	[StaticConstructorOnStartup]
	public class WeatherOverlay_Fallout : SkyOverlay
	{
		// Token: 0x04000BAF RID: 2991
		private static readonly Material FalloutOverlayWorld = MatLoader.LoadMat("Weather/SnowOverlayWorld", -1);

		// Token: 0x0600133B RID: 4923 RVA: 0x000A57D4 File Offset: 0x000A3BD4
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
	}
}
