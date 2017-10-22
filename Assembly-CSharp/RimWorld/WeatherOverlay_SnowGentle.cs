using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class WeatherOverlay_SnowGentle : SkyOverlay
	{
		private static readonly Material SnowGentleOverlayWorld = MatLoader.LoadMat("Weather/SnowOverlayWorld", -1);

		public WeatherOverlay_SnowGentle()
		{
			base.worldOverlayMat = WeatherOverlay_SnowGentle.SnowGentleOverlayWorld;
			base.worldOverlayPanSpeed1 = 0.002f;
			base.worldPanDir1 = new Vector2(-0.25f, -1f);
			base.worldPanDir1.Normalize();
			base.worldOverlayPanSpeed2 = 0.003f;
			base.worldPanDir2 = new Vector2(-0.24f, -1f);
			base.worldPanDir2.Normalize();
		}
	}
}
