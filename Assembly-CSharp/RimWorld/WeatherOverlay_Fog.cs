using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200044B RID: 1099
	[StaticConstructorOnStartup]
	public class WeatherOverlay_Fog : SkyOverlay
	{
		// Token: 0x0600132F RID: 4911 RVA: 0x000A5478 File Offset: 0x000A3878
		public WeatherOverlay_Fog()
		{
			this.worldOverlayMat = WeatherOverlay_Fog.FogOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.0005f;
			this.worldOverlayPanSpeed2 = 0.0004f;
			this.worldPanDir1 = new Vector2(1f, 1f);
			this.worldPanDir2 = new Vector2(1f, -1f);
		}

		// Token: 0x04000BAB RID: 2987
		private static readonly Material FogOverlayWorld = MatLoader.LoadMat("Weather/FogOverlayWorld", -1);
	}
}
