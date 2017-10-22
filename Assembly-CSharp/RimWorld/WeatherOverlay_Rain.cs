using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class WeatherOverlay_Rain : SkyOverlay
	{
		private static readonly Material RainOverlayWorld = MatLoader.LoadMat("Weather/RainOverlayWorld", -1);

		public WeatherOverlay_Rain()
		{
			base.worldOverlayMat = WeatherOverlay_Rain.RainOverlayWorld;
			base.worldOverlayPanSpeed1 = 0.015f;
			base.worldPanDir1 = new Vector2(-0.25f, -1f);
			base.worldPanDir1.Normalize();
			base.worldOverlayPanSpeed2 = 0.022f;
			base.worldPanDir2 = new Vector2(-0.24f, -1f);
			base.worldPanDir2.Normalize();
		}
	}
}
