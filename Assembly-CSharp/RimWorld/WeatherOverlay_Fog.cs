using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200044D RID: 1101
	[StaticConstructorOnStartup]
	public class WeatherOverlay_Fog : SkyOverlay
	{
		// Token: 0x04000BAB RID: 2987
		private static readonly Material FogOverlayWorld = MatLoader.LoadMat("Weather/FogOverlayWorld", -1);

		// Token: 0x06001333 RID: 4915 RVA: 0x000A55C8 File Offset: 0x000A39C8
		public WeatherOverlay_Fog()
		{
			this.worldOverlayMat = WeatherOverlay_Fog.FogOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.0005f;
			this.worldOverlayPanSpeed2 = 0.0004f;
			this.worldPanDir1 = new Vector2(1f, 1f);
			this.worldPanDir2 = new Vector2(1f, -1f);
		}
	}
}
