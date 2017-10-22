using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class WeatherOverlay_Fog : SkyOverlay
	{
		private static readonly Material FogOverlayWorld = MatLoader.LoadMat("Weather/FogOverlayWorld", -1);

		public WeatherOverlay_Fog()
		{
			base.worldOverlayMat = WeatherOverlay_Fog.FogOverlayWorld;
			base.worldOverlayPanSpeed1 = 0.0005f;
			base.worldOverlayPanSpeed2 = 0.0004f;
			base.worldPanDir1 = new Vector2(1f, 1f);
			base.worldPanDir2 = new Vector2(1f, -1f);
		}
	}
}
